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

namespace Microsoft.eShopWeb.PublicApi.RoleMembershipEndpoints
{
    public class RoleMembershipGetByNameEndpoint : Endpoint<GetRoleMembershipRequest, Results<Ok<GetRoleMembershipResponse>, NotFound>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleMembershipGetByNameEndpoint(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override void Configure()
        {
            Get("api/roles/{roleName}/members");
            Roles(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            Description(d =>
                d.Produces<GetRoleMembershipResponse>()
                 .WithTags("RoleManagementEndpoints"));
        }

        public override async Task<Results<Ok<GetRoleMembershipResponse>, NotFound>> ExecuteAsync(GetRoleMembershipRequest request, CancellationToken ct)
        {
            var response = new GetRoleMembershipResponse(request.CorrelationId());
            var members = await _userManager.GetUsersInRoleAsync(request.RoleName);

            if (members == null)
            {
                return TypedResults.NotFound();
            }

            response.RoleMembers = members.ToList();
            return TypedResults.Ok(response);
        }
    }
}

#endif
