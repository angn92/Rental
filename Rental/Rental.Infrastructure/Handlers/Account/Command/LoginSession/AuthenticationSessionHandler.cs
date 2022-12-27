using Microsoft.Extensions.Logging;
using Rental.Core.Enum;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using Rental.Infrastructure.Services.SessionService;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Command.LoginSession
{
    public class AuthenticationSessionHandler : ICommandHandler<AuthenticationSessionCommand, AuthenticationSessionResponse>
    {
        private readonly ILogger<AuthenticationSessionHandler> _logger;
        private readonly ICustomerService _customerService;
        private readonly IPasswordHelper _passwordHelper;
        private readonly ISessionService _sessionService;
        private readonly ICustomerHelper _customerHelper;
        private readonly ApplicationDbContext _context;
        
        public AuthenticationSessionHandler(ILogger<AuthenticationSessionHandler> logger, ApplicationDbContext context, ICustomerService customerService,
            IPasswordHelper passwordHelper, ISessionService sessionService, ICustomerHelper customerHelper)
        {
            _logger = logger;
            _customerService = customerService;
            _passwordHelper = passwordHelper;
            _sessionService = sessionService;
            _customerHelper = customerHelper;
            _context = context;
        }

        public async ValueTask<AuthenticationSessionResponse> HandleAsync(AuthenticationSessionCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(nameof(command));

            AuthenticationSessionResponse authSessionResponse;

            try
            {
                _logger.LogInformation("Starting login process...");

                var session = await _sessionService.GetSessionAsync(command.SessionId);

                var customer = await _customerService.GetCustomerAsync(command.Request.Username);

                _customerHelper.ValidateCustomerAccount(customer);

                var password = await _passwordHelper.GetActivePassword(customer);

                _passwordHelper.ComaprePasswords(password, command.Request.Password);

                if(password.NewPassword)
                    password.ChangePasswordMarker();

                session.State = SessionState.Active;
                session.UpdateLastAccessDate();

                authSessionResponse = new AuthenticationSessionResponse
                {
                    SessionId = session.SessionId,
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
