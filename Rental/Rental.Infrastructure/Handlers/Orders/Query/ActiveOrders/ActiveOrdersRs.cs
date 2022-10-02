using Newtonsoft.Json;
using Rental.Infrastructure.DTO;
using System.Collections.Generic;

namespace Rental.Infrastructure.Handlers.Orders.Query.ActiveOrders
{
    public class ActiveOrdersRs
    {
        [JsonProperty("orderDetailDtoList", Required = Required.Always)]
        public List<OrderDetailDto> OrderDetailDtoList { get; set; }
    }
}
