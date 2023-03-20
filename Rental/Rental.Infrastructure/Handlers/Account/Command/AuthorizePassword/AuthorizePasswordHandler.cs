using Microsoft.Extensions.Logging;
using Rental.Core.Enum;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Wrapper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Command.AuthorizePassword
{
    public class AuthorizePasswordHandler : ICommandHandler<AuthorizePasswordCommand>
    {
        private readonly ILogger<AuthorizePasswordHandler> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ISessionHelper _sessionHelper;
        private readonly IPasswordHelper _passwordHelper;
        private readonly ICustomerHelper _customerHelper;
        private readonly IHttpContextWrapper _httpContextWrapper;

        public AuthorizePasswordHandler(ILogger<AuthorizePasswordHandler> logger, ApplicationDbContext context, ISessionHelper sessionHelper, 
             IPasswordHelper passwordHelper, ICustomerHelper customerHelper, IHttpContextWrapper httpContextWrapper)
        {
            _logger = logger;
            _context = context;
            _sessionHelper = sessionHelper;
            _passwordHelper = passwordHelper;
            _customerHelper = customerHelper;
            _httpContextWrapper = httpContextWrapper;
        }

        public async ValueTask HandleAsync(AuthorizePasswordCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command.Request);

            var sessionId = _httpContextWrapper.GetValueFromRequestHeader("SessionId");

            ValidationParameter.FailIfNull(sessionId);
            try
            {
                var customer = await _customerHelper.GetCustomerAsync(command.Request.Username);

                var session = await _sessionHelper.GetSessionByIdAsync(sessionId);

                if (_sessionHelper.CheckSessionStatus(session) != SessionState.NotAuthorized)
                    throw new CoreException(ErrorCode.WrongSessionState, $"SessionId {session.SessionIdentifier} has incorrect state for authorize password.");

                if (_sessionHelper.SessionExpired(session))
                    throw new CoreException(ErrorCode.SessionExpired, "SessionId expired");

                //Find password for given user to authorize
                var password = await _passwordHelper.FindPasswordToAuthorize(customer.Username);

                if (password != null && _passwordHelper.ComaprePasswords(password.ActivationCode, command.Request.Code))
                {
                    password.ActivatePassword();
                    session.SetSessionActive();
                    customer.ActivateAccount();
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Authorize process is failed. {ex.Message}.");
                throw;
            }
        }
    }
}
