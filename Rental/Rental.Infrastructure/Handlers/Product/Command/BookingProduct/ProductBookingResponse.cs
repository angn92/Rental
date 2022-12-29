using Newtonsoft.Json;
using System;

namespace Rental.Infrastructure.Handlers.Product.Command.BookingProduct
{
    public class ProductBookingResponse
    {
        [JsonProperty("OrderId")]
        public string OrderId { get; set; }

        [JsonProperty("OrderTime")]
        public DateTime? OrderTime { get; set; }

        [JsonProperty("ProductName")]
        public string ProductName { get; set; }

        [JsonProperty("Amount")]
        public int Amount { get; set; }

        [JsonProperty("NumberDays")]
        public TimeSpan? NumberDays { get; set; }

        [JsonProperty("Owner")]
        public string Owner { get; set; }
    }
}
