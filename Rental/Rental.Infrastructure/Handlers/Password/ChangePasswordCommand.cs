using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Password
{
    public class ChangePasswordCommand : ICommand
    {
        public string Session { get; set; }
        public string NewPassword { get; set; }
    }
}
