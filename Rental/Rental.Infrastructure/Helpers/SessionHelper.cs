using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Helpers
{
    public interface ISessionHelper
    {
        Task<Session> CreateSession(User user);
        Task<Session> GetSessionAsync(string idSession);
        void RemoveSession(string idSession);
    }

    public class SessionHelper : ISessionHelper
    {
        private readonly RentalContext _context;

        public SessionHelper(RentalContext context)
        {
            _context = context;
        }


        public async Task<Session> CreateSession(User user)
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
            throw new NotImplementedException();
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
