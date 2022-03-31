using Newtonsoft.Json;
using System.Collections.Generic;

namespace Rental.Infrastructure.Handlers.Product.Queries
{
    public class ProductDetailsResponse
    {
        public List<ProductDetail> ProductDetailsList { get; set; }
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
        public Status Status { get; set; }
    }

    public enum Status
    {
        Available = 1,
        InUse
    }
}