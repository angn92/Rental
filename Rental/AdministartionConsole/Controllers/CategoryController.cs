using AdministartionConsole.Helpers;
using Microsoft.AspNetCore.Mvc;
using Rental.Infrastructure.Helpers;

namespace AdministartionConsole.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryHelper _categoryHelper;
        private readonly ICategoryDtoHelper _categoryDtoHelper;

        public CategoryController(ILogger<CategoryController> logger, ICategoryHelper categoryHelper, ICategoryDtoHelper categoryDtoHelper)
        {
            _logger = logger;
            _categoryHelper = categoryHelper;
            _categoryDtoHelper = categoryDtoHelper;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var category = await _categoryHelper.GetAllCategories();

                _logger.LogInformation($"Was found {category.Count} categories.");

                var categoryList = _categoryDtoHelper.PrepareResult(category);

                return View(categoryList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
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
