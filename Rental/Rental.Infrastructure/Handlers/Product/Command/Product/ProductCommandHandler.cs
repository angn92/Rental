using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Wrapper;
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
        private readonly IHttpContextWrapper _httpContextWrapper;
        private readonly ICategoryHelper _categoryHelper;
        private readonly ApplicationDbContext _context;

        public ProductCommandHandler([NotNull] ApplicationDbContext context, IProductHelper productHelper, ICustomerHelper customerService,
                    ISessionHelper sessionHelper, IHttpContextWrapper httpContextWrapper, ICategoryHelper categoryHelper)
        {
            _context = context;
            _productHelper = productHelper;
            _customerHelper = customerService;
            _sessionHelper = sessionHelper;
            _httpContextWrapper = httpContextWrapper;
            _categoryHelper = categoryHelper;
        }

        public async ValueTask HandleAsync(ProductCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            var requestPart = command.Request;

            if (string.IsNullOrWhiteSpace(command.Request.Name))
                throw new CoreException(ErrorCode.IncorrectArgument, $"Value of {nameof(requestPart.Name)} is invalid.");

            var sessionId = _httpContextWrapper.GetValueFromRequestHeader("SessionId");
            var session = await _sessionHelper.GetSessionByIdAsync(sessionId);
            _sessionHelper.ValidateSessionStatus(session);

            var customer = await _customerHelper.GetCustomerAsync(requestPart.Username);
            _customerHelper.ValidateCustomerAccount(customer);

            await _productHelper.CheckIfGivenProductExistAsync(requestPart.Name, customer);

            var category = await _categoryHelper.GetCategory(requestPart.CategoryName);

            await _productHelper.AddProductAsync(requestPart.Name, requestPart.Amount, customer, category, requestPart.Description);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
