using Rental.Core.Enum;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Services.SessionService;
using Rental.Infrastructure.Services.UserService;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Sessions
{
    public class CreateSessionHandler : ICommandHandler<CreateSessionCommand, CreateSessionResponse>
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;

        public CreateSessionHandler(IUserService userService, ISessionService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;
        }

        public async Task<CreateSessionResponse> HandleAsync(CreateSessionCommand command)
        {
            var user = await _userService.GetUserAsync(command.Username);

            if (user.Status != AccountStatus.Active)
            {
                throw new CoreException(ErrorCode.AccountNotActive, "User is not active");
            }

            var session = await _sessionService.CreateSessionAsync(user);

            return new CreateSessionResponse
            {
                IdSession = session.SessionId
            };
        }
    }
}
