using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Handlers.Product.Command.NewProduct;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ICommandDispatcher commandDispacher;

        public ProductController(ICommandDispatcher commandDispacher)
        {
            this.commandDispacher = commandDispacher;
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

        //public async Task<ProductDetailsResponse> GetProductDetailsAsync([FromBody][NotNull] ProductDetailRequest request, [CanBeNull] CancellationToken = default)
        //{

        //}
    }
}
