using Rental.Core.Enum;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Services.SessionService;
using Rental.Infrastructure.Services.CustomerService;
using System.Threading.Tasks;
using System.Threading;

namespace Rental.Infrastructure.Handlers.Sessions
{
    public class CreateSessionHandler : ICommandHandler<CreateSessionCommand, CreateSessionResponse>
    {
        private readonly ICustomerService _customerService;
        private readonly ISessionService _sessionService;

        public CreateSessionHandler(ICustomerService userService, ISessionService sessionService)
        {
            _customerService = userService;
            _sessionService = sessionService;
        }

        public async Task<CreateSessionResponse> HandleAsync(CreateSessionCommand command, CancellationToken cancellationToken = default)
        {
            var user = await _customerService.GetCustomerAsync(command.Username);

            if(user is null)
            {
                throw new CoreException(ErrorCode.UserNotExist, $"User {command.Username} does not exist.");
            }

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
