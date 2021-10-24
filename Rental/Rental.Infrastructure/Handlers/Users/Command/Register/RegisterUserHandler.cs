using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Command.Users.Command.Register;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Services.UserService;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Users.Command.Register
{
    public class RegisterUserHandler : ICommandHandler<RegisterUser>
    {
        private readonly IUserService _userService;

        public RegisterUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task HandleAsync(RegisterUser command)
        {
            ValidationParameter.FailIfNull(command);

            var customerExist = await _userService.CheckIfExist(command.Username);

            if (customerExist)
                throw new CoreException(ErrorCode.UsernameExist, $"Name of {command.Username} is in use.");

            await _userService.RegisterAsync(command.FirstName, command.LastName, command.Username, 
                                            command.Email, command.PhoneNumber, command.Password);
        }
    }
}
