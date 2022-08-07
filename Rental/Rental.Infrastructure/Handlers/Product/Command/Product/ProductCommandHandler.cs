using Microsoft.EntityFrameworkCore;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Product.Command.NewProduct
{
    public class ProductCommandHandler : ICommandHandler<ProductCommand>
    {
        private readonly IProductHelper productHelper;
        private readonly ICustomerService customerService;
        private readonly ISessionHelper sessionHelper;
        private readonly ApplicationDbContext context;

        public ProductCommandHandler([NotNull] ApplicationDbContext context, IProductHelper productHelper, ICustomerService customerService,
                    ISessionHelper sessionHelper)
        {
            this.context = context;
            this.productHelper = productHelper;
            this.customerService = customerService;
            this.sessionHelper = sessionHelper;
        }

        public async ValueTask HandleAsync(ProductCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            if (string.IsNullOrWhiteSpace(command.Request.Name))
                throw new CoreException(ErrorCode.IncorrectArgument, $"Value of {nameof(command.Request.Name)} in invalid.");

            var customer = await customerService.GetCustomerAsync(command.Request.Username);

            var session = await sessionHelper.GetSessionAsync(context, customer);

            sessionHelper.ValidateSession(session);

            await productHelper.CheckIfGivenProductExistAsync(context, command.Request.Name, customer);

            var category = await context.Categories.SingleOrDefaultAsync(x => x.Name == command.Request.CategoryName, cancellationToken);

            await productHelper.AddProductAsync(context, command.Request.Name, command.Request.Amount, customer, category, command.Request.Description);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
