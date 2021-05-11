using Rental.Core.Domain;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.UserService
{
    public interface IUserService : IService
    {
        Task RegisterAsync(string firstName, string lastName, string username, string email, string phoneNumber, string password);
        Task<User> LoginAsync(string username, string password);
        Task<User> GetUserAsync(string nick);
    }
}
