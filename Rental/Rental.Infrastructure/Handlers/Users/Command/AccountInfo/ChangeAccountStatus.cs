using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Users.Command.AccountInfo
{
    public class ChangeAccountStatus : ICommand
    {
        public string Username { get; set; }
    }
}
