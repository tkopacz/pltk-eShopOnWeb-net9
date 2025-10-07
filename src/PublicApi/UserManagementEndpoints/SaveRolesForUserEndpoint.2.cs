#if NETCOREAPP2_0

using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.eShopWeb.Infrastructure.Identity;

namespace Microsoft.eShopWeb.PublicApi.UserManagementEndpoints
{
    public class SaveRolesForUserEndpoint : Endpoint<SaveRolesForUserRequest, Results<Ok, NotFound, BadRequest>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public SaveRolesForUserEndpoint(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override void Configure()
        {
            Put("api/users/{userId}/roles");
            Roles(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            Description(d =>
            {
                d.Produces(StatusCodes.Status200OK);
                d.Produces(StatusCodes.Status404NotFound);
                d.Produces(StatusCodes.Status409Conflict);
                d.WithTags("UserManagementEndpoints");
            });
        }

        public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(SaveRolesForUserRequest request, CancellationToken ct)
        {
            var userToUpdate = await _userManager.FindByIdAsync(request.UserId);
            if (userToUpdate == null)
            {
                return TypedResults.NotFound();
            }

            if (string.IsNullOrEmpty(userToUpdate.UserName))
            {
                throw new Exception("Unknown user to update");
            }

            if (userToUpdate.UserName == "admin@microsoft.com")
            {
                return TypedResults.BadRequest();
            }

            if (request.RolesToAdd.Count > 0)
            {
                await _userManager.AddToRolesAsync(userToUpdate, request.RolesToAdd);
            }

            if (request.RolesToRemove.Count > 0)
            {
                await _userManager.RemoveFromRolesAsync(userToUpdate, request.RolesToRemove);
            }

            return TypedResults.Ok();
        }
    }
}

#endif
