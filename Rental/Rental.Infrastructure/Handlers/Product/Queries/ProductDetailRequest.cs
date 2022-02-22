using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Product.Queries
{
    public class ProductDetailRequest : IQuery
    {
        public string ProductName { get; set; }
    }
}
