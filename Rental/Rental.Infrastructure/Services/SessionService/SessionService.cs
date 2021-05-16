using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.SessionService
{
    public class SessionService : ISessionService
    {
        private readonly RentalContext _context;

        public SessionService(RentalContext context)
        {
            _context = context;
        }

        public async Task<Session> CreateSessionAsync(User user)
        {
            var sessionId = GenerateNewSession();

            var session = new Session(sessionId, SessionState.Active, user);

            await _context.AddAsync(session);
            await _context.SaveChangesAsync();

            return session;
        }

        public async Task<Session> GetSessionAsync(string idSession)
        {
            return await _context.Sessions.FirstOrDefaultAsync(x => x.SessionId == idSession);
        }

        public void RemoveSession(string idSession)
        {
            var sessionToRemove = _context.Sessions.FirstOrDefault(x => x.SessionId == idSession);

            if (sessionToRemove == null)
            {
                throw new CoreException(ErrorCode.SessionDoesNotExist, $"Session {idSession} does not exist.");
            }

            _context.Sessions.Remove(sessionToRemove);
            _context.SaveChanges();
        }

        private static string GenerateNewSession()
        {
            var rng = new RNGCryptoServiceProvider();
            var byteArray = new byte[4];

            rng.GetBytes(byteArray);

            return BitConverter.ToInt32(byteArray, 0).ToString();
        }
    }
}
