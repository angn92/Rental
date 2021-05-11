using Rental.Core.Enum;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.UserService;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Sessions
{
    public class CreateSessionHandler : ICommandHandler<CreateSessionCommand, CreateSessionResponse>
    {
        private readonly IUserService _userService;
        private readonly ISessionHelper _sessionHelper;

        public CreateSessionHandler(IUserService userService, ISessionHelper sessionHelper)
        {
            _userService = userService;
            _sessionHelper = sessionHelper;
        }

        public async Task<CreateSessionResponse> HandleAsync(CreateSessionCommand command)
        {
            var user = await _userService.GetUserAsync(command.Username);

            if (user.Status != AccountStatus.Active)
            {
                throw new CoreException(ErrorCode.AccountNotActive, "User is not active");
            }

            var session = await _sessionHelper.CreateSession(user);

            return new CreateSessionResponse
            {
                IdSession = session.SessionId
            };
        }
    }
}
