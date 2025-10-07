#if NETCOREAPP2_0

namespace Microsoft.eShopWeb.PublicApi.UserManagementEndpoints
{
    public class GetRolesByUserIdRequest : BaseRequest
    {
        public GetRolesByUserIdRequest(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; private set; }
    }
}

#endif
