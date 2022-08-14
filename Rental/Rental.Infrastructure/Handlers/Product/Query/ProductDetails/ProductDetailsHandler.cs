using JetBrains.Annotations;
using Rental.Core.Validation;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Product.Query.ProductDetails
{
    public class ProductDetailsHandler : IQueryHandler<ProductDetailRequest, ProductDetailsResponse>
    {
        private readonly ApplicationDbContext context;
        private readonly IProductHelper productHelper;

        public ProductDetailsHandler([NotNull] ApplicationDbContext context, IProductHelper productHelper)
        {
            this.context = context;
            this.productHelper = productHelper;
        }

        public async ValueTask<ProductDetailsResponse> HandleAsync([NotNull] ProductDetailRequest query, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(query);

            var product = await productHelper.GetProductAsync(context, query.ProductId);

            var productDetail = new ProductDetail
            {
                Name = product.Name,
                Owner = product.Customer.FirstName,
                Category = product.Category.Name,
                Quantity = product.Amount,
                AvailableQuantity = product.QuantityAvailable,
                Status = product.Status
            };

            return new ProductDetailsResponse
            {
                ProductDetail = productDetail
            };
        }
    }
}
