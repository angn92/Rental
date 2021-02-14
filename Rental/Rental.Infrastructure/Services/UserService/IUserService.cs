using Rental.Infrastructure.DTO;
using System;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.UserService
{
    public interface IUserService : IService
    {
        Task RegisterAsync(string firstName, string lastName, string username, string email, string phoneNumber);
        Task<UserDto> LoginAsync(string username, string password);
    }
}
