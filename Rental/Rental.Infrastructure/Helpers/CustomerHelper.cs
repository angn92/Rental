using JetBrains.Annotations;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Services;

using System.Threading.Tasks;

namespace Rental.Infrastructure.Helpers
{
    public interface ICustomerHelper : IService
    {
        string CheckAccountStatus(Customer user);
        Task ChangeAccountStatus([NotNull] ApplicationDbContext context, [NotNull] Customer customer, AccountStatus status);
        void ValidateCustomerAccount([NotNull] Customer customer);
    }

    public class CustomerHelper : ICustomerHelper
    {
        //Customer can change status only on NotActive
        public async Task ChangeAccountStatus([NotNull] ApplicationDbContext context, [NotNull] Customer customer, AccountStatus status)
        {
            if (customer.Status == status)
                return;

            customer.Status = status;
            await context.SaveChangesAsync();
        }

        public string CheckAccountStatus([NotNull] Customer customer)
        {
            string status = null;

            switch (customer.Status)
            {
                case AccountStatus.Active:
                    status = AccountStatus.Active.ToString();
                    break;
                case AccountStatus.Blocked:
                    status = AccountStatus.Blocked.ToString();
                    break;
                case AccountStatus.NotActive:
                    status = AccountStatus.NotActive.ToString();
                    break;
            }
            return status;
        }

        public void ValidateCustomerAccount([NotNull] Customer customer)
        {
            if (customer.Status == AccountStatus.Blocked)
                throw new CoreException(ErrorCode.AccountBlocked, $"Account for customer {customer.Username} is blocked.");

            if (customer.Status == AccountStatus.NotActive)
                throw new CoreException(ErrorCode.AccountNotActive, $"Account for customer {customer.Username} is not active.");
        }
    }
}
