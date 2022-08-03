using JetBrains.Annotations;
using Rental.Core.Domain;
using Rental.Infrastructure.EF;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Helpers
{
    public interface IProductHelper
    {
        Task AddProductAsync([NotNull] ApplicationDbContext context, [NotNull] string name, [NotNull] int quantity, [NotNull] Customer customer, 
            [CanBeNull] string description, [CanBeNull] Category category);
        Task<StatusProduct> GetStatusProductAsync([NotNull] string name);
        Task ChangeProductStatusAsync([NotNull] string name);
    }

    public class ProductHelper : IProductHelper
    {
        public async Task AddProductAsync([NotNull] ApplicationDbContext context, [NotNull] string name, [NotNull] int quantity, [NotNull] Customer customer,
            [CanBeNull] string description, [CanBeNull] Category category)
        {
            var product = new Product(name, quantity, category, customer);

            await context.AddAsync(product);
        }

        public async Task ChangeProductStatusAsync([NotNull] string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<StatusProduct> GetStatusProductAsync([NotNull] string name)
        {
            throw new System.NotImplementedException();
        }
    }
}
