using Rental.Core.Enum;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
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
        private readonly IUserHelper userHelper;
        private readonly ICustomerService customerService;
        private readonly IPasswordHelper passwordHelper;
        private readonly ISessionService sessionService;

        public AuthenticationSessionHandler(IUserHelper userHelper, ICustomerService customerService, IPasswordHelper passwordHelper,
                                            ISessionService sessionService)
        {
            this.userHelper = userHelper;
            this.customerService = customerService;
            this.passwordHelper = passwordHelper;
            this.sessionService = sessionService;
        }

        public async ValueTask<AuthenticationSessionResponse> HandleAsync(AuthenticationSessionCommand command, CancellationToken cancellationToken = default)
        {
            var request = command.Request;

            ValidationParameter.FailIfNull(nameof(request));

            AuthenticationSessionResponse authSessionResponse = null;

            try
            {
                var session = sessionService.GetSessionAsync(request.SessionId).Result;

                var customer = customerService.GetCustomerAsync(command.Request.Username).Result;

                customerService.ValidateCustomerAccountAsync(customer);

                var password = passwordHelper.GetActivePassword(customer).Result;

                //Check input password
                passwordHelper.ComaprePasswords(password, request.Password);

                session.State = SessionState.Active;
                session.UpdateLastAccessDate();

                authSessionResponse = new AuthenticationSessionResponse
                {
                    SessionId = session.SessionId,
                    SessionState = session.State.ToString(),
                    ExpirationTime = session.LastAccessDate.AddMinutes(10)
                };
                
            }
            catch (Exception ex)
            {
                // log error
            }

            return authSessionResponse;
        }
    }
}
