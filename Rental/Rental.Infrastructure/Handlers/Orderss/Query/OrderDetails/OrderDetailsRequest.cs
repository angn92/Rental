using Newtonsoft.Json;
using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Orders.Query.OrderDetails
{
    public class OrderDetailsRequest : IQuery
    {
        [JsonProperty("orderId", Required = Required.Always)]
        public string OrderId { get; set; }

        [JsonProperty("sessionId", Required = Required.Always)]
        public string SessionId { get; set; }
    }
}
