using Newtonsoft.Json;
using System;

namespace Rental.Infrastructure.Handlers.Product.Command.BookingProduct
{
    public class ProductBookingResponse
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("orderTime")]
        public DateTime? OrderTime { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("numberDays")]
        public TimeSpan? NumberDays { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }
    }
}
