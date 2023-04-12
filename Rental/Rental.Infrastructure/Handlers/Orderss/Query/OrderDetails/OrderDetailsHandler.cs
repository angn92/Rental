using Rental.Core.Validation;
using Rental.Infrastructure.DTO;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Wrapper;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Orders.Query.OrderDetails
{
    public class OrderDetailsHandler : IQueryHandler<OrderDetailsRequest, OrderDetailsResponse>
    {
        private readonly IHttpContextWrapper _httpContextWrapper;
        private readonly ISessionHelper _sessionHelper;
        private readonly IOrderHelper _orderHelper;
        private readonly IProductHelper _productHelper;

        public OrderDetailsHandler(IHttpContextWrapper httpContextWrapper, ISessionHelper sessionHelper, IOrderHelper orderHelper, IProductHelper productHelper)
        {
            _httpContextWrapper = httpContextWrapper;
            _sessionHelper = sessionHelper;
            _orderHelper = orderHelper;
            _productHelper = productHelper;
        }

        public async ValueTask<OrderDetailsResponse> HandleAsync(OrderDetailsRequest query, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNullOrEmpty(query.OrderId);

            var sessionId = _httpContextWrapper.GetValueFromRequestHeader("SessionId");

            var session = await _sessionHelper.GetSessionByIdAsync(sessionId);
            _sessionHelper.ValidateSessionStatus(session);
            
            var order = await _orderHelper.GetOrderAsync(query.OrderId);

            var product = await _productHelper.GetProductAsync(order.ProductId);

            var response = new OrderDetailsResponse
            {
                OrderDetailDto = new OrderDetailDto
                {
                    OrderId = order.OrderId,
                    OrderProduct = new OrderProduct
                    {
                        Name = product.Name,
                        Owner = product.Customer.FirstName + " " + product.Customer.LastName,
                    },
                    OrderStatus = order.OrderStatus.ToString(),
                    ValidTo = order.ValidTo
                }
            };

            return response;
        }
    }
}
