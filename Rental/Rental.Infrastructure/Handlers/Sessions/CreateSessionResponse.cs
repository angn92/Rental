using Newtonsoft.Json;
using Rental.Core.Enum;
using System;

namespace Rental.Infrastructure.Handlers.Sessions
{
    public class CreateSessionResponse
    {
        [JsonProperty("IdSession")]
        public int IdSession { get; set; }

        [JsonProperty("Status")]
        public SessionState Status { get; set; }

        [JsonProperty("ExpiredTime")]
        public DateTime ExpierdTime { get; set; }
    }
}
