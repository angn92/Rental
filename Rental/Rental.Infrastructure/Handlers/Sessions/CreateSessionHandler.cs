using Rental.Core.Enum;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Services.SessionService;
using Rental.Infrastructure.Services.CustomerService;
using System.Threading.Tasks;
using System.Threading;
using Rental.Core.Validation;
using Rental.Infrastructure.EF;

namespace Rental.Infrastructure.Handlers.Sessions
{
    public class CreateSessionHandler : ICommandHandler<CreateSessionCommand, CreateSessionResponse>
    {
        private readonly ICustomerService _customerService;
        private readonly ISessionService _sessionService;
        private readonly ApplicationDbContext _context;

        public CreateSessionHandler(ApplicationDbContext context, ICustomerService userService, ISessionService sessionService)
        {
            _context = context;
            _customerService = userService;
            _sessionService = sessionService;
        }

        public async Task<CreateSessionResponse> HandleAsync(CreateSessionCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            var user = await _customerService.GetCustomerAsync(command.Username);

            if(user is null)
            {
                throw new CoreException(ErrorCode.UserNotExist, $"User {command.Username} does not exist.");
            }

            if (user.Status != AccountStatus.Active)
            {
                throw new CoreException(ErrorCode.AccountNotActive, $"User {command.Username} is not active");
            }

            await _sessionService.RemoveAllSession(_context, command.Username);

            var session = await _sessionService.CreateSessionAsync(user);

            return new CreateSessionResponse
            {
                IdSession = session.SessionId
            };
        }
    }
}
