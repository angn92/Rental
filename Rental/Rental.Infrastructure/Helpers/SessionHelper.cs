﻿using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Helpers
{
    public interface ISessionHelper
    {
        Task<Session> CreateSession([NotNull] Customer username);
        Task RemoveSession([NotNull] string session);
        void RemoveAllSession([NotNull] string username);
        Task ChangeSessionStatus([NotNull] string sessionId, [NotNull] SessionState sessionState);
        SessionState CheckSessionStatus([NotNull] Session session);
        bool SessionExpired([NotNull] Session session);
        void ValidateSessionStatus([NotNull] Session session);
        Task<Session> GetSessionAsync([NotNull] Customer customer);
        Task<Session> GetSessionByIdAsync([NotNull] string sessionId);
        string GenerateSessionId();
        Task<List<Session>> FindOldSession([NotNull] string username);
    }

    public class SessionHelper : ISessionHelper
    {
        private readonly ILogger<SessionHelper> _logger;
        private readonly ApplicationDbContext _context;

        public SessionHelper(ILogger<SessionHelper> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;  
        }

        public async Task<Session> CreateSession([NotNull] Customer customer)
        {
            var session = CreateNotAuthorizeSession(customer, SessionState.NotAuthorized);

            await _context.AddAsync(session);
            await _context.SaveChangesAsync();

            return session;
        }

        public async Task<Session> GetSessionAsync([NotNull] string idSession)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(x => x.SessionIdentifier == idSession);

            if (session is null)
                throw new CoreException(ErrorCode.SessionDoesNotExist, $"SessionId {idSession} does not exist.");

            return session;
        }

        public async Task RemoveSession([NotNull] string idSession)
        {
            var sessionToRemove = await _context.Sessions.FirstOrDefaultAsync(x => x.SessionIdentifier == idSession);

            if (sessionToRemove == null)
            {
                throw new CoreException(ErrorCode.SessionDoesNotExist, $"SessionId {idSession} does not exist.");
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

            _context.SaveChanges();
        }

        public async Task ChangeSessionStatus([NotNull] string sessionId, [NotNull] SessionState sessionState)
        {
            var session = await GetSessionAsync(sessionId);

            if (session.State.Equals(sessionState))
                return;

            session.ChangeState(sessionState);

            await _context.SaveChangesAsync();
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
            return session.LastAccessDate.AddMinutes(25) < DateTime.UtcNow;
        }

        public void ValidateSessionStatus([NotNull] Session session)
        {
            if (session.State == SessionState.NotAuthorized)
                throw new CoreException(ErrorCode.SessionNotAuthorized, $"Session {session.SessionIdentifier} is not authorized.");

            if (session.State == SessionState.Expired)
                throw new CoreException(ErrorCode.SessionExpired, $"Session {session.SessionIdentifier} is expired.");
        }

        public async Task<Session> GetSessionAsync([NotNull] Customer customer)
        {
            return await _context.Sessions.SingleOrDefaultAsync(x => x.IdCustomer == customer.CustomerId);
        }

        public async Task<Session> GetSessionByIdAsync([NotNull] string sessionId)
        {
            var session = await _context.Sessions.SingleOrDefaultAsync(x => x.SessionIdentifier == sessionId);

            if (session == null)
                throw new CoreException(ErrorCode.SessionDoesNotExist, $"Customer session with id {sessionId} does not exist.");

            return session;
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

        public async Task<List<Session>> FindOldSession(string username)
        {
            return await _context.Sessions.Where(x => x.Customer.Username == username).ToListAsync();
        }

        private Session CreateNotAuthorizeSession(Customer customer, SessionState sessionState)
        {
            return new Session(GenerateSessionId(), customer, sessionState);
        }
    }
}