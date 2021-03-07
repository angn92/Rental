using Rental.Infrastructure.Command;
using Rental.Infrastructure.Command.Users.Command;
using Rental.Infrastructure.Services.UserService;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Users.Command
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
            await _userService.RegisterAsync(command.FirstName, command.LastName, command.Username, command.Email, command.PhoneNumber, command.Password);
        }
    }
}
