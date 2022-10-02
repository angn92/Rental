﻿using Newtonsoft.Json;
using System;

namespace Rental.Infrastructure.Handlers.Orders.Query.OrderDetails
{
    public class OrderDetailsRs
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("orderStatus")]
        public string OrderStatus { get; set; }

        [JsonProperty("orderProduct")]
        public OrderProduct OrderProduct { get; set; }

        [JsonProperty("validTo")]
        public DateTime? ValidTo { get; set; }

    }

    public class OrderProduct
    {
        public string Name { get; set; }
        public string Owner { get; set; }

    }
}
