using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Rental.Core.Domain;
using Rental.Core.Validation;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Wrapper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Query.AccountDetails
{
    public class GetCustomerDetailsHandler : IQueryHandler<GetCustomerDetailsRequest, GetCustomerDetailsResponse>
    {
        private readonly ILogger<GetCustomerDetailsHandler> logger;
        private readonly ApplicationDbContext _context;
        private readonly ICustomerHelper _customerHelper;
        private readonly IHttpContextWrapper _httpContextWrapper;

        public GetCustomerDetailsHandler(ILogger<GetCustomerDetailsHandler> logger, ApplicationDbContext context, ICustomerHelper customerHelper,
            IHttpContextWrapper httpContextWrapper)
        {
            this.logger = logger;
            _context = context;
            _customerHelper = customerHelper;
            _httpContextWrapper = httpContextWrapper;
        }

        public async ValueTask<GetCustomerDetailsResponse> HandleAsync([NotNull] GetCustomerDetailsRequest command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            Customer customerAccount;
            try
            {
                var sessionId = _httpContextWrapper.GetValueFromRequestHeader("SessionId");
                customerAccount = await _customerHelper.GetCustomerAsync(command.Username);
            }
            catch (Exception ex)
            {
                logger.LogError($"Customer with name {command.Username} does not exist." + $" {ex.Message}");
                throw;
            }

            return new GetCustomerDetailsResponse
            {
                Fullname = customerAccount.FirstName + " " + customerAccount.LastName,
                Username = customerAccount.Username,
                Email = customerAccount.Email,
                Status = customerAccount.Status
            };
        }
    }
}
