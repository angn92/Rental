using JetBrains.Annotations;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Services;
using System;

using System.Threading.Tasks;

namespace Rental.Infrastructure.Helpers
{
    public interface IUserHelper : IService
    {
        string CheckAccountStatus(Customer user);
        Task ChangeAccountStatus([NotNull] ApplicationDbContext context, [NotNull] Customer customer, AccountStatus status); 
    }

    public class UserHelper : IUserHelper
    {
        //Customer can change status only on NotActive
        public async Task ChangeAccountStatus([NotNull] ApplicationDbContext context, [NotNull] Customer customer, AccountStatus status)
        {
            if (customer.Status == status)
                return;

            customer.Status = status;
            await context.SaveChangesAsync();
        }

        public string CheckAccountStatus(Customer customer)
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
    }
}
