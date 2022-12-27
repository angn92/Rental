using Newtonsoft.Json;
using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Account.Command.Sessions
{
    public class CreateSessionCommand : ICommand
    {
        [JsonProperty("Username")]
        public string Username { get; set; }
    }
}
