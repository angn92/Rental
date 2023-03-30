using Microsoft.Extensions.Logging;
using Rental.Core.Validation;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Handlers.Orders.Query.ActiveOrders;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Wrapper;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Orderss.Query.ActiveOrders
{
    public class ActiveOrdersHandler : IQueryHandler<ActiveOrdersRequest, ActiveOrdersResponse>
    {
        private readonly ILogger<ActiveOrdersHandler> _logger;
        private readonly IHttpContextWrapper _httpContextWrapper;
        private readonly ISessionHelper _sessionHelper;
        private readonly IOrderHelper _orderHelper;

        public ActiveOrdersHandler(ILogger<ActiveOrdersHandler> logger, IHttpContextWrapper httpContextWrapper, ISessionHelper sessionHelper, 
            ICustomerHelper customerHelper, IOrderHelper orderHelper)
        {
            _logger = logger;
            _httpContextWrapper = httpContextWrapper;
            _sessionHelper = sessionHelper;
            _orderHelper = orderHelper;
        }

        public async ValueTask<ActiveOrdersResponse> HandleAsync(ActiveOrdersRequest query, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNullOrEmpty(query.Username);

            var sessionId = _httpContextWrapper.GetValueFromRequestHeader("SessionId"); 
            var session = await _sessionHelper.GetSessionByIdAsync(sessionId);

            _sessionHelper.ValidateSessionStatus(session);

            var orders = _orderHelper.GetActiveOrders(query.Username);

            if (orders.Count == 0)
                throw new CoreException(ErrorCode.ClientHasNoActiveOrders, "No active orders");

            return new ActiveOrdersResponse
            {
                OrderDetailDtoList = orders
            };
        }
    }
}
