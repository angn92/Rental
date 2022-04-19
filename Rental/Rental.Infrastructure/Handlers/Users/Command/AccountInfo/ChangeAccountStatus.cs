using Newtonsoft.Json;
using Rental.Core.Enum;
using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Users.Command.AccountInfo
{
    public class ChangeAccountStatus : ICommand
    {
        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Status")]
        public AccountStatus Status { get; set; }
    }
}
