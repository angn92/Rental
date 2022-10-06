using JetBrains.Annotations;
using Rental.Core.Domain;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.CustomerService
{
    public interface ICustomerService : IService
    {
        Task<Customer> RegisterAsync([NotNull] string firstName, [NotNull] string lastName, [NotNull] string username, 
            [NotNull] string email, [CanBeNull] string phoneNumber);

        Task<Customer> LoginAsync([NotNull] string username, [NotNull] string password);

        Task<Customer> GetCustomerAsync([NotNull] string username);

        Task<bool> CheckIfExist([NotNull] string username);

        void ValidateCustomerAccount([NotNull] Customer customer);
    }
}
