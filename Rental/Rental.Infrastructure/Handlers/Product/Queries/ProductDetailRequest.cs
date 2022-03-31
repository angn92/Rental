using Newtonsoft.Json;
using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Product.Queries
{
    public class ProductDetailRequest : IQuery
    {
        [JsonProperty("Name")]
        public string ProductName { get; set; }
    }
}
