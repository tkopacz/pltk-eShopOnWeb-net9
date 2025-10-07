#if NETCOREAPP2_0

using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Microsoft.eShopWeb.PublicApi.RoleManagementEndpoints
{
    public class RoleGetByIdEndpoint : Endpoint<GetByIdRoleRequest, Results<Ok<GetByIdRoleResponse>, NotFound>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleGetByIdEndpoint(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public override void Configure()
        {
            Get("api/roles/{roleId}");
            Roles(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            Description(d =>
                d.Produces<GetByIdRoleResponse>()
                 .WithTags("RoleManagementEndpoints"));
        }

        public override async Task<Results<Ok<GetByIdRoleResponse>, NotFound>> ExecuteAsync(GetByIdRoleRequest request, CancellationToken ct)
        {
            var response = new GetByIdRoleResponse(request.CorrelationId());

            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role == null)
            {
                return TypedResults.NotFound();
            }

            response.Role = role;
            return TypedResults.Ok(response);
        }
    }
}

#endif
