using Rental.Core.Domain;
using System;
using System.Threading.Tasks;

namespace Rental.Core.Repository
{
    public interface IUserRepository : IRepository
    {
        Task<Customer> GetAsync(string username);
        Task AddAsync(Customer user);
        Task EditAsync(Customer user);
    }
}
