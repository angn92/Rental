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
        //Task<Session> CreateSession(ApplicationDbContext context, Customer customer);

        Task ValidateSession([NotNull] ApplicationDbContext context, [NotNull] Customer customerId);
    }

    public class SessionHelper : ISessionHelper
    {
        public void CheckSessionStatus(Session session)
        {
            if(session.State == SessionState.NotActive)
            {
                throw new CoreException(ErrorCode.SessioNotActive, $"Given session {session.SessionId} is not active.");
            }
        }

        //public async Task<Session> CreateSession(ApplicationDbContext context, [NotNull] Customer customer)
        //{
        //    var oldSession = await GetSession(context, customer);

        //    if (oldSession is not null)
        //        DeleteOtherSession(context, oldSession);

        //    var sessionId = new Guid().ToString();
        //    var session = new Session(sessionId, customer);

        //    return session;
        //}

        private void DeleteOtherSession(ApplicationDbContext context, [NotNull] Session session)
        {
            context.Remove(session);
            context.SaveChangesAsync();
        }

        

        public bool SessionExpired(Session session)
        {
            return session.LastAccessDate.AddMinutes(5) < DateTime.UtcNow;
        }

        public async Task ValidateSession([NotNull] ApplicationDbContext context, [NotNull] Customer customerId)
        {
            var session = await GetSession(context, customerId);

            if (session == null)
                throw new CoreException(ErrorCode.SessionDoesNotExist, "Session does not exist.");

            if (session.State == SessionState.NotActive)
                throw new CoreException(ErrorCode.SessioNotActive, $"Session {session.SessionId} is not active, should has status active.");

            if (session.State == SessionState.NotAuthorized)
                throw new CoreException(ErrorCode.SessionNotAuthorized, $"Session {session.SessionId} is not authorized");
        }

        private static async Task<Session> GetSession(ApplicationDbContext context, Customer customer)
        {
            return await context.Sessions.SingleOrDefaultAsync(x => x.IdCustomer == customer.CustomerId);
        }
    }
}
