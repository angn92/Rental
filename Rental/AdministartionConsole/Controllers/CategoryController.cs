using Microsoft.AspNetCore.Mvc;
using Rental.Infrastructure.EF;

namespace AdministartionConsole.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ApplicationDbContext _context;

        public CategoryController(ILogger<CategoryController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories;

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
