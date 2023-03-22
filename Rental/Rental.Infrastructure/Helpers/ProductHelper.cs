using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Helpers
{
    public interface IProductHelper
    {
        Task AddProductAsync([NotNull] string name, [NotNull] int quantity, [NotNull] Customer customer,
            [NotNull] Category category, [CanBeNull] string description);

        Task CheckIfGivenProductExistAsync([NotNull] string name, [NotNull] Customer customer);

        void MakeReservationProduct([NotNull] Product product);

        Task<Product> GetProductAsync([NotNull] string productId);
    }

    public class ProductHelper : IProductHelper
    {
        private readonly ILogger<ProductHelper> _logger;
        private readonly ApplicationDbContext _context;

        public ProductHelper(ILogger<ProductHelper> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task AddProductAsync([NotNull] string name, [NotNull] int quantity, [NotNull] Customer customer,
            [NotNull] Category category, [CanBeNull] string description)
        {
            var product = new Product(name, quantity, category, customer, description);

            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task CheckIfGivenProductExistAsync([NotNull] string name, [NotNull] Customer customer)
        {
            var productName = await _context.Products.SingleOrDefaultAsync(x => x.Customer.Username == customer.Username &&
                                       x.Status != ProductStatus.Inaccessible);

            if (productName != null)
                throw new CoreException(ErrorCode.ProductExist, $"This product {name} exist in your board.");
        }

        public void MakeReservationProduct([NotNull] Product product)
        {
            product.SetReservedStatus();
            product.QuantityAvailable -= 1;
        }


        public async Task<Product> GetProductAsync([NotNull] string productId)
        {
            var product = await _context.Products
                        .Include(z => z.Category)
                        .Include(q => q.Customer)
                        .SingleOrDefaultAsync(x => x.ProductId == productId);

            if (product == null)
                throw new CoreException(ErrorCode.ProductNotExist, $"Product with id {productId} does not exist.");

            return product;
        }
    }
}
