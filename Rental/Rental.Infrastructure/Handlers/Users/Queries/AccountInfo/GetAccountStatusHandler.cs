using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Services.SessionService;
using Rental.Infrastructure.Services.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Users.Queries.AccountInfo
{
    public class GetAccountStatusHandler : IQueryHandler<GetAccountStatusRq, GetAccountStatusRs>
    {
        private readonly ISessionService _sessionService;
        private readonly ISessionHelper _sessionHelper;
        private readonly IUserService _userService;

        public GetAccountStatusHandler(ISessionService sessionService, ISessionHelper sessionHelper,
                                        IUserService userService)
        {
            _sessionService = sessionService;
            _sessionHelper = sessionHelper;
            _userService = userService;
        }

        public async Task<GetAccountStatusRs> HandleAsync(GetAccountStatusRq query)
        {
            var session = await _sessionService.GetSessionAsync(query.IdSession);

            if(session is null)
            {
                throw new CoreException(ErrorCode.SessionDoesNotExist, $"Session not created.");
            }

            _sessionHelper.CheckSessionStatus(session);
            _sessionHelper.SessionExpired(session);

            var userAccount = await _userService.GetUserAsync(query.Username);

            return new GetAccountStatusRs
            {
                Username = userAccount.Username,
                AccountStatus = userAccount.Status.ToString()
            };
        }
    }
}
