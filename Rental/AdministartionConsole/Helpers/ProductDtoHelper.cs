using AdministartionConsole.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Rental.Infrastructure.EF;

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
            try
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

                if (productVm.Count == 0)
                    _logger.LogInformation("There is no any product.");

                return productVm;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
