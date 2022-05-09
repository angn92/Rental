using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Services.CustomerService;
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

            var customer = await _customerService.GetCustomerAsync(request.Username);

            if (customer.Status == request.Status)
                return;

            customer.Status = request.Status;
           
            await _rentalContext.SaveChangesAsync();
        }
    }
}
