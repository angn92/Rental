using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Configuration;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using Rental.Infrastructure.Services.SessionService;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Command.CreateAccount
{
    public class RegisterCustomerHandler : ICommandHandler<RegisterCustomer>
    {
        private readonly ILogger logger;
        private readonly ICustomerService customerService;
        private readonly IOptions<ConfigurationOptions> options;
        private readonly IEmailHelper emailHelper;
        private readonly ISessionService sessionService;
        private readonly ApplicationDbContext context;
        private readonly IPasswordHelper passwordHelper;

        public RegisterCustomerHandler(ILogger<RegisterCustomerHandler> logger, ICustomerService customerService, IOptions<ConfigurationOptions> options, 
            IEmailHelper emailHelper, ISessionService sessionService, ApplicationDbContext context, IPasswordHelper passwordHelper)
        {
            this.logger = logger;
            this.customerService = customerService;
            this.options = options;
            this.emailHelper = emailHelper;
            this.sessionService = sessionService;
            this.context = context;
            this.passwordHelper = passwordHelper;
        }

        public async ValueTask HandleAsync(RegisterCustomer command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            var customerExist = await customerService.CheckIfExist(command.Username);

            if (customerExist)
                throw new CoreException(ErrorCode.UsernameExist, $"This name {command.Username} is use.");

            try
            {
                var customer = await customerService.RegisterAsync(command.FirstName, command.LastName, command.Username, command.Email, command.PhoneNumber);

                var customerSession = await sessionService.CreateNotAuthorizedSession(customer);

                var code = passwordHelper.GenerateActivationCode();

                logger.LogInformation($"Activation code {code}");

                await passwordHelper.SetPassword(command.Password, customer, code.ToString());

                var prepareEmail = new EmailConfiguration
                {
                    From = "test@email.com",
                    To = command.Email,
                    Subject = "Activate password",
                    Message = $"You created new account. This is your activation code {code}",
                };

                emailHelper.SendEmail(prepareEmail);

                logger.LogInformation("Registration process was successful. Activation code has been sent on given address email.");
            }
            catch (Exception ex)
            {
                logger.LogError("Process registration new customer is failed.");
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
