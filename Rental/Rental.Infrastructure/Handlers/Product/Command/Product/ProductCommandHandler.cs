using Microsoft.EntityFrameworkCore;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Product.Command.NewProduct
{
    public class ProductCommandHandler : ICommandHandler<ProductCommand>
    {
        private readonly IProductHelper _productHelper;
        private readonly ICustomerHelper _customerHelper;
        private readonly ISessionHelper _sessionHelper;
        private readonly ApplicationDbContext _context;

        public ProductCommandHandler([NotNull] ApplicationDbContext context, IProductHelper productHelper, ICustomerHelper customerService,
                    ISessionHelper sessionHelper)
        {
            _context = context;
            _productHelper = productHelper;
            _customerHelper = customerService;
            _sessionHelper = sessionHelper;
        }

        public async ValueTask HandleAsync(ProductCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            if (string.IsNullOrWhiteSpace(command.Request.Name))
                throw new CoreException(ErrorCode.IncorrectArgument, $"Value of {nameof(command.Request.Name)} is invalid.");

            var customer = await _customerHelper.GetCustomerAsync(_context, command.Request.Username);

            var session = await _sessionHelper.GetSessionAsync(_context, customer);

            _sessionHelper.ValidateSession(session);

            await _productHelper.CheckIfGivenProductExistAsync(_context, command.Request.Name, customer);

            var category = await _context.Categories.SingleOrDefaultAsync(x => x.Name == command.Request.CategoryName, cancellationToken);

            await _productHelper.AddProductAsync(_context, command.Request.Name, command.Request.Amount, customer, category, command.Request.Description);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
