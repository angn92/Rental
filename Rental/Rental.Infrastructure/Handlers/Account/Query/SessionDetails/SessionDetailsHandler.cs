using Rental.Core.Validation;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Query.SessionDetails
{
    public class SessionDetailsHandler : IQueryHandler<SessionDetailsRq, SessionDetailsRs>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionHelper _sessionHelper;

        public SessionDetailsHandler(ApplicationDbContext context, ISessionHelper sessionHelper)
        {
            _context = context;
            _sessionHelper = sessionHelper;
        }

        public async ValueTask<SessionDetailsRs> HandleAsync(SessionDetailsRq query, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNullOrEmpty(query.Session.ToString());

            var session = await _sessionHelper.GetSessionByIdAsync(_context, query.Session);

            if (session == null)
                throw new CoreException(ErrorCode.SessionDoesNotExist, "Session does not exist.");

            if (_sessionHelper.SessionExpired(session))
                throw new CoreException(ErrorCode.SessionExpired, $"Session {query.Session} expired.");

            var statusSession = _sessionHelper.CheckSessionStatus(session);

            session.UpdateLastAccessDate();

            return new SessionDetailsRs
            {
                SessionId = session.SessionId,
                SessionStatus = statusSession.ToString(),
                ValidTo = session.LastAccessDate.AddMinutes(5) // add parameter to dictionary and replace 
            };
        }
    }
}
