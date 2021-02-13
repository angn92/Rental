using Rental.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rental.Core.Repository
{
    public interface ICategoryRepository : IRepository
    {
        Task<Category> GetAsync(string name);
        Task<IEnumerable<Category>> GetAllAsync();
        Task AddAsync(Category category);
        Task EditAsync(Category category);
        Task RemoveAsync(Guid id);
    }
}
