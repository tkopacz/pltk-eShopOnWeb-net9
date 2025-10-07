#if NETCOREAPP2_0

namespace Microsoft.eShopWeb.PublicApi.RoleManagementEndpoints
{
    public class GetByIdRoleRequest : BaseRequest
    {
        public GetByIdRoleRequest(string roleId)
        {
            RoleId = roleId;
        }

        public string RoleId { get; private set; }
    }
}

#endif
