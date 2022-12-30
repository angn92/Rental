using Microsoft.Extensions.Logging;
using Rental.Core.Enum;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
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

        public AuthorizePasswordHandler(ILogger<AuthorizePasswordHandler> logger, ApplicationDbContext context, ISessionHelper sessionHelper, 
             IPasswordHelper passwordHelper, ICustomerHelper customerHelper)
        {
            _logger = logger;
            _context = context;
            _sessionHelper = sessionHelper;
            _passwordHelper = passwordHelper;
            _customerHelper = customerHelper;
        }

        public async ValueTask HandleAsync(AuthorizePasswordCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command.Request);
            ValidationParameter.FailIfNullOrEmpty(command.SessionId.ToString());

            try
            {
                var customer = await _customerHelper.GetCustomerAsync(_context, command.Request.Username);

                var session = await _sessionHelper.GetSessionAsync(_context, customer);

                if (_sessionHelper.CheckSessionStatus(session) != SessionState.NotAuthorized)
                    throw new CoreException(ErrorCode.WrongSessionState, $"Session {session.SessionId} has incorrect state for authorize password.");

                if (_sessionHelper.SessionExpired(session))
                    throw new CoreException(ErrorCode.SessionExpired, "Session expired");

                //Find password for given user to authorize
                var password = await _passwordHelper.FindPasswordToAuthorize(customer.Username, command.Request.Code);

                if (password != null)
                {
                    password.ActivatePassword();
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
