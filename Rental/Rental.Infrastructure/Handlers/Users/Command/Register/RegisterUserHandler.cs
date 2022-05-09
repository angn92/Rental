using Microsoft.Extensions.Options;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Command.Users.Command.Register;
using Rental.Infrastructure.Configuration;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Users.Command.Register
{
    public class RegisterUserHandler : ICommandHandler<RegisterUser>
    {
        private readonly ICustomerService _customerService;
        private readonly IOptions<ConfigurationOptions> _options;
        private readonly IEmailHelper _emailHelper;

        public RegisterUserHandler(ICustomerService customerService, IOptions<ConfigurationOptions> options, IEmailHelper emailHelper)
        {
            _customerService = customerService;
            _options = options;
            _emailHelper = emailHelper;
        }

        public async ValueTask HandleAsync(RegisterUser command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            var customerExist = await _customerService.CheckIfExist(command.Username);

            if (customerExist)
                throw new CoreException(ErrorCode.UsernameExist, $"Name of {command.Username} is in use. Choose another one.");

            await _customerService.RegisterAsync(command.FirstName, command.LastName, command.Username, command.Email, command.PhoneNumber);

            //if (_options.Value.SendRealEmail)
            //    await _emailHelper.SendEmail(command.Email, "content todo");

        }
    }
}
