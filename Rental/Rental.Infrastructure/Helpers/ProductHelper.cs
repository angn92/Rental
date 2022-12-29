using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Helpers
{
    public interface IProductHelper
    {
        Task AddProductAsync([NotNull] ApplicationDbContext context, [NotNull] string name, [NotNull] int quantity, [NotNull] Customer customer,
            [NotNull] Category category, [CanBeNull] string description);

        Task CheckIfGivenProductExistAsync([NotNull] ApplicationDbContext context, [NotNull] string name, [NotNull] Customer customer);

        void MakeReservationProduct([NotNull] Product product);

        Task<Product> GetProductAsync([NotNull] ApplicationDbContext context, [NotNull] string productId);
    }

    public class ProductHelper : IProductHelper
    {
        public async Task AddProductAsync([NotNull] ApplicationDbContext context, [NotNull] string name, [NotNull] int quantity, [NotNull] Customer customer,
            [NotNull] Category category, [CanBeNull] string description)
        {
            var product = new Product(name, quantity, category, customer, description);

            await context.AddAsync(product);
            await context.SaveChangesAsync();
        }

        public async Task CheckIfGivenProductExistAsync([NotNull] ApplicationDbContext context, [NotNull] string name, [NotNull] Customer customer)
        {
            var productName = await context.Products.SingleOrDefaultAsync(x => x.Customer.Username == customer.Username &&
                                       x.Status != ProductStatus.Inaccessible);

            if (productName != null)
                throw new CoreException(ErrorCode.ProductExist, $"This product {name} exist in your board.");
        }

        public void MakeReservationProduct([NotNull] Product product)
        {
            product.SetReservedStatus();
        }


        public async Task<Product> GetProductAsync([NotNull] ApplicationDbContext context, [NotNull] string productId)
        {
            var product = await context.Products.SingleOrDefaultAsync(x => x.ProductId == productId);

            if (product == null)
                throw new CoreException(ErrorCode.ProductNotExist, $"Product with id {productId} does not exist.");

            return product;
        }
    }
}
