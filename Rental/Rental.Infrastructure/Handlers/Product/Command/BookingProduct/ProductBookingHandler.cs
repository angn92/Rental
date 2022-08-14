using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Product.Command.BookingProduct
{
    public class ProductBookingHandler : ICommandHandler<ProductBookingCommand, ProductBookingResponse>
    {
        private readonly ApplicationDbContext context;
        private readonly ICustomerService customerService;
        private readonly IProductHelper productHelper;

        public ProductBookingHandler(ApplicationDbContext context, ICustomerService customerService, IProductHelper productHelper)
        {
            this.context = context;
            this.customerService = customerService;
            this.productHelper = productHelper;
        }

        public async ValueTask<ProductBookingResponse> HandleAsync(ProductBookingCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            var request = command.Request;

            var customer = await customerService.GetCustomerAsync(command.Request.Username);

            var product = await productHelper.GetProductAsync(context, request.ProductId);

            if (product.Status != ProductStatus.Available)
                throw new CoreException(ErrorCode.ProductNotAvailable, $"{product.Name} is not available now.");

            productHelper.MakeReservationProduct(product);

            var transaction = new Order
            {
                TransactionId = Guid.NewGuid().ToString(),
                CustomerId = customer.CustomerId.ToString(),
                ProductId = product.ProductId,
                ProductName = product.Name,
                Amount = request.Amount,
                From = request.From,
                To = request.To,
                TransactionHash = Guid.NewGuid().ToString()
            };

            await context.AddAsync(transaction, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            var response = new ProductBookingResponse
            {
                OrderId = transaction.TransactionId,
                OrderTime = DateTime.UtcNow,
                Amount = request.Amount,
                Owner = customer.FirstName,
                ProductName = product.Name,
                NumberDays = request.To - request.From
            };

            return response;
        }
    }
}
