using Newtonsoft.Json;
using System;

namespace Rental.Infrastructure.DTO
{
    public class OrderDetailDto
    {
        [JsonProperty("OrderId")]
        public string OrderId { get; set; }

        [JsonProperty("OrderStatus")]
        public string OrderStatus { get; set; }

        [JsonProperty("OrderProduct")]
        public OrderProduct OrderProduct { get; set; }

        [JsonProperty("ValidTo")]
        public DateTime? ValidTo { get; set; }
    }

    public class OrderProduct
    {
        public string Name { get; set; }
        public string Owner { get; set; }

    }
}
