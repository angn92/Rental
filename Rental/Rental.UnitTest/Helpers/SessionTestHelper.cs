using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rental.Test.Helpers
{
    public static class SessionTestHelper
    {
        public static Session CreateSession(ApplicationDbContext _context, string sessionId, Customer customer, SessionState sessionState, Action<Session> action = null)
        {
            var session = new Session(sessionId, customer, sessionState);
            action?.Invoke(session);

            _context.Add(session);
            _context.SaveChanges();

            return session;
        }

        public static Session FindSession(ApplicationDbContext _context, string sessionIdentifier)
        {
            return _context.Sessions.SingleOrDefault(x => x.SessionIdentifier == sessionIdentifier);
        }

        public static List<Session> FindAllCustomerSession(ApplicationDbContext _context, string username)
        {
            return _context.Sessions.Where(x => x.Customer.Username == username).ToList();
        }
    }
}
