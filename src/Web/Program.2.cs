#if NETCOREAPP2_0

using System.Collections.Generic;
using Ardalis.ListStartupServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.Web.Areas.Identity.Helpers;
using Microsoft.eShopWeb.Web.Configuration;
using Microsoft.eShopWeb.Web.Extensions;
using NimblePros.Metronome;

namespace Microsoft.eShopWeb.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddAspireServiceDefaults();

            builder.Services.AddDatabaseContexts(builder.Environment, builder.Configuration);

            builder.Services.AddCookieSettings();
            builder.Services.AddCookieAuthentication();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                       .AddDefaultUI()
                       .AddEntityFrameworkStores<AppIdentityDbContext>()
                       .AddDefaultTokenProviders();

            var gitHubClientId = builder.Configuration["GitHub:ClientId"] ?? string.Empty;

            if (!string.IsNullOrEmpty(gitHubClientId))
            {
                builder.Services.AddAuthentication()
                    .AddOAuth("GitHub", "GitHub", options =>
                    {
                        options.ClientId = gitHubClientId;
                        options.ClientSecret = builder.Configuration["GitHub:ClientSecret"] ?? string.Empty;
                        options.CallbackPath = "/signin-github";
                        options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                        options.TokenEndpoint = "https://github.com/login/oauth/access_token";
                        options.UserInformationEndpoint = "https://api.github.com/user";
                        options.UsePkce = false;
                        options.SaveTokens = true;
                        options.ClaimsIssuer = "GitHub";
                        options.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
                        {
                            OnCreatingTicket = GitHubClaimsHelper.OnOAuthCreatingTicket
                        };
                    });
            }

            builder.Services.AddScoped<ITokenClaimsService, IdentityTokenClaimService>();
            builder.Services.AddCoreServices(builder.Configuration);
            builder.Services.AddWebServices(builder.Configuration);

            builder.Services.AddMemoryCache();
            builder.Services.AddRouting(options =>
            {
                options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
            });

            builder.Services.AddMvc(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(
                         new SlugifyParameterTransformer()));
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/Basket/Checkout");
            });
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddCustomHealthChecks();

            builder.Services.Configure<ServiceConfig>(config =>
            {
                config.Services = new List<ServiceDescriptor>(builder.Services);
                config.Path = "/allservices";
            });

            builder.Services.AddBlazor(builder.Configuration);

            builder.Services.AddMetronome();
            builder.AddSeqEndpoint("seq");

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            var app = builder.Build();

            app.Logger.LogInformation("App created...");

            app.SeedDatabaseAsync().GetAwaiter().GetResult();

            var catalogBaseUrl = builder.Configuration.GetValue(typeof(string), "CatalogBaseUrl") as string;
            if (!string.IsNullOrEmpty(catalogBaseUrl))
            {
                app.Use((context, next) =>
                {
                    context.Request.PathBase = new PathString(catalogBaseUrl);
                    return next();
                });
            }

            app.UseCustomHealthChecks();

            app.UseTroubleshootingMiddlewares();

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<UserContextEnrichmentMiddleware>();
            app.MapControllerRoute("default", "{controller:slugify=Home}/{action:slugify=Index}/{id?}");
            app.MapRazorPages();
            app.MapHealthChecks("home_page_health_check", new HealthCheckOptions { Predicate = check => check.Tags.Contains("homePageHealthCheck") });
            app.MapHealthChecks("api_health_check", new HealthCheckOptions { Predicate = check => check.Tags.Contains("apiHealthCheck") });
            app.MapFallbackToFile("index.html");

            app.Logger.LogInformation("LAUNCHING");
            app.Run();
        }
    }
}

#endif
