#if NETCOREAPP2_0

using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.PublicApi.Extensions;

namespace Microsoft.eShopWeb.PublicApi.UserManagementEndpoints
{
    public class UpdateRoleEndpoint : Endpoint<UpdateUserRequest, Results<Ok<UpdateUserResponse>, NotFound>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateRoleEndpoint(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override void Configure()
        {
            Put("api/users");
            Roles(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            Description(d =>
                d.Produces<UpdateUserResponse>()
                 .WithTags("UserManagementEndpoints"));
        }

        public override async Task<Results<Ok<UpdateUserResponse>, NotFound>> ExecuteAsync(UpdateUserRequest request, CancellationToken ct)
        {
            var response = new UpdateUserResponse(request.CorrelationId());

            if (request == null || request.User == null || request.User.Id == null)
            {
                return TypedResults.NotFound();
            }

            var existingUser = await _userManager.FindByIdAsync(request.User.Id);
            if (existingUser == null)
            {
                return TypedResults.NotFound();
            }

            existingUser.FromUserDto(request.User);

            await _userManager.UpdateAsync(existingUser);
            var updatedUser = await _userManager.FindByIdAsync(existingUser.Id);
            if (updatedUser == null)
            {
                return TypedResults.NotFound();
            }

            response.User = updatedUser;
            return TypedResults.Ok(response);
        }
    }
}

#endif
