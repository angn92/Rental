using Rental.Infrastructure.Command;
using System.Threading.Tasks;
using System.Threading;
using Rental.Core.Validation;
using Rental.Infrastructure.EF;
using System.Security.Cryptography;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Microsoft.Extensions.Logging;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using System.Linq;

namespace Rental.Infrastructure.Handlers.Account.Command.Sessions
{
    public class CreateSessionHandler : ICommandHandler<CreateSessionCommand, CreateSessionResponse>
    {
        private readonly ICustomerHelper _customerHelper;
        private readonly ISessionHelper _sessionHelper;
        private readonly ILogger<CreateSessionHandler> _logger;
        private readonly ApplicationDbContext _context;

        public CreateSessionHandler(ILogger<CreateSessionHandler> logger, ApplicationDbContext context, ICustomerHelper customerHelper, 
            ISessionHelper sessionHelper)
        {
            _logger = logger;
            _context = context;
            _customerHelper = customerHelper;
            _sessionHelper = sessionHelper;
        }

        public async ValueTask<CreateSessionResponse> HandleAsync(CreateSessionCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            try
            {
                var customer = await _customerHelper.GetCustomerAsync(_context, command.Username);

                _customerHelper.ValidateCustomerAccount(customer);

                var oldSessionCustomer = await _sessionHelper.FindOldSession(_context, customer.Username);

                if (oldSessionCustomer.Any())
                    _sessionHelper.RemoveAllSession(customer.Username);

                var sessionId = GenerateNewSession();

                var session = new Session(sessionId, customer, SessionState.NotAuthorized);

                await _context.AddAsync(session, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var expirationDate = session.GenerationDate;

                _logger.LogInformation($"New session {sessionId} was has been created.");

                return new CreateSessionResponse
                {
                    IdSession = session.SessionId,
                    Status = session.State,
                    ExpirationTime = expirationDate
                };
            }
            catch(CoreException ex)
            {
                _logger.LogInformation($"To create session is required active customer account. {ex.Message}.");
                throw;
            } 
        }

        private static int GenerateNewSession()
        {
            return RandomNumberGenerator.GetInt32(100000000, 999999999);
        }
    }
}
