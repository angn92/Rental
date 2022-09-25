using Microsoft.EntityFrameworkCore;
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
        private readonly ICustomerService customerService;
        private readonly IOptions<ConfigurationOptions> options;
        private readonly IEmailHelper emailHelper;
        private readonly ISessionService sessionService;
        private readonly ApplicationDbContext context;
        private readonly IPasswordHelper passwordHelper;

        public RegisterCustomerHandler(ICustomerService customerService, IOptions<ConfigurationOptions> options, IEmailHelper emailHelper,
                                    ISessionService sessionService, ApplicationDbContext context, IPasswordHelper passwordHelper)
        {
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

                await passwordHelper.SetPassword(command.Password, customer, code);

                var message = await context.Dictionaries.FirstOrDefaultAsync(x => x.Name == "RegisterEmail", cancellationToken);

                if (message == null)
                    throw new CoreException(ErrorCode.IncorrectArgument, $"Given parameter does not exist.");

                
                var prepareEmail = new EmailConfiguration
                {
                    From = "test@email.com",
                    To = command.Email,
                    Subject = message.Name,
                    Message = message.Value,
                };

                emailHelper.SendEmail(prepareEmail);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
