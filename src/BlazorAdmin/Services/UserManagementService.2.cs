#if NETCOREAPP2_0

using System.Threading.Tasks;
using BlazorAdmin.Interfaces;
using BlazorAdmin.Models;
using Microsoft.Extensions.Logging;

namespace BlazorAdmin.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly HttpService _httpService;
        private readonly ILogger<IUserManagementService> _logger;

        public UserManagementService(HttpService httpService, ILogger<IUserManagementService> logger)
        {
            _httpService = httpService;
            _logger = logger;
        }

        public async Task<CreateUserResponse> Create(CreateUserRequest user)
        {
            var response = await _httpService.HttpPost<CreateUserResponse>("users", user);
            return response;
        }

        public async Task Delete(string userId)
        {
            await _httpService.HttpDelete("users/" + userId);
        }

        public async Task<GetUserResponse> Update(UpdateUserRequest user)
        {
            return await _httpService.HttpPut<GetUserResponse>("users", user);
        }

        public async Task<GetUserResponse> GetById(string userId)
        {
            return await _httpService.HttpGet<GetUserResponse>("users/" + userId);
        }

        public async Task<GetUserRolesResponse> GetRolesByUserId(string userId)
        {
            return await _httpService.HttpGet<GetUserRolesResponse>("users/" + userId + "/roles");
        }

        public async Task<GetUserResponse> GetByName(string userName)
        {
            return await _httpService.HttpGet<GetUserResponse>("users/name/" + userName);
        }

        public async Task<UserListResponse> List()
        {
            _logger.LogInformation("Fetching users");
            return await _httpService.HttpGet<UserListResponse>("users");
        }

        public async Task SaveRolesForUser(SaveRolesForUserRequest request)
        {
            await _httpService.HttpPut("users/" + request.UserId + "/roles", request);
        }
    }
}

#endif
