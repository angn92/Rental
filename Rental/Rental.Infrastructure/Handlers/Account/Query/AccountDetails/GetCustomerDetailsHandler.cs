using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Rental.Core.Domain;
using Rental.Core.Validation;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Query.AccountDetails
{
    public class GetCustomerDetailsHandler : IQueryHandler<GetCustomerDetailsRq, GetCustomerDetailsRs>
    {
        private readonly ILogger<GetCustomerDetailsHandler> logger;
        private readonly ApplicationDbContext _context;
        private readonly ICustomerHelper _customerHelper;

        public GetCustomerDetailsHandler(ILogger<GetCustomerDetailsHandler> logger, ApplicationDbContext context, ICustomerHelper customerHelper)
        {
            this.logger = logger;
            _context = context;
            _customerHelper = customerHelper;
        }

        public async ValueTask<GetCustomerDetailsRs> HandleAsync([NotNull] GetCustomerDetailsRq command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            Customer customerAccount;
            try
            {
                customerAccount = await _customerHelper.GetCustomerAsync(command.Username);
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
