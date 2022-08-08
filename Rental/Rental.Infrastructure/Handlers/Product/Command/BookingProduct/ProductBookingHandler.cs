using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Product.Command.BookingProduct
{
    public class ProductBookingHandler : ICommandHandler<ProductBookingCommand, ProductBookingResponse>
    {
        private readonly ApplicationDbContext context;

        public ProductBookingHandler(ApplicationDbContext context)
        {
            this.context = context;
        }

        public ValueTask<ProductBookingResponse> HandleAsync(ProductBookingCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
