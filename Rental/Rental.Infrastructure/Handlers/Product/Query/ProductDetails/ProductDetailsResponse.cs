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
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Quantity")]
        public int Quantity { get; set; }

        [JsonProperty("AvailableQuantity")]
        public int AvailableQuantity { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("Owner")]
        public string Owner { get; set; }

        [JsonProperty("Status")]
        public ProductStatus Status { get; set; }
    }
}