using Rental.Core.Validation;
using Rental.Infrastructure.DTO;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Orders.Query.OrderDetails
{
    public class OrderDetailsHandler : IQueryHandler<OrderDetailsRequest, OrderDetailsResponse>
    {
        private readonly ApplicationDbContext context;
        private readonly ISessionHelper sessionHelper;
        private readonly IOrderHelper orderHelper;
        private readonly ProductHelper productHelper;

        public OrderDetailsHandler(ApplicationDbContext context, ISessionHelper sessionHelper, IOrderHelper orderHelper,
                                    ProductHelper productHelper)
        {
            this.context = context;
            this.sessionHelper = sessionHelper;
            this.orderHelper = orderHelper;
            this.productHelper = productHelper;
        }

        public async ValueTask<OrderDetailsResponse> HandleAsync(OrderDetailsRequest query, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNullOrEmpty(query.OrderId);
            ValidationParameter.FailIfNullOrEmpty(query.SessionId.ToString());

            //Get session by id
            var session = await sessionHelper.GetSessionByIdAsync(query.SessionId);

            var isExpired = sessionHelper.SessionExpired(session);

            if (isExpired)
                throw new CoreException(ErrorCode.SessionExpired, $"Given session {session.SessionIdentifier} expired.");

            //Check session status, we can get order only when session is active
            sessionHelper.ValidateSessionStatus(session);

            var order = await orderHelper.GetOrderByIdAsync(context, query.OrderId);

            if (order == null)
                throw new CoreException(ErrorCode.OrderNotExist, "Order not exist.");

            var product = await productHelper.GetProductAsync(order.ProductId);

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
