#if NETCOREAPP2_0

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
    public class DeleteUserEndpoint : Endpoint<DeleteUserRequest, Results<NoContent, NotFound, BadRequest>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserEndpoint(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override void Configure()
        {
            Delete("api/users/{userId}");
            Roles(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            Description(d =>
            {
                d.Produces(StatusCodes.Status204NoContent);
                d.Produces(StatusCodes.Status404NotFound);
                d.Produces(StatusCodes.Status409Conflict);
                d.WithTags("UserManagementEndpoints");
            });
        }

        public override async Task<Results<NoContent, NotFound, BadRequest>> ExecuteAsync(DeleteUserRequest request, CancellationToken ct)
        {
            var userToDelete = await _userManager.FindByIdAsync(request.UserId);
            if (userToDelete == null || string.IsNullOrEmpty(userToDelete.UserName))
            {
                return TypedResults.NotFound();
            }

            if (userToDelete.UserName == "admin@microsoft.com")
            {
                return TypedResults.BadRequest();
            }

            await _userManager.DeleteAsync(userToDelete);

            return TypedResults.NoContent();
        }
    }
}

#endif
