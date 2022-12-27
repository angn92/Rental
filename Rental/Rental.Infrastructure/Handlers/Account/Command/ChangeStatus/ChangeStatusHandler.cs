using Microsoft.Extensions.Logging;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Handlers.Account.Commmand.ChangeStatus;
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

        public ChangeStatusHandler(ILogger<ChangeStatusHandler> logger, ApplicationDbContext context, ICustomerService customerService)
        {
            _logger = logger;
            _context = context;
            _customerService = customerService;
        }

        public async ValueTask HandleAsync(ChangeStatusCommand command, CancellationToken cancellationToken = default)
        {
            var request = command.ChangeStatusRequest;

            ValidationParameter.FailIfNullOrEmpty(request.Username);
            ValidationParameter.FailIfNullOrEmpty(request.Status.ToString());

            try
            {
                var customer = await _customerService.GetCustomerAsync(request.Username);

                if (customer.Status.Equals(request.Status))
                    return;

                customer.Status = request.Status;

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
