using JetBrains.Annotations;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Query;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Product.Queries
{
    public class ProductDetailsHandler : IQueryHandler<ProductDetailRequest, ProductDetailsResponse>
    {
        public ApplicationDbContext Context { get; }

        public ProductDetailsHandler([NotNull] ApplicationDbContext context)
        {
            Context = context;
        }

        public Task<ProductDetailsResponse> HandleAsync(ProductDetailRequest query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
