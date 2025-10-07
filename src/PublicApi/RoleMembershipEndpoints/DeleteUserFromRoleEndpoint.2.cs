#if NETCOREAPP2_0

using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.eShopWeb.Infrastructure.Identity;

namespace Microsoft.eShopWeb.PublicApi.RoleMembershipEndpoints
{
    public class DeleteUserFromRoleEndpoint : Endpoint<DeleteUserFromRoleRequest, Results<NoContent, NotFound>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserFromRoleEndpoint(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public override void Configure()
        {
            Delete("api/roles/{RoleId}/members/{UserId}");
            Roles(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            Description(d =>
            {
                d.Produces(StatusCodes.Status204NoContent);
                d.Produces(StatusCodes.Status404NotFound);
                d.WithTags("RoleManagementEndpoints");
            });
        }

        public override async Task<Results<NoContent, NotFound>> ExecuteAsync(DeleteUserFromRoleRequest request, CancellationToken ct)
        {
            var userToUpdate = await _userManager.FindByIdAsync(request.UserId);
            if (userToUpdate == null)
            {
                return TypedResults.NotFound();
            }

            var roleToUpdate = await _roleManager.FindByIdAsync(request.RoleId);
            if (roleToUpdate == null || roleToUpdate.Name == null)
            {
                return TypedResults.NotFound();
            }

            await _userManager.RemoveFromRoleAsync(userToUpdate, roleToUpdate.Name);

            return TypedResults.NoContent();
        }
    }
}

#endif
