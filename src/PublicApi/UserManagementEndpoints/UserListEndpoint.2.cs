#if NETCOREAPP2_0

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.PublicApi.Extensions;

namespace Microsoft.eShopWeb.PublicApi.UserManagementEndpoints
{
    public class UserListEndpoint : EndpointWithoutRequest<UserListResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserListEndpoint(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override void Configure()
        {
            Get("api/users");
            Roles(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            Description(d => d.Produces<UserListResponse>()
                .WithTags("UserManagementEndpoints"));
        }

        public override async Task<UserListResponse> ExecuteAsync(CancellationToken ct)
        {
            await Task.Delay(1000, ct);
            var response = new UserListResponse();
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                response.Users.Add(user.ToUserDto());
            }

            return response;
        }
    }
}

#endif
