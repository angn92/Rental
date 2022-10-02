using Rental.Core.Validation;
using Rental.Infrastructure.DTO;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Services.CustomerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Orders.Query.ActiveOrders
{
    public class ActiveOrdersHandler : IQueryHandler<ActiveOrdersRq, ActiveOrdersRs>
    {
        private readonly ApplicationDbContext context;
        private readonly IOrderHelper orderHelper;
        private readonly ICustomerService customerService;
        private readonly ISessionHelper sessionHelper;

        public ActiveOrdersHandler(ApplicationDbContext context, IOrderHelper orderHelper, ICustomerService customerService, ISessionHelper sessionHelper)
        {
            this.context = context;
            this.orderHelper = orderHelper;
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

            var orderActiveList = orderHelper.GetActiveOrders(context, customer.CustomerId.ToString());

            foreach (var item in orderActiveList)
            {
                orderDetailDtoList.Add(new OrderDetailDto
                {
                    OrderId = item.OrderId,
                    OrderStatus = item.OrderStatus.ToString(),
                    ValidTo = item.ValidTo,
                    OrderProduct = new OrderProduct
                    {
                        Name = item.ProductName,
                        Owner = item.CustomerId //TODO: to change way return information about order and product add fatch<product> in linq
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
