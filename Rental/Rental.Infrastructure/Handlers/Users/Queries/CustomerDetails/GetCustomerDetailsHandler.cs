using JetBrains.Annotations;
using Rental.Core.Validation;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Services.CustomerService;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Users.Queries
{
    public class GetCustomerDetailsHandler : IQueryHandler<GetCustomerDetailsRq, GetCustomerDetailsRs>
    {
        private readonly ICustomerService _customerService;

        public GetCustomerDetailsHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<GetCustomerDetailsRs> HandleAsync([NotNull] GetCustomerDetailsRq command)
        {
            ValidationParameter.FailIfNull(command);

            var customerAccount = await _customerService.GetCustomerAsync(command.Nick);

            return new GetCustomerDetailsRs
            {
                Fullname = customerAccount.FirstName + " " + customerAccount.LastName,
                Username = customerAccount.Username,
                Email = customerAccount.Email,
                Status = customerAccount.Status
            };
        }
    }
}
