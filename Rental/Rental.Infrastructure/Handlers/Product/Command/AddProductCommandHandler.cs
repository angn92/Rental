using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Product.Command
{
    public class AddProductCommandHandler : ICommandHandler<AddProductCommand>
    {
        public AddProductCommandHandler()
        {

        }

        public async Task HandleAsync(AddProductCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);


        }
    }
}
