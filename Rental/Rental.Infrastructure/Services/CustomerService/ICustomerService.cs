using Rental.Core.Domain;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.CustomerService
{
    public interface ICustomerService : IService
    {
        Task RegisterAsync(string firstName, string lastName, string username, string email, string phoneNumber);
        Task<Customer> LoginAsync(string username, string password);
        Task<Customer> GetCustomerAsync(string username);
        Task<bool> CheckIfExist(string username);
        Task ValidateCustomerAccountAsync(Customer customer);
    }
}
