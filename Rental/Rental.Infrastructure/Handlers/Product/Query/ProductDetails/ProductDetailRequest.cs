using Newtonsoft.Json;
using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Product.Query.ProductDetails
{
    public class ProductDetailRequest : IQuery
    {
        [JsonProperty("ProductId")]
        public string ProductId { get; set; }
    }
}
