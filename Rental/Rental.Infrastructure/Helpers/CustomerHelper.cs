using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Helpers
{
    public interface ICustomerHelper : IService
    {
        string CheckAccountStatus([NotNull] Customer user);
        Task<Customer> GetCustomerAsync([NotNull] ApplicationDbContext context, [NotNull] string username);
        void ValidateCustomerAccount([NotNull] Customer customer);
        bool CheckIfExist([NotNull] ApplicationDbContext context, [NotNull] string username);
    }

    public class CustomerHelper : ICustomerHelper
    {
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

        public async Task<Customer> GetCustomerAsync([NotNull] ApplicationDbContext context, [NotNull] string username)
        {
            var customer = await context.Customers.SingleOrDefaultAsync(x => x.Username == username);

            if (customer is null)
                throw new CoreException(ErrorCode.UserNotExist, $"User {username} does not exist.");

            return customer;
        }

        public void ValidateCustomerAccount([NotNull] Customer customer)
        {
            if (customer.Status == AccountStatus.Blocked)
                throw new CoreException(ErrorCode.AccountBlocked, $"Account for customer {customer.Username} is blocked.");

            if (customer.Status == AccountStatus.NotActive)
                throw new CoreException(ErrorCode.AccountNotActive, $"Account for customer {customer.Username} is not active.");
        }

        public bool CheckIfExist([NotNull] ApplicationDbContext context, [NotNull] string username)
        {
            var customer = context.Customers.SingleOrDefault(x => x.Username == username);

            if (customer is null)
                return false;

            return true;
        }
    }
}
