using Rental.Infrastructure.Command;
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
        private readonly ICustomerService _customerService;
        private readonly ISessionService _sessionService;
        private readonly ISessionHelper _sessionHelper;
        private readonly IPasswordHelper _passwordHelper;

        public ChangePasswordHandler(ICustomerService customerService, ISessionService sessionService,
                                     ISessionHelper sessionHelper, IPasswordHelper passwordHelper)
        {
            _customerService = customerService;
            _sessionService = sessionService;
            _sessionHelper = sessionHelper;
            _passwordHelper = passwordHelper;
        }

        public async ValueTask HandleAsync(ChangePasswordCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            // to do get session which will be get from headers, implementation after add headers

            var customer = await _customerService.GetCustomerAsync(command.Username);

            _customerService.ValidateCustomerAccountAsync(customer);
            
            var activeUserPassword = await _passwordHelper.GetActivePassword(customer);

            //compare old password and new password. New password can not be exactly same like old password
            _passwordHelper.ComaprePasswords(activeUserPassword, command.NewPassword);

            await _passwordHelper.RemoveOldPassword(command.Username);

            await _passwordHelper.SetPassword(command.NewPassword, customer, "123");
        }
    }
}
