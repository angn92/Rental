using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using System;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Helpers
{
    public interface ISessionHelper
    {
        SessionState CheckSessionStatus([NotNull] Session session);
        bool SessionExpired([NotNull] Session session);
        void ValidateSession([NotNull] Session session);
        Task<Session> GetSessionAsync([NotNull] ApplicationDbContext context, [NotNull] Customer customer);
        Task<Session> GetSessionByIdAsync([NotNull] ApplicationDbContext context, [NotNull] int sessionId);
        string GenerateSessionId();
    }

    public class SessionHelper : ISessionHelper
    {
        public SessionState CheckSessionStatus([NotNull] Session session)
        {
            if(session.State == SessionState.NotAuthorized)
                return SessionState.NotAuthorized;

            if (session.State == SessionState.Expired)
                return SessionState.Expired;

            return SessionState.Active;
        }

        public bool SessionExpired([NotNull] Session session)
        {
            return session.LastAccessDate.AddMinutes(5) < DateTime.UtcNow;
        }

        public void ValidateSession([NotNull] Session session)
        {
            if (session == null)
                throw new CoreException(ErrorCode.SessionDoesNotExist, "Session does not exist.");

            if (session.State == SessionState.NotAuthorized)
                throw new CoreException(ErrorCode.SessionNotAuthorized, $"Session {session.SessionId} is not authorized.");

            if (session.State == SessionState.Expired)
                throw new CoreException(ErrorCode.SessionExpired, $"Session {session.SessionId} is expired.");
        }

        public async Task<Session> GetSessionAsync([NotNull] ApplicationDbContext context, [NotNull] Customer customer)
        {
            return await context.Sessions.SingleOrDefaultAsync(x => x.IdCustomer == customer.CustomerId);
        }

        public async Task<Session> GetSessionByIdAsync([NotNull] ApplicationDbContext context, [NotNull] int sessionId)
        {
            return await context.Sessions.SingleOrDefaultAsync(x => x.SessionId == sessionId);
        }

        /// <summary>
        /// Generate random string session Id.
        /// </summary>
        /// <returns>New ession id</returns>
        public string GenerateSessionId()
        {
            var randomSession = Guid.NewGuid().ToString();

            randomSession = randomSession.Replace("-", "");

            return randomSession;
        }
    }
}