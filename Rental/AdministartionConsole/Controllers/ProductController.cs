using Microsoft.AspNetCore.Mvc;

namespace AdministartionConsole.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Get()
        {
            return View();
        }
    }
}
