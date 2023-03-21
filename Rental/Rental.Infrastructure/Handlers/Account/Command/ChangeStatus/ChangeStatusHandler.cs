using Microsoft.Extensions.Logging;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Handlers.Account.Commmand.ChangeStatus;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Wrapper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Command.ChangeStatus
{
    public class ChangeStatusHandler : ICommandHandler<ChangeStatusCommand>
    {
        private readonly ILogger<ChangeStatusHandler> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ICustomerHelper _customerHelper;
        private readonly IHttpContextWrapper _httpContextWrapper;
        private readonly ISessionHelper _sessionHelper;

        public ChangeStatusHandler(ILogger<ChangeStatusHandler> logger, ApplicationDbContext context, ICustomerHelper customerHelper,
            IHttpContextWrapper httpContextWrapper, ISessionHelper sessionHelper)
        {
            _logger = logger;
            _context = context;
            _customerHelper = customerHelper;
            _httpContextWrapper = httpContextWrapper;
            _sessionHelper = sessionHelper;
        }

        public async ValueTask HandleAsync(ChangeStatusCommand command, CancellationToken cancellationToken = default)
        {
            var request = command.ChangeStatusRequest;

            ValidationParameter.FailIfNullOrEmpty(request.Username);
            ValidationParameter.FailIfNullOrEmpty(request.Status.ToString());

            try
            {
                var sessionId = _httpContextWrapper.GetValueFromRequestHeader("SessionId");

                var customerSession = await _sessionHelper.GetSessionByIdAsync(sessionId);
                _sessionHelper.ValidateSessionStatus(customerSession);

                var customer = await _customerHelper.GetCustomerAsync(request.Username);
                _customerHelper.ValidateCustomerAccount(customer);

                if (customer.Status.Equals(request.Status))
                {
                    _logger.LogInformation($"Customer {request.Username} has set status {request.Status.ToString()}.");
                    return;
                }
                    
                _customerHelper.ChangeAccountStatus(customer, request.Status);

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Changing account status was failure.");
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
