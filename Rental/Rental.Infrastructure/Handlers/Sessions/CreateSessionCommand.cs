using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Sessions
{
    public class CreateSessionCommand : ICommand
    {
        public string Username { get; set; }
    }
}
