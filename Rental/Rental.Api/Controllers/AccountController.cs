using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Command.Users.Command;
using Rental.Infrastructure.Handlers.Users.Queries;
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

        [HttpPost("register/user")]
        public async Task RegisterAccount([FromBody][NotNull] RegisterUser command)
        {
            await _commandDispatcher.DispatchAsync(command);
        }

        [HttpGet("user")]
        public async Task<GetUserDetailsRs> GetUserDetails([FromQuery][NotNull] GetUserDetailsRq query)
        {
            return await _queryDispatcher.DispatchAsync<GetUserDetailsRq, GetUserDetailsRs>(query);
        }

        //[HttpPost("session")]
        //public async Task CreateSession([FromBody] [NotNull] CreateSessionCommand command)
        //{
        //    await _commandDispatcher.DispatchAsync(command);
        //}
    }
}
