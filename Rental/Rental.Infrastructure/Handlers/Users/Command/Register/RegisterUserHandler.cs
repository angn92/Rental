using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Command.Users.Command.Register;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Services.CustomerService;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Users.Command.Register
{
    public class RegisterUserHandler : ICommandHandler<RegisterUser>
    {
        private readonly ICustomerService _customerService;

        public RegisterUserHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task HandleAsync(RegisterUser command)
        {
            ValidationParameter.FailIfNull(command);

            var customerExist = await _customerService.CheckIfExist(command.Username);

            if (customerExist)
                throw new CoreException(ErrorCode.UsernameExist, $"Name of {command.Username} is in use.");

            await _customerService.RegisterAsync(command.FirstName, command.LastName, command.Username, 
                                            command.Email, command.PhoneNumber, command.Password);
        }
    }
}
