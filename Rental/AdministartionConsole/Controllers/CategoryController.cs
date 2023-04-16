using Microsoft.AspNetCore.Mvc;

namespace AdministartionConsole.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Get()
        {
            return View();
        }
    }
}
