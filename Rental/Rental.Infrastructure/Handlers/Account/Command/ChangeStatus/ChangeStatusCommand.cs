using Newtonsoft.Json;
using Rental.Core.Enum;
using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Account.Commmand.ChangeStatus
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
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("accountStatus")]
        public AccountStatus Status { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}
