using Rental.Core.Domain;
using System;
using System.Threading.Tasks;

namespace Rental.Core.Repository
{
    public interface IUserRepository : IRepository
    {
        Task<User> GetAsync(string username);
        Task AddAsync(User user);
        Task EditAsync(User user);
    }
}
