#if NETCOREAPP2_0

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Microsoft.eShopWeb.PublicApi.RoleManagementEndpoints
{
    public class RoleListEndpoint : EndpointWithoutRequest<RoleListResponse>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleListEndpoint(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public override void Configure()
        {
            Get("api/roles");
            Roles(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            Description(d => d.Produces<RoleListResponse>()
                .WithTags("RoleManagementEndpoints"));
        }

        public override async Task<RoleListResponse> ExecuteAsync(CancellationToken ct)
        {
            await Task.Delay(1000, ct);
            var response = new RoleListResponse();
            response.Roles = _roleManager.Roles.ToList();
            return response;
        }
    }
}

#endif
