using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Helpers
{
    public interface ICategoryHelper : IService
    {
        Task<Category> GetCategory([NotNull] string name);
        Task<List<Category>> GetAllCategories();
    }

    public class CategoryHelper : ICategoryHelper
    {
        private readonly ApplicationDbContext _context;

        public CategoryHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();

            //if (!categories.Any())
            //    throw new CoreException(ErrorCode.ListOfCategoriesIsEmpty, "There is no any categories in system.");

            return categories;
        }

        public async Task<Category> GetCategory([NotNull] string name)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(x => x.Name == name);

            if (category == null)
                throw new CoreException(ErrorCode.CategoryNotExist, $"Category {name} does not exist.");

            return category;
        }
    }
}
