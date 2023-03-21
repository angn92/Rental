using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using System.Threading.Tasks;
using System.Threading;
using Rental.Infrastructure.EF;

namespace Rental.Infrastructure.Handlers.Users.Queries.AccountInfo
{
    public class GetAccountStatusHandler : IQueryHandler<GetAccountStatusRq, GetAccountStatusRs>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionHelper _sessionHelper;
        private readonly ICustomerHelper _customerHelper;

        public GetAccountStatusHandler(ApplicationDbContext context, ISessionHelper sessionHelper, ICustomerHelper customerHelper)
        {
            _context = context;
            _sessionHelper = sessionHelper;
            _customerHelper = customerHelper;
        }

        public async ValueTask<GetAccountStatusRs> HandleAsync(GetAccountStatusRq query, CancellationToken cancellationToken = default)
        {

            var session = await _sessionHelper.GetSessionByIdAsync(query.SessionId);

            _sessionHelper.CheckSessionStatus(session);
            _sessionHelper.SessionExpired(session);

            var userAccount = await _customerHelper.GetCustomerAsync(query.Username);

            return new GetAccountStatusRs
            {
                Username = userAccount.Username,
                AccountStatus = userAccount.Status.ToString()
            };
        }
    }
}
