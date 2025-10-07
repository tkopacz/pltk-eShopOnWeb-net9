#if NETCOREAPP2_0

namespace Microsoft.eShopWeb.PublicApi.UserManagementEndpoints
{
    public class GetByIdUserRequest : BaseRequest
    {
        public GetByIdUserRequest(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; private set; }
    }
}

#endif
