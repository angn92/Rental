using Rental.Core.Enum;

namespace Rental.Infrastructure.DTO
{
    public class CustomerAccountDto
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public AccountStatus Status { get; set; }
        public PasswordStatus PasswordStatus { get; set; }
    }
}
