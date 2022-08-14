using JetBrains.Annotations;
using Rental.Core.Enum;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Product.Command.Cancel
{
    public class CancelReservationHandler : ICommandHandler<CancelReservationCommand>
    {
        private readonly ApplicationDbContext context;
        private readonly IProductHelper productHelper;

        public CancelReservationHandler([NotNull] ApplicationDbContext context, IProductHelper productHelper)
        {
            this.context = context;
            this.productHelper = productHelper;
        }

        public async ValueTask HandleAsync(CancelReservationCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNullOrEmpty(command.Username);

            //Find product in db
            var product = await productHelper.GetProductAsync(context, command.ProductId);

            if (product.Status == ProductStatus.Available)
                throw new CoreException(ErrorCode.ProductIsNotReserved, $"You can not canceling reservation for {product.Name} because this item is not reserved.");

            //Find Order for current product

        }
    }
}
