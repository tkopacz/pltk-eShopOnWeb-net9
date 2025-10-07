#if NETCOREAPP2_0

namespace Microsoft.eShopWeb.PublicApi.RoleManagementEndpoints
{
    public class DeleteRoleRequest : BaseRequest
    {
        public DeleteRoleRequest(string roleId)
        {
            RoleId = roleId;
        }

        public string RoleId { get; private set; }
    }
}

#endif
