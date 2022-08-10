using Rental.Infrastructure.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Product.Command.Cancel
{
    public class CancelReservationHandler : ICommandHandler<CancelReservationCommand>
    {
        public ValueTask HandleAsync(CancelReservationCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
