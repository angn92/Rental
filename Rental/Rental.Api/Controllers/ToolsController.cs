using Microsoft.AspNetCore.Mvc;
using Rental.Infrastructure.Query;

namespace Rental.Api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ToolsController : Controller
    {
        private readonly IQueryDispatcher queryDispatcher;

        public ToolsController(IQueryDispatcher queryDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
        }

        public 
    }
}
