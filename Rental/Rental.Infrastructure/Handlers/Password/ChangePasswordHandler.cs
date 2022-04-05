using Rental.Core.Enum;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.SessionService;
using Rental.Infrastructure.Services.CustomerService;
using System.Threading.Tasks;
using System.Threading;
using Rental.Core.Validation;

namespace Rental.Infrastructure.Handlers.Password
{
    public class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand>
    {
        private readonly ApplicationDbContext Context;
        private readonly ICustomerService _customerService;
        private readonly ISessionService _sessionService;
        private readonly ISessionHelper _sessionHelper;
        private readonly IPasswordHelper _passwordHelper;

        public ChangePasswordHandler(ApplicationDbContext context, ICustomerService customerService, ISessionService sessionService,
                                     ISessionHelper sessionHelper, IPasswordHelper passwordHelper)
        {
            Context = context;
            _customerService = customerService;
            _sessionService = sessionService;
            _sessionHelper = sessionHelper;
            _passwordHelper = passwordHelper;
        }

        public async Task HandleAsync(ChangePasswordCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            // to do get session which will be get from headers, implementation after add headers

            var customer = await _customerService.GetCustomerAsync(command.Username);

            await _customerService.ValidateCustomerAccountAsync(customer);
            
            //Get active password


            //_rentalContext.Remove(oldPassword);
            await _rentalContext.SaveChangesAsync();

            //Set new password and save to DB

            //await _passwordHelper.SetPassword(command.NewPassword, customer);
        }
    }
}
