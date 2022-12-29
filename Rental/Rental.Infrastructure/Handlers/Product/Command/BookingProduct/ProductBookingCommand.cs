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
        [JsonProperty("ProductId")]
        public string ProductId { get; set; }

        [JsonProperty("Amount")]
        public int Amount { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("From")]
        public DateTime? From { get; set; }

        [JsonProperty("To")]
        public DateTime? To { get; set; }
    }
}
