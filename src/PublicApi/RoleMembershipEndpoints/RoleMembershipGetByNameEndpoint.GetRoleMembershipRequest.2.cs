#if NETCOREAPP2_0

namespace Microsoft.eShopWeb.PublicApi.RoleMembershipEndpoints
{
    public class GetRoleMembershipRequest : BaseRequest
    {
        public GetRoleMembershipRequest(string roleName)
        {
            RoleName = roleName;
        }

        public string RoleName { get; private set; }
    }
}

#endif
