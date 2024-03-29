﻿using Newtonsoft.Json;
using Rental.Infrastructure.DTO;

namespace Rental.Infrastructure.Handlers.Orders.Query.OrderDetails
{
    public class OrderDetailsResponse
    {
        [JsonProperty("OrderDetails")]
        public OrderDetailDto OrderDetailDto { get; set; }
    }
}
