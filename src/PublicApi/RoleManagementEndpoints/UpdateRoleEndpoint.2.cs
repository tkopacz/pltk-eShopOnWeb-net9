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
    public class UpdateRoleEndpoint : Endpoint<UpdateRoleRequest, Results<Ok<UpdateRoleResponse>, NotFound>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public UpdateRoleEndpoint(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public override void Configure()
        {
            Put("api/roles");
            Roles(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            Description(d =>
                d.Produces<UpdateRoleResponse>()
                 .WithTags("RoleManagementEndpoints"));
        }

        public override async Task<Results<Ok<UpdateRoleResponse>, NotFound>> ExecuteAsync(UpdateRoleRequest request, CancellationToken ct)
        {
            var response = new UpdateRoleResponse(request.CorrelationId());

            if (request == null)
            {
                return TypedResults.NotFound();
            }

            var existingRole = await _roleManager.FindByIdAsync(request.Id);
            if (existingRole == null)
            {
                return TypedResults.NotFound();
            }

            existingRole.Name = request.Name;

            await _roleManager.UpdateAsync(existingRole);
            var updatedRole = await _roleManager.FindByIdAsync(existingRole.Id);
            if (updatedRole == null)
            {
                return TypedResults.NotFound();
            }

            response.Role = updatedRole;
            return TypedResults.Ok(response);
        }
    }
}

#endif
