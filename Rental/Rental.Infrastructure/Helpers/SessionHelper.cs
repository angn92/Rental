using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Services.SessionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        Session CreateNotAuthorizeSession(Customer customer, SessionState notAuthorized);
        Task<List<Session>> FindOldSession([NotNull] ApplicationDbContext context, [NotNull] string username);
        Task<Session> CreateSession([NotNull] Customer username);
        Task RemoveSession([NotNull] int session);
        void RemoveAllSession([NotNull] string username);
        Task ChangeSessionStatus([NotNull] int sessionId, [NotNull] SessionState sessionState);
    }

    public class SessionHelper : ISessionHelper
    {
        public ILogger<SessionService> _logger { get; }
        public ApplicationDbContext _context { get; }
        public ISessionHelper _sessionHelper { get; }

        public SessionHelper(ILogger<SessionService> logger, ApplicationDbContext context, ISessionHelper sessionHelper)
        {
            _logger = logger;
            _context = context;
            _sessionHelper = sessionHelper;
        }

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

        private static int GenerateNewIdSession()
        {
            return RandomNumberGenerator.GetInt32(100000000, 999999999);
        }

        public Session CreateNotAuthorizeSession(Customer customer, SessionState sessionState)
        {
            return new Session(GenerateNewIdSession(), customer, sessionState);
        }

        public async Task<List<Session>> FindOldSession(ApplicationDbContext context, string username)
        {
            return await context.Sessions.Where(x => x.Customer.Username == username).ToListAsync();
        }

        public async Task<Session> CreateSession([NotNull] Customer customer)
        {
            var session = _sessionHelper.CreateNotAuthorizeSession(customer, SessionState.NotAuthorized);

            await _context.AddAsync(session);
            await _context.SaveChangesAsync();

            return session;
        }

        public async Task<Session> GetSessionAsync([NotNull] int idSession)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(x => x.SessionId == idSession);

            if (session is null)
                throw new CoreException(ErrorCode.SessionDoesNotExist, $"Session {idSession} does not exist.");

            return session;
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

        public void RemoveAllSession([NotNull] string username)
        {
            var sessionList = _context.Sessions.Where(x => x.Customer.Username == username).ToList();

            if (!sessionList.Any())
                return;

            foreach (var item in sessionList)
                _context.Remove(item);


            _logger.LogInformation($"Was remove {sessionList.Count} old session for customer {username}.");

            _context.SaveChangesAsync();
        }

        public async Task ChangeSessionStatus([NotNull] int sessionId, [NotNull] SessionState sessionState)
        {
            var session = await GetSessionAsync(sessionId);

            if (session.State.Equals(sessionState))
                return;

            session.ChangeState(sessionState);

            await _context.SaveChangesAsync();
        }
    }
}