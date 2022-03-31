﻿using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Command.Users.Command.Register;
using Rental.Infrastructure.Handlers.Password;
using Rental.Infrastructure.Handlers.Sessions;
using Rental.Infrastructure.Handlers.Users.Command.AccountInfo;
using Rental.Infrastructure.Handlers.Users.Queries;
using Rental.Infrastructure.Query;
using System.Threading.Tasks;

namespace Rental.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandDispatcher"></param>
        /// <param name="queryDispatcher"></param>
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
        [HttpPost("register/user")]
        public async Task RegisterAccount([FromBody][NotNull] RegisterUser command)
        {
            await _commandDispatcher.DispatchAsync(command);
        }

        /// <summary>
        /// Return customer details.
        /// </summary>
        /// <param name="query">Give a nick for user.</param>
        /// <returns></returns>
        [HttpGet("user/details")]
        
        public async Task<GetCustomerDetailsRs> GetUserDetails([FromQuery][NotNull] GetCustomerDetailsRq query)
        {
            return await _queryDispatcher.DispatchAsync<GetCustomerDetailsRq, GetCustomerDetailsRs>(query);
        }

        /// <summary>
        /// Create new session for customer.
        /// </summary>
        /// <param name="username">Username parameter.</param>
        /// <returns></returns>
        [HttpPost("session")]
        public async Task<CreateSessionResponse> CreateSeesion([FromQuery] [NotNull] string username)
        {
            var command = new CreateSessionCommand
            {
                Username = username
            };

            return await _commandDispatcher.DispatchAsync<CreateSessionCommand, CreateSessionResponse>(command);
        }

        /// <summary>
        /// Change password for customer account.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("change/password")]
        public async Task ChangePassword([FromBody][NotNull] ChangePasswordCommand command)
        {
            await _commandDispatcher.DispatchAsync(command);
        }

        /// <summary>
        /// Change account status.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPut("account/status")]
        public async Task ChangeAccountStatus([FromQuery] string username)
        {
            var command = new ChangeAccountStatus
            {
                Username = username
            };

            await _commandDispatcher.DispatchAsync<ChangeAccountStatus, ChangeAccountStatusResponse>(command);
        }

        //[HttpGet("account/status")]
        //public async Task GetAccountStatus([FromQuery] string username)
        //{

        //}
    }
}
