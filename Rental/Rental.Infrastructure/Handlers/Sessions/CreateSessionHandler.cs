using Rental.Infrastructure.Command;
using Rental.Infrastructure.Services.SessionService;
using Rental.Infrastructure.Services.CustomerService;
using System.Threading.Tasks;
using System.Threading;
using Rental.Core.Validation;
using Rental.Infrastructure.EF;
using System.Security.Cryptography;
using Rental.Core.Domain;
using Rental.Core.Enum;

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

        public async ValueTask<CreateSessionResponse> HandleAsync(CreateSessionCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            var customer = await _customerService.GetCustomerAsync(command.Username);

            // Do not allow create session if customer has blocked or not active account 
            _customerService.ValidateCustomerAccountAsync(customer);

            // If customer has assigned other session remove them all
            //await _sessionService.RemoveAllSession(command.Username);

            var sessionId = GenerateNewSession();

            var session = new Session(sessionId, customer, SessionState.NotAuthorized);

            await _context.AddAsync(session);
            await _context.SaveChangesAsync();

            var expirationDate = session.GenerationDate;

            return new CreateSessionResponse
            {
                IdSession = session.SessionId,
                Status = session.State,
                ExpirationTime = expirationDate
            };
        }

        private static int GenerateNewSession()
        {
            return RandomNumberGenerator.GetInt32(100000000, 999999999);
        }
    }
}
