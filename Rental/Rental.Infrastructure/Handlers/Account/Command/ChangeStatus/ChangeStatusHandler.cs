using Microsoft.Extensions.Logging;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Handlers.Account.Commmand.ChangeStatus;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Command.ChangeStatus
{
    public class ChangeStatusHandler : ICommandHandler<ChangeStatusCommand>
    {
        private readonly ILogger<ChangeStatusHandler> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ICustomerService _customerService;
        private readonly ICustomerHelper _customerHelper;

        public ChangeStatusHandler(ILogger<ChangeStatusHandler> logger, ApplicationDbContext context, ICustomerService customerService,
            ICustomerHelper customerHelper)
        {
            _logger = logger;
            _context = context;
            _customerService = customerService;
            _customerHelper = customerHelper;
        }

        public async ValueTask HandleAsync(ChangeStatusCommand command, CancellationToken cancellationToken = default)
        {
            var request = command.ChangeStatusRequest;

            ValidationParameter.FailIfNullOrEmpty(request.Username);
            ValidationParameter.FailIfNullOrEmpty(request.Status.ToString());

            try
            {
                var customer = await _customerHelper.GetCustomerAsync(_context, request.Username);

                if (customer.Status.Equals(request.Status))
                    return;

                _customerService.ChangeAccountStatus(customer, request.Status);

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
