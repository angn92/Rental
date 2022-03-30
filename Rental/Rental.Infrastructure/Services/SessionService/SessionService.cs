using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.SessionService
{
    public class SessionService : ISessionService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICustomerService _customerService;
        private readonly IUserHelper _userHelper;

        public SessionService(ApplicationDbContext context, ICustomerService customerService, IUserHelper userHelper)
        {
            _context = context;
            _customerService = customerService;
            _userHelper = userHelper;
        }

        public async Task<Session> CreateSessionAsync(Customer user)
        {
            var userAccount = await _customerService.GetCustomerAsync(user.Username);

            if(userAccount == null)
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

            var session = new Session(sessionId, userAccount);

            await _context.AddAsync(session);
            await _context.SaveChangesAsync();

            return session;
        }

        public async Task<Session> GetSessionAsync(int idSession)
        {
            return await _context.Sessions.FirstOrDefaultAsync(x => x.SessionId == idSession);
        }

        public async Task RemoveSession(int idSession)
        {
            var sessionToRemove = await _context.Sessions.FirstOrDefaultAsync(x => x.SessionId == idSession);

            if (sessionToRemove == null)
            {
                throw new CoreException(ErrorCode.SessionDoesNotExist, $"Session {idSession} does not exist.");
            }

            _context.Sessions.Remove(sessionToRemove);
            await _context.SaveChangesAsync();
        }

        private int GenerateNewSession()
        {
            return RandomNumberGenerator.GetInt32(100000, 999999);
        }
    }
}
