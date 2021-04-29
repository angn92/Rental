using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Password
{
    public class ChangePassword : ICommand
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
