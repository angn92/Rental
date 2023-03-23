using JetBrains.Annotations;
using Rental.Core.Validation;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Wrapper;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Product.Query.ProductDetails
{
    public class ProductDetailsHandler : IQueryHandler<ProductDetailRequest, ProductDetailsResponse>
    {
        private readonly IHttpContextWrapper _httpContextWrapper;
        private readonly IProductHelper _productHelper;
        private readonly ISessionHelper _sessionHelper;

        public ProductDetailsHandler(IHttpContextWrapper httpContextWrapper, IProductHelper productHelper, ISessionHelper sessionHelper)
        {
            _httpContextWrapper = httpContextWrapper;
            _productHelper = productHelper;
            _sessionHelper = sessionHelper;
        }

        public async ValueTask<ProductDetailsResponse> HandleAsync([NotNull] ProductDetailRequest query, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(query);

            var sessionId = _httpContextWrapper.GetValueFromRequestHeader("SessionId");
            var session = await _sessionHelper.GetSessionByIdAsync(sessionId);
            _sessionHelper.ValidateSessionStatus(session);

            var product = await _productHelper.GetProductAsync(query.ProductId);

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
