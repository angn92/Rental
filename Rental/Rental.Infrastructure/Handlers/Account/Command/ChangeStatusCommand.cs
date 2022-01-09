using Rental.Core.Enum;
using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Account.Command
{
    public class ChangeStatusCommand : ICommand
    {
        public ChangeStatusRequest ChangeStatusRequest { get; }

        public ChangeStatusCommand(ChangeStatusRequest request)
        {
            ChangeStatusRequest = request;
        }
    }

    public class ChangeStatusRequest
    {
        public string Username { get; set; }
        public AccountStatus Status { get; set; }
    }
}
