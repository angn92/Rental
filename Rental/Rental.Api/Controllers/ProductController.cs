using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Handlers.Product.Command.NewProduct;
using Rental.Infrastructure.Handlers.Product.Query.ProductDetails;
using Rental.Infrastructure.Query;
using System.Threading.Tasks;

namespace Rental.Api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ICommandDispatcher commandDispacher;
        private readonly IQueryDispatcher queryDispatcher;

        public ProductController(ICommandDispatcher commandDispacher, IQueryDispatcher queryDispatcher)
        {
            this.commandDispacher = commandDispacher;
            this.queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Add new product for customer to rental.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("product")]
        public async Task AddProductAsync([FromBody] [NotNull] ProductRequest request)
        {
            var command = new ProductRequest
            {
                Name = request.Name,
                Amount = request.Amount,
                CategoryName = request.CategoryName,
                Description = request.Description,
                Username = request.Username
            };

            await commandDispacher.DispatchAsync(new ProductCommand(command));
        }

        /// <summary>
        /// Get product details.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("product/{id}")]
        public async Task<ProductDetailsResponse> GetProductDetailsAsync([FromRoute] [NotNull] string id)
        {
            var request = new ProductDetailRequest
            {
                ProductId = id
            };

            return await queryDispatcher.DispatchAsync<ProductDetailRequest, ProductDetailsResponse>(request);
        }
    }
}
