using JetBrains.Annotations;
using Newtonsoft.Json;
using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Account.Command.LoginSession
{
    public class AuthenticationSessionCommand : ICommand
    {
        public AuthenticationSessionRequest Request { get; set; }

        [JsonProperty("SessionIdentifier")]
        public string SessionId { get; set; }
    }

    public class AuthenticationSessionRequest
    {
        
        [JsonProperty("Username")]
        [NotNull]
        public string Username { get; set; }

        [JsonProperty("Password")]
        [NotNull]
        public string Password { get; set; }

    }
}
