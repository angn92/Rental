using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using System.Threading.Tasks;
using System.Threading;
using Rental.Infrastructure.Wrapper;
using Rental.Core.Validation;
using System.Linq;
using Rental.Core.Enum;

namespace Rental.Infrastructure.Handlers.Users.Queries.AccountInfo
{
    public class GetAccountStatusHandler : IQueryHandler<GetAccountStatusRequest, GetAccountStatusResponse>
    {
        private readonly ISessionHelper _sessionHelper;
        private readonly ICustomerHelper _customerHelper;
        private readonly IHttpContextWrapper _httpContextWrapper;

        public GetAccountStatusHandler(ISessionHelper sessionHelper, ICustomerHelper customerHelper, IHttpContextWrapper httpContextWrapper)
        {
            _sessionHelper = sessionHelper;
            _customerHelper = customerHelper;
            _httpContextWrapper = httpContextWrapper;
        }

        public async ValueTask<GetAccountStatusResponse> HandleAsync(GetAccountStatusRequest query, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNullOrEmpty(query.Username);

            var sessionId = _httpContextWrapper.GetValueFromRequestHeader("SessionId");
            var session = await _sessionHelper.GetSessionByIdAsync(sessionId);

            _sessionHelper.ValidateSessionStatus(session);

            if (_sessionHelper.SessionExpired(session))
                throw new CoreException(ErrorCode.SessionExpired, $"Session {sessionId} is expired.");

            var customerAccount = await _customerHelper.GetCustomerAsync(query.Username);
            var customerPasswordStatus = customerAccount.Passwords
                    .Where(x => x.Status == PasswordStatus.Active)
                    .OrderByDescending(x => x.UpdatedAt)
                    .FirstOrDefault();

            return new GetAccountStatusResponse
            {
                Username = customerAccount.Username,
                AccountStatus = customerAccount.Status.ToString(),
                PasswordStatus = customerPasswordStatus.Status.ToString()
            };
        }
    }
}
