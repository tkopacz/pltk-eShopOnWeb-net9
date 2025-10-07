#if NETCOREAPP2_0

namespace BlazorAdmin.Models
{
    public class UserForMembership
    {
        public UserForMembership(string userName, string id)
        {
            UserName = userName;
            Id = id;
        }

        public string UserName { get; private set; }

        public string Id { get; private set; }
    }
}

#endif
