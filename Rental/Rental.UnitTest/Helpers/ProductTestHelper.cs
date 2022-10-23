using JetBrains.Annotations;
using Rental.Core.Domain;
using Rental.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental.Test.Helpers
{
    public static class ProductTestHelper
    {
        public static Product AddProduct(ApplicationDbContext context, string name, int amount, Category category, Customer customer,
            string description = null, Action<Product> action = null)
        {
            var product = new Product(name, amount, category, customer, description);
            context.Add(product);

            action?.Invoke(product);

            context.SaveChanges();

            return product;
        }
    }
}
