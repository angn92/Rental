using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.UserService;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.SessionService
{
    public class SessionService : ISessionService
    {
        private readonly RentalContext _context;
        private readonly IUserService _userService;
        private readonly IUserHelper _userHelper;

        public SessionService(RentalContext context, IUserService userService, IUserHelper userHelper)
        {
            _context = context;
            _userService = userService;
            _userHelper = userHelper;
        }

        public async Task<Session> CreateSessionAsync(User user)
        {
            var isUserExist = await _userService.GetUserAsync(user.Username);

            if(isUserExist == null)
            {
                throw new CoreException(ErrorCode.UserNotExist, $"User {user.Username} does not exist.");
            }

            var accountStatus = _userHelper.CheckAccountStatus(user);

            if(accountStatus == AccountStatus.Blocked.ToString())
            {
                throw new CoreException(ErrorCode.AccountBlocked, "Account is blocked.");
            }

            if(accountStatus == AccountStatus.NotActive.ToString())
            {
                throw new CoreException(ErrorCode.AccountNotActive, "Account is not active.");
            }

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
