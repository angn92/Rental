using Rental.Infrastructure.Command;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.SessionService;
using Rental.Infrastructure.Services.CustomerService;
using System.Threading.Tasks;
using System.Threading;
using Rental.Core.Validation;
using Microsoft.Extensions.Logging;
using System;
using Rental.Infrastructure.EF;

namespace Rental.Infrastructure.Handlers.Password
{
    public class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand>
    {
        private readonly ILogger<ChangePasswordHandler> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ICustomerService _customerService;
        private readonly ISessionService _sessionService;
        private readonly ISessionHelper _sessionHelper;
        private readonly IPasswordHelper _passwordHelper;
        private readonly ICustomerHelper _customerHelper;

        public ChangePasswordHandler(ILogger<ChangePasswordHandler> logger, ApplicationDbContext context, ICustomerService customerService, 
            ISessionService sessionService, ISessionHelper sessionHelper, IPasswordHelper passwordHelper, ICustomerHelper customerHelper)
        {
            _logger = logger;
            _context = context;
            _customerService = customerService;
            _sessionService = sessionService;
            _sessionHelper = sessionHelper;
            _passwordHelper = passwordHelper;
            _customerHelper = customerHelper;
        }

        public async ValueTask HandleAsync(ChangePasswordCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            // to do get session which will be get from headers, implementation after add headers
            try
            {
                var customer = await _customerHelper.GetCustomerAsync(_context, command.Username);

                _customerHelper.ValidateCustomerAccount(customer);

                var activeUserPassword = await _passwordHelper.GetActivePassword(customer);

                //compare old password and new password. New password can not be exactly same like old password
                _passwordHelper.ComaprePasswords(activeUserPassword, command.NewPassword);

                await _passwordHelper.RemoveOldPassword(command.Username);

                var activationCode = _passwordHelper.GenerateActivationCode();

                _logger.LogInformation($"Preparing activation code. Your code is {activationCode}.");

                await _passwordHelper.SetPassword(command.NewPassword, customer, activationCode.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Action change password is failed. {ex.Message}.");
                throw;
            }
        }
    }
}
