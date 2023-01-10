using Newtonsoft.Json;
using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Orders.Query.ActiveOrders
{
    public class ActiveOrdersRq : IQuery
    {
        [JsonProperty("Username", Required = Required.Always)]
        public string Username { get; set; }

        [JsonProperty("SessionIdentifier", Required = Required.Always)]
        public string SessionId { get; set; }
    }
}
