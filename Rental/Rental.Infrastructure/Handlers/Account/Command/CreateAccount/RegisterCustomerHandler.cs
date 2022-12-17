using Microsoft.Extensions.Logging;
using Rental.Core.Enum;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using Rental.Infrastructure.Services.SessionService;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Command.CreateAccount
{
    public class RegisterCustomerHandler : ICommandHandler<RegisterCustomer, RegisterCustomerResponse>
    {
        private readonly ILogger _logger;
        private readonly ICustomerService _customerService;
        private readonly IEmailHelper _emailHelper;
        private readonly ISessionService _sessionService;
        private readonly IPasswordHelper _passwordHelper;

        public RegisterCustomerHandler(ILogger<RegisterCustomerHandler> logger, ICustomerService customerService, IEmailHelper emailHelper, 
            ISessionService sessionService, IPasswordHelper passwordHelper)
        {
            _logger = logger;
            _customerService = customerService;
            _emailHelper = emailHelper;
            _sessionService = sessionService;
            _passwordHelper = passwordHelper;
        }

        public async ValueTask<RegisterCustomerResponse> HandleAsync(RegisterCustomer command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            var customerExist = await _customerService.CheckIfExist(command.Username);

            if (customerExist)
                throw new CoreException(ErrorCode.UsernameExist, $"This username {command.Username} is in use.");

            try
            {
                var customer = await _customerService.RegisterAsync(command.FirstName, command.LastName, command.Username, 
                    command.Email, command.PhoneNumber);

                var customerSession = await _sessionService.CreateSession(customer);

                var code = _passwordHelper.GenerateActivationCode();

                _logger.LogInformation($"Activation code {code}");

                await _passwordHelper.SetPassword(command.Password, customer, code.ToString());

                var message = _emailHelper.PrepareEmail(command.Email, SubjectMessage.RegistrationAccount);

                await _emailHelper.SendEmail(message);

                _logger.LogInformation("Registration process was successful. Activation code has been sent on given address email.");

                var response = new RegisterCustomerResponse
                {
                    SessionId = customerSession.SessionId.ToString()
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Process registration new customer is failed.");
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
