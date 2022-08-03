using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Product.Command
{
    public class AddProductCommandHandler : ICommandHandler<AddProductCommand>
    {
        private readonly IProductHelper productHelper;
        private readonly ICustomerService customerService;

        public ApplicationDbContext context { get; }

        public AddProductCommandHandler([NotNull] ApplicationDbContext context, IProductHelper productHelper, ICustomerService customerService)
        {
            this.context = context;
            this.productHelper = productHelper;
            this.customerService = customerService;
        }

        public async ValueTask HandleAsync(AddProductCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            if (string.IsNullOrWhiteSpace(command.Request.Name))
                throw new CoreException(ErrorCode.IncorrectArgument, $"Value of {nameof(command.Request.Name)} in invalid.");

            var customer = await customerService.GetCustomerAsync(command.Request.Username);

            if (customer == null)
                throw new CoreException(ErrorCode.UserNotExist, $"Wrong customer");
            
            CheckIfGivenProductExistAsync(context, command.Request.Name, customer);

            var category = await context.Categories.SingleOrDefaultAsync(x => x.Name == command.Request.CategoryName, cancellationToken);

            await productHelper.AddProductAsync(context, command.Request.Name, command.Request.Amount, command.Request.Description, category, customer);

            await context.SaveChangesAsync(cancellationToken);
        }

        private static void CheckIfGivenProductExistAsync([NotNull] ApplicationDbContext context, [NotNull] string name, [NotNull] Customer customer)
        {
            var productName = context.Products.Where(x => x.Customer.Username == customer.Username).ToList();

            if (productName.Any(x => x.Name == name))
                throw new CoreException(ErrorCode.ProductExist, $"Product {name} exist.");
        }
    }
}
