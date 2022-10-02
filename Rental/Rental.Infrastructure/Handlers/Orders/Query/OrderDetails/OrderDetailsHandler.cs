using Rental.Core.Validation;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Orders.Query.OrderDetails
{
    public class OrderDetailsHandler : IQueryHandler<OrderDetailsRq, OrderDetailsRs>
    {
        private readonly ApplicationDbContext context;
        private readonly ISessionHelper sessionHelper;
        private readonly IOrderHelper orderHelper;
        private readonly IProductHelper productHelper;

        public OrderDetailsHandler(ApplicationDbContext context, ISessionHelper sessionHelper, IOrderHelper orderHelper,
                                    IProductHelper productHelper)
        {
            this.context = context;
            this.sessionHelper = sessionHelper;
            this.orderHelper = orderHelper;
            this.productHelper = productHelper;
        }

        public async ValueTask<OrderDetailsRs> HandleAsync(OrderDetailsRq query, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNullOrEmpty(query.OrderId);
            ValidationParameter.FailIfNullOrEmpty(query.SessionId.ToString());

            //Get session by id
            var session = await sessionHelper.GetSessionByIdAsync(context, query.SessionId);

            var isExpired = sessionHelper.SessionExpired(session);

            if (isExpired)
                throw new CoreException(ErrorCode.SessionExpired, $"Given session {session.SessionId} expired.");

            //Check session status, we can get order only when session is active
            sessionHelper.ValidateSession(session);

            var order = await orderHelper.GetOrderByIdAsync(context, query.OrderId);

            if (order == null)
                throw new CoreException(ErrorCode.OrderNotExist, "Order not exist.");

            var product = await productHelper.GetProductAsync(context, order.ProductId);

            var response = new OrderDetailsRs
            {
                OrderId = order.OrderId,
                OrderProduct = new OrderProduct
                {
                    Name = product.Name,
                    Owner = product.Customer.FirstName + " " + product.Customer.LastName,
                },
                OrderStatus = order.OrderStatus.ToString(),
                ValidTo = order.ValidTo
            };

            return response;
        }
    }
}
