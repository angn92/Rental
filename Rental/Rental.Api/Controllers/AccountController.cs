using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Handlers.Account.Command.AuthorizePassword;
using Rental.Infrastructure.Handlers.Account.Command.CreateAccount;
using Rental.Infrastructure.Handlers.Account.Command.LoginSession;
using Rental.Infrastructure.Handlers.Account.Commmand.ChangeStatus;
using Rental.Infrastructure.Handlers.Account.Query.AccountDetails;
using Rental.Infrastructure.Handlers.Account.Query.SessionDetails;
using Rental.Infrastructure.Handlers.Password;
using Rental.Infrastructure.Handlers.Account.Command.Sessions;
using Rental.Infrastructure.Query;
using System.Threading.Tasks;

namespace Rental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public AccountController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Create new customer account.
        /// </summary>
        /// <param name="command">Base information about user account.</param>
        /// <returns></returns>
        [HttpPost("Register/Customer")]
        public async Task<RegisterCustomerResponse> RegisterAccount([FromBody][NotNull] RegisterCustomer command)
        {
            ValidationParameter.FailIfNull(command);

            return await _commandDispatcher.DispatchAsync<RegisterCustomer, RegisterCustomerResponse>(command);
        }

        /// <summary>
        /// Return customer details.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("Customer/Details/{username}")]

        public async Task<GetCustomerDetailsRs> GetUserDetails([FromRoute][NotNull] string username)
        {
            var query = new GetCustomerDetailsRq
            {
                Username = username
            };

            return await _queryDispatcher.DispatchAsync<GetCustomerDetailsRq, GetCustomerDetailsRs>(query);
        }

        /// <summary>
        /// Change account status.
        /// </summary>
        /// <returns></returns>
        [HttpPut("Status")]
        public async Task ChangeAccountStatus([FromBody][NotNull] ChangeStatusCommand command)
        {
            await _commandDispatcher.DispatchAsync<ChangeStatusCommand>(command);
        }

        /// <summary>
        /// Create new session for customer.
        /// </summary>
        /// <param name="username">Username parameter for who sessione will be create.</param>
        /// <returns></returns>
        [HttpPost("Session/{username}")]
        public async Task<CreateSessionResponse> CreateSeesion([FromRoute] [NotNull] string username)
        {
            var command = new CreateSessionCommand
            {
                Username = username
            };

            return await _commandDispatcher.DispatchAsync<CreateSessionCommand, CreateSessionResponse>(command);
        }

        /// <summary>
        /// Authentication session given in URL
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Session/Authentication/{sessionId}")]
        public async Task<AuthenticationSessionResponse> LogInSession([FromRoute] int sessionId, [NotNull] AuthenticationSessionRequest request)
        {
            var command = new AuthenticationSessionCommand
            {
                Request = request,
                SessionId = sessionId
            };

            return await _commandDispatcher.DispatchAsync<AuthenticationSessionCommand, AuthenticationSessionResponse>(command);
        }

        /// <summary>
        /// Check session details and update LastAccessDate
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [HttpGet("Details/Session/{sessionId}")]
        public async Task<SessionDetailsRs> VerifySessionDetails([FromRoute][NotNull] int sessionId)
        {
            var request = new SessionDetailsRq
            {
                Session = sessionId
            };

            return await _queryDispatcher.DispatchAsync<SessionDetailsRq, SessionDetailsRs>(request); 
        }

        /// <summary>
        /// Change password for customer account.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("Change/Password")]
        public async Task ChangePassword([FromBody][NotNull] ChangePasswordCommand command)
        {
            await _commandDispatcher.DispatchAsync(command);
        }

        /// <summary>
        /// Method to authorize password for new created account. This method also authorize session which was created during the first step - create account
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Register/Authorize/Password/{sessionId}")]
        public async Task AuthorizePassword([FromRoute] int sessionId, [NotNull] AuthorizePasswordRequest request)
        {
            var command = new AuthorizePasswordCommand
            {
                Request = request,
                SessionId = sessionId
            };

            await _commandDispatcher.DispatchAsync(command);
        }
    }
}
