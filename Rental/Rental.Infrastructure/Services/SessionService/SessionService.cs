using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.SessionService
{
    public class SessionService : ISessionService
    {
        private readonly ICustomerService _customerService;
        private readonly IUserHelper _userHelper;

        public SessionService(ICustomerService customerService, IUserHelper userHelper)
        {
            _customerService = customerService;
            _userHelper = userHelper;
        }

        public async Task<Session> CreateSessionAsync([NotNull] ApplicationDbContext context, [NotNull] Customer user)
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

            await context.AddAsync(session);
            await context.SaveChangesAsync();

            return session;
        }

        public async Task<Session> GetSessionAsync([NotNull] ApplicationDbContext context, [NotNull] int idSession)
        {
            return await context.Sessions.FirstOrDefaultAsync(x => x.SessionId == idSession);
        }

        public async Task RemoveAllSession([NotNull] ApplicationDbContext context, [NotNull] string username)
        {
            var session = context.Sessions.Where(x => x.Customer.Username == username).ToList();

            context.Remove(session);
            await context.SaveChangesAsync();
        }

        public async Task RemoveSession([NotNull] ApplicationDbContext context, [NotNull] int idSession)
        {
            var sessionToRemove = await context.Sessions.FirstOrDefaultAsync(x => x.SessionId == idSession);

            if (sessionToRemove == null)
            {
                throw new CoreException(ErrorCode.SessionDoesNotExist, $"Session {idSession} does not exist.");
            }

            context.Sessions.Remove(sessionToRemove);
            await context.SaveChangesAsync();
        }

        private int GenerateNewSession()
        {
            return RandomNumberGenerator.GetInt32(100000, 999999);
        }
    }
}
