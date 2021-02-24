using Rental.Core.Enum;

namespace Rental.Infrastructure.Handlers.Users.Queries
{
    public class GetUserDetailsRs
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public AccountStatus Status { get; set; }
    }
}
