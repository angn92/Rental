using Microsoft.Extensions.Logging;
using Rental.Core.Enum;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Command.AuthorizePassword
{
    public class AuthorizePasswordHandler : ICommandHandler<AuthorizePasswordCommand>
    {
        private readonly ILogger<AuthorizePasswordHandler> logger;
        private readonly ApplicationDbContext context;
        private readonly ISessionHelper sessionHelper;
        private readonly ICustomerService customerService;
        private readonly IPasswordHelper passwordHelper;

        public AuthorizePasswordHandler(ILogger<AuthorizePasswordHandler> logger, ApplicationDbContext context, ISessionHelper sessionHelper, 
            ICustomerService customerService, IPasswordHelper passwordHelper)
        {
            this.logger = logger;
            this.context = context;
            this.sessionHelper = sessionHelper;
            this.customerService = customerService;
            this.passwordHelper = passwordHelper;
        }

        public async ValueTask HandleAsync(AuthorizePasswordCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command.Request);
            ValidationParameter.FailIfNullOrEmpty(command.SessionId.ToString());

            try
            {
                var customer = await customerService.GetCustomerAsync(command.Request.Username);

                var session = await sessionHelper.GetSessionAsync(context, customer);

                if (sessionHelper.CheckSessionStatus(session) != SessionState.NotAuthorized)
                    throw new CoreException(ErrorCode.WrongSessionState, $"Session {session.SessionId} has incorrect state for authorize password.");

                if (sessionHelper.SessionExpired(session))
                    throw new CoreException(ErrorCode.SessionExpired, "Session expired");

                //Find password for given user to authorize
                var password = await passwordHelper.FindPasswordToAuthorize(customer.Username, command.Request.Code);

                if (password != null)
                {
                    password.ActivatePassword();
                    await context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Authorize process is failed. {ex.Message}.");
                throw;
            }
            
        }
    }
}
