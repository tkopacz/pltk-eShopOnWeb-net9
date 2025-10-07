#if NETCOREAPP2_0

namespace BlazorAdmin.Models
{
    public class CreateUserRequest
    {
        public CreateUserRequest()
        {
            User = new User();
        }

        public User User { get; set; }
    }
}

#endif
