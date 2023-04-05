using Microsoft.Extensions.Logging;
using Rental.Core.Base;
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

namespace Rental.Infrastructure.Handlers.Account.Command.LoginSession
{
    public class AuthenticationSessionHandler : ICommandHandler<AuthenticationSessionCommand, AuthenticationSessionResponse>
    {
        private readonly ILogger<AuthenticationSessionHandler> _logger;
        private readonly IPasswordHelper _passwordHelper;
        private readonly ICustomerHelper _customerHelper;
        private readonly ISessionHelper _sessionHelper;
        private readonly IHttpContextWrapper _httpContextWrapper;
        private readonly ApplicationDbContext _context;
        
        public AuthenticationSessionHandler(ILogger<AuthenticationSessionHandler> logger, ApplicationDbContext context,
            IPasswordHelper passwordHelper, ICustomerHelper customerHelper, ISessionHelper sessionHelper, IHttpContextWrapper httpContextWrapper)
        {
            _logger = logger;
            _passwordHelper = passwordHelper;
            _customerHelper = customerHelper;
            _sessionHelper = sessionHelper;
            _httpContextWrapper = httpContextWrapper;
            _context = context;
        }

        public async ValueTask<AuthenticationSessionResponse> HandleAsync(AuthenticationSessionCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(nameof(command));

            AuthenticationSessionResponse authSessionResponse;

            try
            {
                _logger.LogInformation("Starting login process...");

                var session = await _sessionHelper.GetSessionByIdAsync(_httpContextWrapper.GetValueFromRequestHeader("SessionId"));
                if (_sessionHelper.SessionExpired(session))
                    throw new CoreException(ErrorCode.SessionExpired, $"Session {session.SessionIdentifier} expired.");

                var customer = await _customerHelper.GetCustomerAsync(command.Request.Username);
                _customerHelper.ValidateCustomerAccount(customer);

                var password = await _passwordHelper.GetActivePassword(customer);
                _passwordHelper.ComparePasswords(password.Hash, command.Request.Password);

                if(password.NewPassword)
                    password.ChangePasswordMarker();

                session.State = SessionState.Active;
                session.UpdateLastAccessDate();

                authSessionResponse = new AuthenticationSessionResponse
                {
                    SessionId = session.SessionIdentifier,
                    SessionState = session.State.ToString(),
                    ExpirationTime = session.LastAccessDate.AddMinutes(10)
                };

                _logger.LogInformation("Authentication process has been successful.");

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Authentication session precess failed.");
                throw new Exception(ex.Message, ex);
            }

            return authSessionResponse;
        }
    }
}
