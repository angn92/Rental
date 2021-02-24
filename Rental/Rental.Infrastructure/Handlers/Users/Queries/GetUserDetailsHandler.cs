﻿using JetBrains.Annotations;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Services.UserService;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Users.Queries
{
    public class GetUserDetailsHandler : ICommandHandler<GetUserDetailsRq, GetUserDetailsRs>
    {
        private readonly IUserService _userService;

        public GetUserDetailsHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetUserDetailsRs> HandleAsync([NotNull] GetUserDetailsRq command)
        {
            var userAccount = await _userService.GetUserAsync(command.Nick);

            return new GetUserDetailsRs
            {
                Fullname = userAccount.FullName,
                Username = userAccount.Username,
                Email = userAccount.Emial,
                Status = userAccount.Status
            };
        }
    }
}
