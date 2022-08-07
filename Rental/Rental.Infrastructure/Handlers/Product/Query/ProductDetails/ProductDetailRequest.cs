using Newtonsoft.Json;
using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Product.Query.ProductDetails
{
    public class ProductDetailRequest : IQuery
    {
        [JsonProperty("productId")]
        public string ProductId { get; set; }
    }
}
