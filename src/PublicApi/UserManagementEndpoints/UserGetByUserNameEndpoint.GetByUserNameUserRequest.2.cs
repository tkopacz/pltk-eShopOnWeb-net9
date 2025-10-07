#if NETCOREAPP2_0

namespace Microsoft.eShopWeb.PublicApi.UserManagementEndpoints
{
    public class GetByUserNameUserRequest : BaseRequest
    {
        public GetByUserNameUserRequest(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; private set; }
    }
}

#endif
