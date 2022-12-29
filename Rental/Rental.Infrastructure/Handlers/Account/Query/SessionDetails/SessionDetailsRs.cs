using Newtonsoft.Json;
using System;

namespace Rental.Infrastructure.Handlers.Account.Query.SessionDetails
{
    public class SessionDetailsRs
    {
        [JsonProperty("SessionStatus")]
        public string SessionStatus { get; set; }

        [JsonProperty("SessionId")]
        public int SessionId { get; set; }

        [JsonProperty("ValidTo")]
        public DateTime ValidTo { get; set; }
    }
}
