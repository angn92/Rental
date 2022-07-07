using Microsoft.Extensions.Options;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Configuration;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using Rental.Infrastructure.Services.SessionService;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Command.CreateAccount
{
    public class RegisterUserHandler : ICommandHandler<RegisterCustomer>
    {
        private readonly ICustomerService _customerService;
        private readonly IOptions<ConfigurationOptions> _options;
        private readonly IEmailHelper _emailHelper;
        private readonly ISessionService _sessionService;

        public RegisterUserHandler(ICustomerService customerService, IOptions<ConfigurationOptions> options, IEmailHelper emailHelper,
                                    ISessionService sessionService)
        {
            _customerService = customerService;
            _options = options;
            _emailHelper = emailHelper;
            _sessionService = sessionService;
        }

        public async ValueTask HandleAsync(RegisterCustomer command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            var customerExist = await _customerService.CheckIfExist(command.Username);

            if (customerExist)
                throw new CoreException(ErrorCode.UsernameExist, $"This name {command.Username} is use.");

            try
            {
                _emailHelper.ValidateEmail(command.Email);

                var customer = await _customerService.RegisterAsync(command.FirstName, command.LastName, command.Username, command.Email, command.PhoneNumber);

                var customerSession = await _sessionService.CreateNotAuthorizeSession(customer);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
