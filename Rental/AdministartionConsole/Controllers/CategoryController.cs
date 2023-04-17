using AdministartionConsole.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Rental.Infrastructure.Helpers;

namespace AdministartionConsole.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryHelper _categoryHelper;

        public CategoryController(ILogger<CategoryController> logger, ICategoryHelper categoryHelper)
        {
            _logger = logger;
            _categoryHelper = categoryHelper;
        }

        public async Task<IActionResult> Index()
        {
            var category = await _categoryHelper.GetAllCategories();

            if (category.Count == 0)
                return NotFound();

            var catDto = new CategoryDto();
            catDto.Name = category.First().Name;
            
            
            return View(catDto);
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
