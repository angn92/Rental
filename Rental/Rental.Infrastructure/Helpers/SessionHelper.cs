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
        void CheckSessionStatus(Session session);
        bool SessionExpired(Session session);
        void ValidateSession([NotNull] Session session);
        Task<Session> GetSessionAsync([NotNull] ApplicationDbContext context, [NotNull] Customer customer);
    }

    public class SessionHelper : ISessionHelper
    {
        public void CheckSessionStatus(Session session)
        {
            if(session.State == SessionState.NotAuthorized)
            {
                throw new CoreException(ErrorCode.SessionNotAuthorized, $"Given session {session.SessionId} is not authorized yet.");
            }
        }

        public bool SessionExpired(Session session)
        {
            return true;// session.LastAccessDate.AddMinutes(5) < DateTime.UtcNow;
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
    }
}
