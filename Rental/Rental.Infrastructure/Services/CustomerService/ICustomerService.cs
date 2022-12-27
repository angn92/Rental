using JetBrains.Annotations;
using Rental.Core.Domain;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.CustomerService
{
    public interface ICustomerService : IService
    {
        /// <summary>
        /// Create new customer account
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<Customer> RegisterAsync([NotNull] string firstName, [NotNull] string lastName, [NotNull] string username, 
            [NotNull] string email);

        /// <summary>
        /// Login to own account
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<Customer> LoginAsync([NotNull] string username, [NotNull] string password);

        Task<Customer> GetCustomerAsync([NotNull] string username);

        Task<bool> CheckIfExist([NotNull] string username);
    }
}
