using Rental.Core.Enum;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Services.CustomerService;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Command
{
    public class ChangeStatusHandler : ICommandHandler<ChangeStatusCommand>
    {
        private readonly ApplicationDbContext _rentalContext;
        private readonly ICustomerService _customerService;

        public ChangeStatusHandler(ApplicationDbContext rentalContext, ICustomerService customerService)
        {
            _rentalContext = rentalContext;
            _customerService = customerService;
        }

        public async ValueTask HandleAsync(ChangeStatusCommand command, CancellationToken cancellationToken = default)
        {
            var request = command.ChangeStatusRequest;

            ValidationParameter.FailIfNullOrEmpty(request.Username);
            ValidationParameter.FailIfNullOrEmpty(request.Status);

            var customer = await _customerService.GetCustomerAsync(request.Username);

            var mapEnum = Enum.TryParse<AccountStatus>(request.Status, out AccountStatus accountStatus);

            if (!mapEnum)
                throw new CoreException(ErrorCode.EnumMapError, $"Wrong mapped");

            if (customer.Status.Equals(accountStatus))
                return;

            customer.Status = accountStatus;
           
            await _rentalContext.SaveChangesAsync(cancellationToken);
        }
    }
}
