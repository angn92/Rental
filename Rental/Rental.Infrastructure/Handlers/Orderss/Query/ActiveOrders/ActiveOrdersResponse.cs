using Newtonsoft.Json;
using Rental.Infrastructure.DTO;
using System.Collections.Generic;

namespace Rental.Infrastructure.Handlers.Orders.Query.ActiveOrders
{
    public class ActiveOrdersResponse
    {
        [JsonProperty("OrderDetailDtoList")]
        public List<OrderDetailDto> OrderDetailDtoList { get; set; }
    }
}
