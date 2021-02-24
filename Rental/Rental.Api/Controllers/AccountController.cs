using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Command.Users.Command;
using Rental.Infrastructure.Handlers.Users.Queries;
using System.Threading.Tasks;

namespace Rental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public AccountController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost("register/user")]
        public async Task RegisterAccount([FromBody][NotNull] RegisterUser command)
        {
            await _commandDispatcher.DispatchAsync(command);
        }

        [HttpGet("user")]
        public async Task<GetUserDetailsRs> GetUserDetails([FromQuery][NotNull] GetUserDetailsRq query)
        {
            return await _commandDispatcher.DispatchAsync<GetUserDetailsRq, GetUserDetailsRs>(query);
        }
    }
}
