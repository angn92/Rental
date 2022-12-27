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
        [JsonProperty("Username", Required = Required.Always)]
        public string Username { get; set; }

        [JsonProperty("AccountStatus", Required = Required.Always)]
        public AccountStatus Status { get; set; }

        [JsonProperty("Reason", Required = Required.Always)]
        public string Reason { get; set; }
    }
}
