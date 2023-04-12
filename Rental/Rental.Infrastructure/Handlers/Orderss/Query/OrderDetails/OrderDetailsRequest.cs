using Newtonsoft.Json;
using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Orders.Query.OrderDetails
{
    public class OrderDetailsRequest : IQuery
    {
        [JsonProperty("OrderId", Required = Required.Always)]
        public string OrderId { get; set; }
    }
}
