using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Rental.Core.Domain;
using Rental.Core.Validation;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Services.CustomerService;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Query.AccountDetails
{
    public class GetCustomerDetailsHandler : IQueryHandler<GetCustomerDetailsRq, GetCustomerDetailsRs>
    {
        private readonly ILogger<GetCustomerDetailsHandler> logger;
        private readonly ICustomerService _customerService;

        public GetCustomerDetailsHandler(ILogger<GetCustomerDetailsHandler> logger, ICustomerService customerService)
        {
            this.logger = logger;
            _customerService = customerService;
        }

        public async ValueTask<GetCustomerDetailsRs> HandleAsync([NotNull] GetCustomerDetailsRq command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            Customer customerAccount;
            try
            {
                customerAccount = await _customerService.GetCustomerAsync(command.Username);
            }
            catch (Exception ex)
            {
                logger.LogError($"Customer with name {command.Username} does not exist." + $" {ex.Message}");
                throw;
            }

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
