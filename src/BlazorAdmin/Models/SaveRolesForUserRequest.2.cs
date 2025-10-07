#if NETCOREAPP2_0

using System.Collections.Generic;

namespace BlazorAdmin.Models
{
    public class SaveRolesForUserRequest
    {
        public SaveRolesForUserRequest()
        {
            RolesToAdd = new List<string>();
            RolesToRemove = new List<string>();
        }

        public string UserId { get; set; }

        public List<string> RolesToAdd { get; set; }

        public List<string> RolesToRemove { get; set; }
    }
}

#endif
