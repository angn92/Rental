﻿using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Product.Command.BookingProduct
{
    public class ProductBookingHandler : ICommandHandler<ProductBookingCommand, ProductBookingResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICustomerHelper _customerHelper;
        private readonly IProductHelper _productHelper;

        public ProductBookingHandler(ApplicationDbContext context, ICustomerHelper customerHelper, IProductHelper productHelper)
        {
            _context = context;
            _customerHelper = customerHelper;
            _productHelper = productHelper;
        }

        public async ValueTask<ProductBookingResponse> HandleAsync(ProductBookingCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            var request = command.Request;

            var customer = await _customerHelper.GetCustomerAsync(command.Request.Username);

            var product = await _productHelper.GetProductAsync(request.ProductId);

            if (product.Status != ProductStatus.Available)
                throw new CoreException(ErrorCode.ProductNotAvailable, $"{product.Name} is not available now.");

            _productHelper.MakeReservationProduct(product);

            var order = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
                CustomerId = customer.CustomerId.ToString(),
                ProductId = product.ProductId,
                ProductName = product.Name,
                Amount = request.Amount,
                ValidFrom = request.From,
                ValidTo = request.To,
                OrderHash = Guid.NewGuid().ToString()
            };

            await _context.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new ProductBookingResponse
            {
                OrderId = order.OrderId,
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
