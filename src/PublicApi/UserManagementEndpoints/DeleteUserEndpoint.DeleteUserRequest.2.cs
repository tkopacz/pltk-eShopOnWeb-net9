#if NETCOREAPP2_0

namespace Microsoft.eShopWeb.PublicApi.UserManagementEndpoints
{
    public class DeleteUserRequest : BaseRequest
    {
        public DeleteUserRequest(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; private set; }
    }
}

#endif
