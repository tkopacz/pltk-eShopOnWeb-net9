#if NETCOREAPP2_0

using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.eShopWeb.ApplicationCore.Exceptions;

namespace Microsoft.eShopWeb.PublicApi.RoleManagementEndpoints
{
    public class CreateRoleEndpoint : Endpoint<CreateRoleRequest, CreateRoleResponse>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateRoleEndpoint(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public override void Configure()
        {
            Post("api/roles");
            Roles(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            Description(d =>
                d.Produces<CreateRoleResponse>()
                 .WithTags("RoleManagementEndpoints"));
        }

        public override async Task HandleAsync(CreateRoleRequest request, CancellationToken ct)
        {
            var response = new CreateRoleResponse(request.CorrelationId());
            var existingRole = await _roleManager.FindByNameAsync(request.Name);
            if (existingRole != null)
            {
                throw new DuplicateException("A role with name " + request.Name + " already exists");
            }

            var newRole = new IdentityRole(request.Name);
            var createRole = await _roleManager.CreateAsync(newRole);
            if (createRole.Succeeded)
            {
                var responseRole = await _roleManager.FindByNameAsync(request.Name);
                if (responseRole != null)
                {
                    response.Role = responseRole;
                    await SendCreatedAtAsync<RoleGetByIdEndpoint>(new { RoleId = response.Role.Id }, response, cancellation: ct);
                }
            }
        }
    }
}

#endif
