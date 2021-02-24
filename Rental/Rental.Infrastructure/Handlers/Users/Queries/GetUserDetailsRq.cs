using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Users.Queries
{
    public class GetUserDetailsRq : ICommand
    {
        public string Nick { get; set; }
    }
}
