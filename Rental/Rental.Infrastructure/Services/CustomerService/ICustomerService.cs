using JetBrains.Annotations;
using Rental.Core.Domain;
using Rental.Core.Enum;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.CustomerService
{
    public interface ICustomerService : IService
    {
        Task<Customer> RegisterAsync([NotNull] string firstName, [NotNull] string lastName, [NotNull] string username, [NotNull] string email);

        void ChangeAccountStatus([NotNull] Customer customer, AccountStatus status);
    }
}
