using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Rental.Core.Enum;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
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

        public CancelReservationHandler([NotNull] ApplicationDbContext context, IProductHelper productHelper, IOrderHelper orderHelper,
            ILogger<CancelReservationHandler> logger, ICustomerHelper customerHelper)
        {
            _context = context;
            _productHelper = productHelper;
            _orderHelper = orderHelper;
            _logger = logger;
            _customerHelper = customerHelper;
        }

        public async ValueTask HandleAsync(CancelReservationCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNullOrEmpty(command.Username);

            try 
            {
                var product = await _productHelper.GetProductAsync(command.ProductId);

                if (new[] { ProductStatus.Available, ProductStatus.Inaccessible }.Contains(product.Status))
                    throw new CoreException(ErrorCode.ProductIsNotReserved, $"You can not canceling reservation for {product.Name} because this item is not reserved.");

                //Client who's made reservation
                var customer = await _customerHelper.GetCustomerAsync(command.Username);

                var order = await _orderHelper.GetAcceptedOrderForGivenProduct(_context, product.ProductId, customer.CustomerId.ToString());

                order.ChangeOrderStatus(OrderStatus.Cancelled);

                _logger.Log(LogLevel.Information, $"One order {order.OrderId} was cancelled");

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, String.Format($"Canceling this reservation did not successful. {ex.Message}"));
            }
        }
    }
}
