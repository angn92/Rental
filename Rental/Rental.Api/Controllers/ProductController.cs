using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Handlers.Product.Command;
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
        /// Add new product to rental.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/new/product")]
        public async Task AddProductAsync([FromBody] [NotNull] ProductRequest request, [CanBeNull] CancellationToken cancellationToken = default)
        {
            var command = new ProductRequest
            {
                Name = request.Name,
                Amount = request.Amount,
                CategoryName = request.CategoryName,
                Description = request.Description
            };

            await commandDispacher.DispatchAsync(new AddProductCommand(command));
        }

        //public async Task<ProductDetailsResponse> GetProductDetailsAsync([FromBody][NotNull] ProductDetailRequest request, [CanBeNull] CancellationToken = default)
        //{

        //}
    }
}
