using Newtonsoft.Json;
using System;

namespace Rental.Infrastructure.Handlers.Account.Query.SessionDetails
{
    public class SessionDetailsRs
    {
        [JsonProperty("sessionStatus")]
        public string SessionStatus { get; set; }

        [JsonProperty("sessionId")]
        public int SessionId { get; set; }

        [JsonProperty("validTo")]
        public DateTime ValidTo { get; set; }
    }
}
