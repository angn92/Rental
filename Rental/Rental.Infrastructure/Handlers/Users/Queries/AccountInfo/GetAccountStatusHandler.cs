using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Services.SessionService;
using Rental.Infrastructure.Services.CustomerService;
using System.Threading.Tasks;
using System.Threading;
using Rental.Core.Domain;

namespace Rental.Infrastructure.Handlers.Users.Queries.AccountInfo
{
    public class GetAccountStatusHandler : IQueryHandler<GetAccountStatusRq, GetAccountStatusRs>
    {
        private readonly ISessionService _sessionService;
        private readonly ISessionHelper _sessionHelper;
        private readonly ICustomerService _customerService;

        public GetAccountStatusHandler(ISessionService sessionService, ISessionHelper sessionHelper,
                                        ICustomerService customerService)
        {
            _sessionService = sessionService;
            _sessionHelper = sessionHelper;
            _customerService = customerService;
        }

        public async Task<GetAccountStatusRs> HandleAsync(GetAccountStatusRq query, CancellationToken cancellationToken = default)
        {
            Session session = null;// await _sessionService.GetSessionAsync(query.IdSession);

            if(session is null)
            {
                throw new CoreException(ErrorCode.SessionDoesNotExist, $"Session not created.");
            }

            _sessionHelper.CheckSessionStatus(session);
            _sessionHelper.SessionExpired(session);

            var userAccount = await _customerService.GetCustomerAsync(query.Username);

            return new GetAccountStatusRs
            {
                Username = userAccount.Username,
                AccountStatus = userAccount.Status.ToString()
            };
        }
    }
}
