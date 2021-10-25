using JetBrains.Annotations;
using Rental.Core.Validation;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Services.UserService;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Users.Queries
{
    public class GetCustomerDetailsHandler : IQueryHandler<GetCustomerDetailsRq, GetCustomerDetailsRs>
    {
        private readonly IUserService _userService;

        public GetCustomerDetailsHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetCustomerDetailsRs> HandleAsync([NotNull] GetCustomerDetailsRq command)
        {
            ValidationParameter.FailIfNull(command);

            var customerAccount = await _userService.GetCustomerAsync(command.Nick);

            return new GetCustomerDetailsRs
            {
                Fullname = customerAccount.FirstName + " " + customerAccount.LastName,
                Username = customerAccount.Username,
                Email = customerAccount.Email,
                Status = customerAccount.Status
            };
        }
    }
}
