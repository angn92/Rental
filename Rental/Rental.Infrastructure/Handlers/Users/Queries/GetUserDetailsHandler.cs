using JetBrains.Annotations;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Services.UserService;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Users.Queries
{
    public class GetUserDetailsHandler : IQueryHandler<GetUserDetailsRq, GetUserDetailsRs>
    {
        private readonly IUserService _userService;

        public GetUserDetailsHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetUserDetailsRs> HandleAsync([NotNull] GetUserDetailsRq command)
        {
            var userAccount = await _userService.GetCustomerAsync(command.Nick);

            return new GetUserDetailsRs
            {
                Fullname = userAccount.FirstName + " " + userAccount.LastName,
                Username = userAccount.Username,
                Email = userAccount.Email,
                Status = userAccount.Status
            };
        }
    }
}
