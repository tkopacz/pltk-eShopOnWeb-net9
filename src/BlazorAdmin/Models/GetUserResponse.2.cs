#if NETCOREAPP2_0

namespace BlazorAdmin.Models
{
    public class GetUserResponse
    {
        public GetUserResponse()
        {
            User = new User();
        }

        public User User { get; set; }
    }
}

#endif
