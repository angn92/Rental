using Newtonsoft.Json;
using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Account.Query.SessionDetails
{
    public class SessionDetailsRq : IQuery
    {
        [JsonProperty("Session", Required = Required.Always)]
        public int Session { get; set; }
    }
}
