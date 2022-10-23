using JetBrains.Annotations;
using Rental.Core.Domain;
using Rental.Infrastructure.EF;
using System.Collections.Generic;

namespace Rental.Test.Helpers
{
    public static class CategoryTestHelper
    {
        public static Category CreateNewCategory([NotNull] ApplicationDbContext _context, string categoryName)
        {
            var category = new Category(categoryName);

            _context.Add(category);
            _context.SaveChanges();

            return category;
        }

        public static List<Category> CreateManyCategories([NotNull] ApplicationDbContext _context, List<string> categoryName)
        {
            var categoryList = new List<Category>();


            foreach (var item in categoryName)
            {
                var category = new Category(item);
                _context.Add(category);

                categoryList.Add(category);
            }

            _context.SaveChanges();

            return categoryList;
        }
    }
}
