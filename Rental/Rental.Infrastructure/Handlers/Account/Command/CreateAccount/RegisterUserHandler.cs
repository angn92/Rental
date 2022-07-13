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
    public class RegisterUserHandler : ICommandHandler<RegisterCustomer>
    {
        private readonly ICustomerService _customerService;
        private readonly IOptions<ConfigurationOptions> _options;
        private readonly IEmailHelper _emailHelper;
        private readonly ISessionService _sessionService;
        private readonly ApplicationDbContext _context;

        public RegisterUserHandler(ICustomerService customerService, IOptions<ConfigurationOptions> options, IEmailHelper emailHelper,
                                    ISessionService sessionService, ApplicationDbContext context)
        {
            _customerService = customerService;
            _options = options;
            _emailHelper = emailHelper;
            _sessionService = sessionService;
            _context = context;
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

                var customerSession = await _sessionService.CreateNotAuthorizedSession(customer);

                var message = _context.Dictionaries.FirstOrDefaultAsync(x => x.Name == "RegisterEmail", cancellationToken);

                if (message == null)
                    throw new CoreException(ErrorCode.IncorrectArgument, $"Given parameter does not exist.");

                var prepareEmail = new EmailConfiguration
                {
                    From = "test@email.com",
                    To = "customer@email.com",
                    Subject = "Register new account",
                    Message = "Just now you created new account in our application. Thanks"
                };

                _emailHelper.SendEmail(prepareEmail);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
