using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using System.Linq;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.SessionService
{
    public class SessionService : ISessionService
    {
        private readonly ILogger<SessionService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ICustomerService _customerService;
        private readonly IUserHelper _userHelper;
        private readonly ISessionHelper _sessionHelper;

        public SessionService(ILogger<SessionService> logger, ApplicationDbContext context, ICustomerService customerService, IUserHelper userHelper,
            ISessionHelper sessionHelper)
        {
            _logger = logger;
            _context = context;
            _customerService = customerService;
            _userHelper = userHelper;
            _sessionHelper = sessionHelper;
        }

        public async Task<Session> CreateSession([NotNull] Customer customer)
        {
            var session = _sessionHelper.CreateNotAuthorizeSession(customer, SessionState.NotAuthorized);
            _context.Add(session);

            await _context.SaveChangesAsync();

            return session;
        }

        public async Task<Session> GetSessionAsync([NotNull] int idSession)
        {
            return await _context.Sessions.FirstOrDefaultAsync(x => x.SessionId == idSession);
        }

        public void RemoveAllSession([NotNull] string username)
        {
            var session = _context.Sessions.Where(x => x.Customer.Username == username).ToList();

            if(session is null)
                return;

            _logger.LogInformation($"Was remove {session.Count} old session for customer {username}.");

            _context.Remove(session);
            _context.SaveChangesAsync();
        }

        public async Task RemoveSession([NotNull] int idSession)
        {
            var sessionToRemove = await _context.Sessions.FirstOrDefaultAsync(x => x.SessionId == idSession);

            if (sessionToRemove == null)
            {
                throw new CoreException(ErrorCode.SessionDoesNotExist, $"Session {idSession} does not exist.");
            }

            _context.Sessions.Remove(sessionToRemove);
            await _context.SaveChangesAsync();
        }

        

        public Task<Session> ChangeSessionStatus([NotNull] int sessionId)
        {
            throw new System.NotImplementedException();
        }
    }
}
