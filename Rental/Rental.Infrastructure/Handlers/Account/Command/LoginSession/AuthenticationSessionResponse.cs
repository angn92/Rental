using Newtonsoft.Json;
using System;

namespace Rental.Infrastructure.Handlers.Account.Command.LoginSession
{
    public class AuthenticationSessionResponse
    {
        [JsonProperty("SessionId")]
        public string SessionId { get; set; }

        [JsonProperty("SessionState")]
        public string SessionState { get; set; }

        [JsonProperty("ExpirationTime")]
        public DateTime? ExpirationTime { get; set; }
    }
}
