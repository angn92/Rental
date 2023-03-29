using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Rental.Core.Enum;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Wrapper;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Product.Command.Cancel
{
    public class CancelReservationHandler : ICommandHandler<CancelReservationCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductHelper _productHelper;
        private readonly IOrderHelper _orderHelper;
        private readonly ILogger<CancelReservationHandler> _logger;
        private readonly ICustomerHelper _customerHelper;
        private readonly IHttpContextWrapper _httpContextWrapper;
        private readonly ISessionHelper _sessionHelper;

        public CancelReservationHandler([NotNull] ApplicationDbContext context, IProductHelper productHelper, IOrderHelper orderHelper,
            ILogger<CancelReservationHandler> logger, ICustomerHelper customerHelper, IHttpContextWrapper httpContextWrapper, ISessionHelper sessionHelper)
        {
            _context = context;
            _productHelper = productHelper;
            _orderHelper = orderHelper;
            _logger = logger;
            _customerHelper = customerHelper;
            _httpContextWrapper = httpContextWrapper;
            _sessionHelper = sessionHelper;
        }

        public async ValueTask HandleAsync(CancelReservationCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNullOrEmpty(command.Username);

            try 
            {
                var sessionId = _httpContextWrapper.GetValueFromRequestHeader("SessionId");
                var session = _sessionHelper.GetSessionByIdAsync(sessionId);

                var product = await _productHelper.GetProductAsync(command.ProductId);

                if (new[] { ProductStatus.Available, ProductStatus.Inaccessible }.Contains(product.Status))
                    throw new CoreException(ErrorCode.ProductIsNotReserved, $"You can not canceling reservation for {product.Name} because this item is not reserved.");

                //Client who's made reservation
                var customer = await _customerHelper.GetCustomerAsync(command.Username);

                var order = await _orderHelper.GetAcceptedOrderForGivenProduct(product.ProductId, customer.CustomerId.ToString());

                order.ChangeOrderStatus(OrderStatus.Cancelled);

                _logger.LogInformation($"Order {order.OrderId} was cancelled.");

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch(Exception ex)
            {
                _logger.LogError(String.Format($"Canceling this reservation did not successful. {ex.Message}"));
            }
        }
    }
}
