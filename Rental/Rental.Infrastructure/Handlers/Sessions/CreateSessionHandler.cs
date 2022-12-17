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
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using System.Linq;

namespace Rental.Infrastructure.Handlers.Sessions
{
    public class CreateSessionHandler : ICommandHandler<CreateSessionCommand, CreateSessionResponse>
    {
        private readonly ICustomerService _customerService;
        private readonly ISessionService _sessionService;
        private readonly ISessionHelper _sessionHelper;
        private readonly ILogger<CreateSessionHandler> _logger;
        private readonly ApplicationDbContext _context;

        public CreateSessionHandler(ILogger<CreateSessionHandler> logger, ApplicationDbContext context, ICustomerService userService, 
            ISessionService sessionService, ISessionHelper sessionHelper)
        {
            _logger = logger;
            _context = context;
            _customerService = userService;
            _sessionService = sessionService;
            _sessionHelper = sessionHelper;
        }

        public async ValueTask<CreateSessionResponse> HandleAsync(CreateSessionCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            try
            {
                var customer = await _customerService.GetCustomerAsync(command.Username);

                _customerService.ValidateCustomerAccount(customer);

                var oldSessionCustomer = await _sessionHelper.FindOldSession(_context, customer.Username);

                if (oldSessionCustomer.Any())
                    _sessionHelper.RemoveAllSession(_context, oldSessionCustomer);
            }
            catch(CoreException ex)
            {
                _logger.LogInformation($"To create session is required active customer account. {ex.Message}.");
                throw;
            }
            
            _sessionService.RemoveAllSession(command.Username);

            var sessionId = GenerateNewSession();

            var session = new Session(sessionId, customer, SessionState.NotAuthorized);

            await _context.AddAsync(session, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var expirationDate = session.GenerationDate;

            _logger.LogInformation($"New session {sessionId} was has beed created.");

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
