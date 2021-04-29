using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Services.UserService;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Sessions
{
    public class CreateSessionHandler : ICommandHandler<CreateSessionCommand, CreateSessionResponse>
    {
        private readonly IUserService _userService;

        public CreateSessionHandler(IUserService userService)
        {
            _userService = userService;
        }

        public Task<CreateSessionResponse> HandleAsync(CreateSessionCommand command)
        {
            var user = _userService.GetUserAsync(command.Username);
            
            if(user == null)
            {
                throw new CoreException(ErrorCode.UserNotExist, $"User {command.Username} does not exist.");
            }

            
        }
    }
}
