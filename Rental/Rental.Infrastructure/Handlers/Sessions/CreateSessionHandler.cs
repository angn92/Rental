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
using System;
using Microsoft.Extensions.Logging;

namespace Rental.Infrastructure.Handlers.Sessions
{
    public class CreateSessionHandler : ICommandHandler<CreateSessionCommand, CreateSessionResponse>
    {
        private readonly ICustomerService _customerService;
        private readonly ISessionService _sessionService;
        private readonly ILogger<CreateSessionHandler> logger;
        private readonly ApplicationDbContext _context;

        public CreateSessionHandler(ILogger<CreateSessionHandler> logger, ApplicationDbContext context, ICustomerService userService, 
            ISessionService sessionService)
        {
            this.logger = logger;
            _context = context;
            _customerService = userService;
            _sessionService = sessionService;
        }

        public async ValueTask<CreateSessionResponse> HandleAsync(CreateSessionCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            var customer = await _customerService.GetCustomerAsync(command.Username);

            try
            {
                _customerService.ValidateCustomerAccount(customer);
            }
            catch(Exception ex)
            {
                logger.LogInformation($"To create session is required active customer account. {ex.Message}.");
                throw;
            }
            
            _sessionService.RemoveAllSession(command.Username);

            var sessionId = GenerateNewSession();

            var session = new Session(sessionId, customer, SessionState.NotAuthorized);

            await _context.AddAsync(session, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var expirationDate = session.GenerationDate;

            logger.LogInformation($"New session {sessionId} was has beed created.");

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
