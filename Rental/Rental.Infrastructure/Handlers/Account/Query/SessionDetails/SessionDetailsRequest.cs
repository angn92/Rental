using Newtonsoft.Json;
using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Account.Query.SessionDetails
{
    public class SessionDetailsRequest : IQuery
    {
        [JsonProperty("SessionId", Required = Required.Always)]
        public string SessionId { get; set; }
    }
}
