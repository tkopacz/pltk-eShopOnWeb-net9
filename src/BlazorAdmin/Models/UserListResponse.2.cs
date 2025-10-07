#if NETCOREAPP2_0

using System.Collections.Generic;

namespace BlazorAdmin.Models
{
    public class UserListResponse
    {
        public UserListResponse()
        {
            Users = new List<User>();
        }

        public List<User> Users { get; set; }
    }
}

#endif
