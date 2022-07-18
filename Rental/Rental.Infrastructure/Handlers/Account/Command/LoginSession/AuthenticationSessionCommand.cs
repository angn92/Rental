using Newtonsoft.Json;
using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Account.Command.LoginSession
{
    public class AuthenticationSessionCommand : ICommand
    {
        public AuthenticationSessionRequest Request { get; set; }
    }

    public class AuthenticationSessionRequest
    {
        [JsonProperty("SessionId")]
        public string SessionId { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

    }
}
