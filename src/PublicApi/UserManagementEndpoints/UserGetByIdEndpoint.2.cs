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
using Microsoft.eShopWeb.PublicApi.UserManagementEndpoints.Models;

namespace Microsoft.eShopWeb.PublicApi.UserManagementEndpoints
{
    public class UserGetByIdEndpoint : Endpoint<GetByIdUserRequest, Results<Ok<GetUserResponse>, NotFound>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserGetByIdEndpoint(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override void Configure()
        {
            Get("api/users/{userId}");
            Roles(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            Description(d =>
                d.Produces<GetUserResponse>()
                 .WithTags("UserManagementEndpoints"));
        }

        public override async Task<Results<Ok<GetUserResponse>, NotFound>> ExecuteAsync(GetByIdUserRequest request, CancellationToken ct)
        {
            var response = new GetUserResponse(request.CorrelationId());

            var userResponse = await _userManager.FindByIdAsync(request.UserId);
            if (userResponse == null)
            {
                return TypedResults.NotFound();
            }

            response.User = userResponse.ToUserDto();
            return TypedResults.Ok(response);
        }
    }
}

#endif
