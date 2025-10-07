#if NETCOREAPP2_0

namespace Microsoft.eShopWeb.PublicApi.RoleMembershipEndpoints
{
    public class DeleteUserFromRoleRequest : BaseRequest
    {
        public DeleteUserFromRoleRequest(string userId, string roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public string UserId { get; private set; }

        public string RoleId { get; private set; }
    }
}

#endif
