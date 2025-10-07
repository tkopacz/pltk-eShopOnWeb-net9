#if NETCOREAPP2_0

namespace BlazorAdmin.Models
{
    public class UpdateUserRequest
    {
        public UpdateUserRequest()
        {
            User = new User();
        }

        public User User { get; set; }
    }
}

#endif
