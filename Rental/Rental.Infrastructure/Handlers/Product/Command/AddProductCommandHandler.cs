using Microsoft.EntityFrameworkCore;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Product.Command
{
    public class AddProductCommandHandler : ICommandHandler<AddProductCommand>
    {
        private readonly IProductHelper _productHelper;

        public ApplicationDbContext _context { get; }

        public AddProductCommandHandler([NotNull] ApplicationDbContext context, IProductHelper productHelper)
        {
            _context = context;
            _productHelper = productHelper;
        }

        public async Task HandleAsync(AddProductCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            
            if (string.IsNullOrWhiteSpace(command.Request.Name))
                throw new CoreException(ErrorCode.IncorrectArgument, $"Value of {nameof(command.Request.Name)} in invalid.");

            await CheckIfExistAsync(_context, command.Request.Name);

            var category = await _context.Categories.SingleOrDefaultAsync(x => x.Name == command.Request.CategoryName);

            await _productHelper.AddProductAsync(_context, command.Request.Name, command.Request.Amount, command.Request.Description, category);

        }

        private async Task CheckIfExistAsync(ApplicationDbContext context, string name)
        {
            var productName = await context.Products.SingleOrDefaultAsync(x => x.Name == name);

            if (productName is not null)
                throw new CoreException(ErrorCode.ProductExist, $"Product {productName.Name} exist.");
        }
    }
}
