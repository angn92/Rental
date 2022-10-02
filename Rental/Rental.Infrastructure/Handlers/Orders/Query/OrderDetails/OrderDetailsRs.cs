using Newtonsoft.Json;
using Rental.Infrastructure.DTO;

namespace Rental.Infrastructure.Handlers.Orders.Query.OrderDetails
{
    public class OrderDetailsRs
    {
        [JsonProperty("orderDetailDto")]
        public OrderDetailDto OrderDetailDto { get; set; }
    }
}
