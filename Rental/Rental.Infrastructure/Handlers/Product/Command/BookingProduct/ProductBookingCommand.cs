using Newtonsoft.Json;
using Rental.Infrastructure.Command;
using System;

namespace Rental.Infrastructure.Handlers.Product.Command.BookingProduct
{
    public class ProductBookingCommand : ICommand
    {
        public ProductBookingRequest Request { get; set; }

        public ProductBookingCommand(ProductBookingRequest request)
        {
            Request = request;
        }
    }

    public class ProductBookingRequest
    {
        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("from")]
        public DateTime? From { get; set; }

        [JsonProperty("to")]
        public DateTime? To { get; set; }
    }
}
