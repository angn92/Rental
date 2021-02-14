using Rental.Core.Enum;

namespace Rental.Infrastructure.DTO
{
    public class UserDto
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Emial { get; set; }
        public AccountStatus Status { get; set; }
    }
}
