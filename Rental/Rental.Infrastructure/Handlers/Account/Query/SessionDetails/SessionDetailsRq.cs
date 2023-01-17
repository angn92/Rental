using Newtonsoft.Json;
using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Account.Query.SessionDetails
{
    public class SessionDetailsRq : IQuery
    {
        [JsonProperty("SessionIdentifier", Required = Required.Always)]
        public string SessionIdentifier { get; set; }
    }
}
