#if NETCOREAPP2_0

using System.Threading.Tasks;
using BlazorAdmin.Interfaces;
using BlazorAdmin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BlazorAdmin.Services
{
    public class RoleManagementService : IRoleManagementService
    {
        private readonly HttpService _httpService;
        private readonly ILogger<RoleManagementService> _logger;

        public RoleManagementService(HttpService httpService, ILogger<RoleManagementService> logger)
        {
            _httpService = httpService;
            _logger = logger;
        }

        public async Task<RoleListResponse> List()
        {
            _logger.LogInformation("Fetching roles");
            var response = await _httpService.HttpGet<RoleListResponse>("roles");
            return response;
        }

        public async Task<CreateRoleResponse> Create(CreateRoleRequest newRole)
        {
            var response = await _httpService.HttpPost<CreateRoleResponse>("roles", newRole);
            return response;
        }

        public async Task<IdentityRole> Edit(IdentityRole role)
        {
            return await _httpService.HttpPut<IdentityRole>("roles", role);
        }

        public async Task Delete(string id)
        {
            await _httpService.HttpDelete("roles/" + id);
        }

        public async Task<GetByIdRoleResponse> GetById(string id)
        {
            var roleById = await _httpService.HttpGet<GetByIdRoleResponse>("roles/" + id);
            return roleById;
        }

        public async Task<GetRoleMembershipResponse> GetMembershipByName(string name)
        {
            var roleMembershipByName = await _httpService.HttpGet<GetRoleMembershipResponse>("roles/" + name + "/members");
            return roleMembershipByName;
        }

        public async Task DeleteUserFromRole(string userId, string roleId)
        {
            await _httpService.HttpDelete("roles/" + roleId + "/members/" + userId);
        }
    }
}

#endif
