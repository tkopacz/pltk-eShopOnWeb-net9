#if NETCOREAPP2_0

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.PublicApi.UserManagementEndpoints.Models;

namespace Microsoft.eShopWeb.PublicApi.UserManagementEndpoints
{
    public class UserGetRolesByIdEndpoint : Endpoint<GetRolesByUserIdRequest, Results<Ok<GetUserRolesResponse>, NotFound>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserGetRolesByIdEndpoint(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override void Configure()
        {
            Get("api/users/{userId}/roles");
            Roles(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            Description(d =>
                d.Produces<GetUserResponse>()
                 .WithTags("UserManagementEndpoints"));
        }

        public override async Task<Results<Ok<GetUserRolesResponse>, NotFound>> ExecuteAsync(GetRolesByUserIdRequest request, CancellationToken ct)
        {
            var response = new GetUserRolesResponse(request.CorrelationId());

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return TypedResults.NotFound();
            }

            var rolesForUser = await _userManager.GetRolesAsync(user);
            if (rolesForUser == null)
            {
                return TypedResults.NotFound();
            }

            response.Roles = rolesForUser.ToList();
            return TypedResults.Ok(response);
        }
    }
}

#endif
