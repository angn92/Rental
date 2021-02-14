using Rental.Infrastructure.Commands;
using Rental.Infrastructure.Commands.Users;
using Rental.Infrastructure.Services.UserService;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Users
{
    public class CreateUserHandler : ICommandHandler<RegisterUser>
    {
        private readonly IUserService _userService;

        public CreateUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task HandleAsync(RegisterUser command)
        {
            await _userService.RegisterAsync(command.FirstName, command.LastName, command.Username, command.Email, command.PhoneNumber);
        }
    }
}
