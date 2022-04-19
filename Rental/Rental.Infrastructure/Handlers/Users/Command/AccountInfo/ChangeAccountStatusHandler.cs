using Rental.Core.Validation;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Users.Command.AccountInfo
{
    public class ChangeAccountStatusHandler : ICommandHandler<ChangeAccountStatus>
    {
        private readonly ApplicationDbContext context;
        private readonly ISessionHelper sessionHelper;
        private readonly IUserHelper userHelper;
        private readonly ICustomerService customerService;

        public ChangeAccountStatusHandler(ApplicationDbContext context, ISessionHelper sessionHelper, IUserHelper userHelper,
                                            ICustomerService customerService)
        {
            this.context = context;
            this.sessionHelper = sessionHelper;
            this.userHelper = userHelper;
            this.customerService = customerService;
        }

        public async Task HandleAsync(ChangeAccountStatus command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNullOrEmpty(command.Username);

            var customer = await customerService.GetCustomerAsync(command.Username);

            await sessionHelper.ValidateSession(context, customer);

            await userHelper.ChangeAccountStatus(context, customer, command.Status);
        }
    }
}
