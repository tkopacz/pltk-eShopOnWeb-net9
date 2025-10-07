#if NETCOREAPP2_0

using System;
using BlazorShared;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.eShopWeb.Infrastructure;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.PublicApi.Extensions;
using Microsoft.eShopWeb.PublicApi.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NimblePros.Metronome;

namespace Microsoft.eShopWeb.PublicApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddAspireServiceDefaults();

            builder.Services.AddFastEndpoints();

            builder.Configuration.AddConfigurationFile("appsettings.test.json");

            builder.Services.ConfigureLocalDatabaseContexts(builder.Configuration);

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();

            builder.Services.AddCustomServices(builder.Configuration);

            builder.Services.AddMemoryCache();

            builder.Services.AddJwtAuthentication();

            const string CORS_POLICY = "CorsPolicy";

            var configSection = builder.Configuration.GetRequiredSection(BaseUrlConfiguration.CONFIG_NAME);
            builder.Services.Configure<BaseUrlConfiguration>(configSection);
            var baseUrlConfig = configSection.Get<BaseUrlConfiguration>();
            if (baseUrlConfig == null)
            {
                baseUrlConfig = new BaseUrlConfiguration();
            }

            builder.Services.AddCorsPolicy(CORS_POLICY, baseUrlConfig);

            builder.Services.AddControllers();

            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            builder.Services.AddSwagger();

            builder.Services.AddMetronome();
            var seqUrl = builder.Configuration["Seq:ServerUrl"];
            if (string.IsNullOrEmpty(seqUrl))
            {
                seqUrl = "http://localhost:5341";
            }

            builder.AddSeqEndpoint("seq", options =>
            {
                options.ServerUrl = seqUrl;
            });

            var app = builder.Build();

            app.Logger.LogInformation("PublicApi App created...");

            app.SeedDatabaseAsync().GetAwaiter().GetResult();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CORS_POLICY);

            app.UseAuthorization();

            app.MapControllers();

            app.UseFastEndpoints();

            app.UseSwaggerGen();

            app.Logger.LogInformation("LAUNCHING PublicApi");

            app.Run();
        }
    }
}

#endif
