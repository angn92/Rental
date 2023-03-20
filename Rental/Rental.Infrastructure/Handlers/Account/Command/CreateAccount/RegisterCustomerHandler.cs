using Microsoft.Extensions.Logging;
using Rental.Core.Enum;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Command.CreateAccount
{
    public class RegisterCustomerHandler : ICommandHandler<RegisterCustomer, RegisterCustomerResponse>
    {
        private readonly ILogger _logger;
        private readonly IEmailHelper _emailHelper;
        private readonly IPasswordHelper _passwordHelper;
        private readonly ISessionHelper _sessionHelper;
        private readonly ICustomerHelper _customerHelper;

        public RegisterCustomerHandler(ILogger<RegisterCustomerHandler> logger, IEmailHelper emailHelper, IPasswordHelper passwordHelper, ISessionHelper sessionHelper, 
            ICustomerHelper customerHelper)
        {
            _logger = logger;
            _emailHelper = emailHelper;
            _passwordHelper = passwordHelper;
            _sessionHelper = sessionHelper;
            _customerHelper = customerHelper;
        }

        public async ValueTask<RegisterCustomerResponse> HandleAsync(RegisterCustomer command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            try
            {
                var customerExist = _customerHelper.CheckIfExist(command.Username);

                if (customerExist)
                    throw new CoreException(ErrorCode.UsernameExist, $"This username {command.Username} is in use.");

                var customer = await _customerHelper.RegisterAsync(command.FirstName, command.LastName, command.Username, command.Email);

                _sessionHelper.RemoveAllSession(customer.Username);

                var customerSession = await _sessionHelper.CreateSession(customer);

                var code = _passwordHelper.GenerateActivationCode();

                _logger.LogInformation($"Activation code {code}");

                await _passwordHelper.SetPassword(command.Password, customer, code.ToString());

                var message = _emailHelper.PrepareEmail(command.Email, SubjectMessage.RegistrationAccount);

                await _emailHelper.SendEmail(message);

                _logger.LogInformation("Registration process was successful. Activation code has been sent on given address email.");

                var response = new RegisterCustomerResponse
                {
                    SessionId = customerSession.SessionIdentifier.ToString()
                };

                return response;
            }
            catch (CoreException ex)
            {
                _logger.LogError("Process registration new customer is failed.");
                throw new CoreException(ex.Code, ex.Message);
            }
        }
    }
}
