using AdministartionConsole.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;

namespace AdministartionConsole.Helpers
{
    public interface IProductDtoHelper
    {
        Task<List<ProductViewModel>> GetAll();
    }

    public class ProductDtoHelper : IProductDtoHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductDtoHelper> _logger;

        public ProductDtoHelper(ApplicationDbContext context, ILogger<ProductDtoHelper> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            var productVm = await _context.Products
                .Join(_context.Customers, p => p.Customer.CustomerId, c => c.CustomerId, (p, c) => new { p, c })
                .Join(_context.Categories, cat => cat.p.Category.CategoryId, pc => pc.CategoryId, (cat, pc) => new { cat.p, cat.c, pc })
                .Select(pcc => new ProductViewModel
                {
                    ProductId = pcc.p.ProductId,
                    Name = pcc.p.Name,
                    Amount = pcc.p.Amount,
                    AvailableAmount = pcc.p.QuantityAvailable,
                    Customer = pcc.c.Username,
                    Category = pcc.pc.Name,
                    Status = pcc.p.Status.ToString()
                }).ToListAsync();

            if (productVm == null)
                throw new CoreException(ErrorCode.NoProducts, $"No product found.");
            
                

            // Alternative way - query syntax

            //var productVm = from product in _context.Products
            //                 join customer in _context.Customers on product.Customer.CustomerId equals customer.CustomerId
            //                 join category in _context.Categories on product.Category.CategoryId equals category.CategoryId
            //                 select new ProductViewModel
            //                 {
            //                     ProductId = product.ProductId,
            //                     Name = product.Name,
            //                     Amount = product.Amount,
            //                     AvailableAmount = product.QuantityAvailable,
            //                     Customer = customer.Username,
            //                     Category = category.Name,
            //                     Status = product.Status.ToString()
            //                 };

            return productVm;
        }
    }
}
