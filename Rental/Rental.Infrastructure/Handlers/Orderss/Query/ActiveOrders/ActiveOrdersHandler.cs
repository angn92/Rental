using Rental.Core.Validation;
using Rental.Infrastructure.DTO;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Handlers.Orders.Query.ActiveOrders;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Services.CustomerService;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Orderss.Query.ActiveOrders
{
    public class ActiveOrdersHandler : IQueryHandler<ActiveOrdersRq, ActiveOrdersRs>
    {
        private readonly ApplicationDbContext context;
        private readonly ICustomerService customerService;
        private readonly ISessionHelper sessionHelper;

        public ActiveOrdersHandler(ApplicationDbContext context, ICustomerService customerService, ISessionHelper sessionHelper)
        {
            this.context = context;
            this.customerService = customerService;
            this.sessionHelper = sessionHelper;
        }

        public async ValueTask<ActiveOrdersRs> HandleAsync(ActiveOrdersRq query, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNullOrEmpty(query.Username);
            ValidationParameter.FailIfNullOrEmpty(query.SessionId.ToString());

            var orderDetailDtoList = new List<OrderDetailDto>();

            //Get session by id
            var session = await sessionHelper.GetSessionByIdAsync(context, query.SessionId);

            var isExpired = sessionHelper.SessionExpired(session);

            if (isExpired)
                throw new CoreException(ErrorCode.SessionExpired, $"Given session {session.SessionId} expired.");

            //Check session status, we can get order only when session is active
            sessionHelper.ValidateSession(session);

            var customer = await customerService.GetCustomerAsync(query.Username);

            var orderActiveList = from order in context.Orders
                     join product in context.Products
                     on order.ProductId equals product.ProductId
                     select new 
                     {
                         OrderId = order.OrderId,
                         OrderStatus = order.OrderStatus,
                         ValidTo = order.ValidTo,
                         Name = product.Name,
                         Owner = product.Customer.FirstName
                     };

            foreach (var item in orderActiveList)
            {
                orderDetailDtoList.Add(new OrderDetailDto
                {
                    OrderId = item.OrderId,
                    OrderStatus = item.OrderStatus.ToString(),
                    ValidTo = item.ValidTo,
                    OrderProduct = new OrderProduct
                    {
                        Name = item.Name,
                        Owner = item.Owner
                    }
                });
            }

            return new ActiveOrdersRs
            {
                OrderDetailDtoList = orderDetailDtoList
            };
        }
    }
}
