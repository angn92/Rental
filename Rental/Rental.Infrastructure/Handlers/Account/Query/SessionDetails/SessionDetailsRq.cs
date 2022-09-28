using Newtonsoft.Json;
using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Account.Query.SessionDetails
{
    public class SessionDetailsRq : IQuery
    {
        [JsonProperty("session", Required = Required.Always)]
        public int Session { get; set; }
    }
}
