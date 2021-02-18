using Rental.Core.Domain;
using System;
using System.Threading.Tasks;

namespace Rental.Core.Repository
{
    public interface IUserRepository : IRepository
    {
        Task<Account> GetAsync(string username);
        Task AddAsync(Account user);
        Task EditAsync(Account user);
    }
}
