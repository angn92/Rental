using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.SessionService
{
    public class SessionService : ISessionService
    {
        private readonly ApplicationDbContext context;
        private readonly ICustomerService _customerService;
        private readonly IUserHelper _userHelper;

        public SessionService(ApplicationDbContext context, ICustomerService customerService, IUserHelper userHelper)
        {
            this.context = context;
            _customerService = customerService;
            _userHelper = userHelper;
        }

        public async Task<Session> GetSessionAsync([NotNull] int idSession)
        {
            return await context.Sessions.FirstOrDefaultAsync(x => x.SessionId == idSession);
        }

        public async Task RemoveAllSession([NotNull] string username)
        {
            var session = context.Sessions.Where(x => x.Customer.Username == username).ToList();

            context.Remove(session);
            await context.SaveChangesAsync();
        }

        public async Task RemoveSession([NotNull] int idSession)
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
