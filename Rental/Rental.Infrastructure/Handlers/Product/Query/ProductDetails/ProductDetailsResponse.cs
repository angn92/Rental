using Newtonsoft.Json;
using Rental.Core.Enum;

namespace Rental.Infrastructure.Handlers.Product.Query.ProductDetails
{
    public class ProductDetailsResponse
    {
        public ProductDetail ProductDetail { get; set; }
    }

    public class ProductDetail
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("availableQuantity")]
        public int AvailableQuantity { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("status")]
        public ProductStatus Status { get; set; }
    }
}