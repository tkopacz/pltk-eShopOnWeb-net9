#if NETCOREAPP2_0

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.eShopWeb.ApplicationCore.Exceptions;
using Microsoft.eShopWeb.Infrastructure.Identity;

namespace Microsoft.eShopWeb.PublicApi.RoleManagementEndpoints
{
    public class DeleteRoleEndpoint : Endpoint<DeleteRoleRequest, Results<NoContent, NotFound>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteRoleEndpoint(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public override void Configure()
        {
            Delete("api/roles/{roleId}");
            Roles(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            Description(d =>
            {
                d.Produces(StatusCodes.Status204NoContent);
                d.Produces(StatusCodes.Status404NotFound);
                d.WithTags("RoleManagementEndpoints");
            });
        }

        public override async Task<Results<NoContent, NotFound>> ExecuteAsync(DeleteRoleRequest request, CancellationToken ct)
        {
            var roleToDelete = await _roleManager.FindByIdAsync(request.RoleId);
            if (roleToDelete == null)
            {
                return TypedResults.NotFound();
            }

            if (string.IsNullOrEmpty(roleToDelete.Name))
            {
                throw new Exception("Unknown role to delete");
            }

            var usersWithRole = await _userManager.GetUsersInRoleAsync(roleToDelete.Name);
            if (usersWithRole.Any())
            {
                throw new RoleStillAssignedException("The " + roleToDelete.Name + " role is in use and cannot be deleted.");
            }

            await _roleManager.DeleteAsync(roleToDelete);

            return TypedResults.NoContent();
        }
    }
}

#endif
