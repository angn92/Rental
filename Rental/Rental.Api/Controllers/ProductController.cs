using Microsoft.AspNetCore.Mvc;
using Rental.Infrastructure.Command;
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

        [HttpPost("product")]
        public async Task AddProduct()
        {

        }
    }
}
