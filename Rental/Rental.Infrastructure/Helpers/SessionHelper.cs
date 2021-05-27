using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.Exceptions;
using System;

namespace Rental.Infrastructure.Helpers
{
    public interface ISessionHelper
    {
        void CheckSessionStatus(Session session);
        bool SessionExpired(Session session);
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

        public bool SessionExpired(Session session)
        {
            return session.LastAccessDate.AddMinutes(5) < DateTime.UtcNow;
        }
    }
}
