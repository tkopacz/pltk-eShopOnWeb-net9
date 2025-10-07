#if NETCOREAPP2_0

using System;
using System.Collections.Generic;

namespace Microsoft.eShopWeb.PublicApi.UserManagementEndpoints
{
    public class UserListResponse : BaseResponse
    {
        public UserListResponse(Guid correlationId) : base(correlationId)
        {
            Users = new List<UserDto>();
        }

        public UserListResponse()
        {
            Users = new List<UserDto>();
        }

        public List<UserDto> Users { get; set; }
    }
}

#endif
