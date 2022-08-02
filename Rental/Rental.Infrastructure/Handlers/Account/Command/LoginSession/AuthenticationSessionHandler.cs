using Rental.Core.Enum;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
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
        private readonly ICustomerService customerService;
        private readonly IPasswordHelper passwordHelper;
        private readonly ISessionService sessionService;
        private readonly ApplicationDbContext context;

        public AuthenticationSessionHandler(ICustomerService customerService, IPasswordHelper passwordHelper, ISessionService sessionService,
                                            ApplicationDbContext context)
        {
            this.customerService = customerService;
            this.passwordHelper = passwordHelper;
            this.sessionService = sessionService;
            this.context = context;
        }

        public async ValueTask<AuthenticationSessionResponse> HandleAsync(AuthenticationSessionCommand command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(nameof(command));

            AuthenticationSessionResponse authSessionResponse;
            try
            {
                var session = await sessionService.GetSessionAsync(command.SessionId);

                var customer = await customerService.GetCustomerAsync(command.Request.Username);

                customerService.ValidateCustomerAccountAsync(customer);

                var password = await passwordHelper.GetActivePassword(customer);

                //Check input password
                passwordHelper.ComaprePasswords(password, command.Request.Password);

                password.ChangePasswordMarker();

                session.State = SessionState.Active;
                session.UpdateLastAccessDate();

                authSessionResponse = new AuthenticationSessionResponse
                {
                    SessionId = session.SessionId,
                    SessionState = session.State.ToString(),
                    ExpirationTime = session.LastAccessDate.AddMinutes(10)
                };

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return authSessionResponse;
        }
    }
}
