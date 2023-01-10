using Newtonsoft.Json;
using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Account.Command.AuthorizePassword
{
    public class AuthorizePasswordCommand : ICommand
    {
        public AuthorizePasswordRequest Request { get; set; }

        [JsonProperty("SessionIdentifier")]
        public int SessionId { get; set; }
    }

    public class AuthorizePasswordRequest
    {
        [JsonProperty("username", Required = Required.Always)]
        public string Username { get; set; }

        [JsonProperty("code", Required = Required.Always)]
        public string Code { get; set; }
    }
}
