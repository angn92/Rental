using JetBrains.Annotations;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using System;

namespace Rental.Test.Helpers
{
    public static class CreateSessionTestHelper
    {
        public static Session CreateSession([NotNull] ApplicationDbContext context, string sessionId, Customer customer,
                        SessionState sessionState, Action<Session> action = null)
        { 
            var session = new Session(sessionId, customer, sessionState);

            action?.Invoke(session);

            context.Add(session);
            context.SaveChanges();

            return session;
        }
    }
}