﻿using AdministartionConsole.Helpers;
using Microsoft.AspNetCore.Mvc;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;

namespace AdministartionConsole.Controllers
{
    [Route("[controller]")]
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

        [HttpGet("startPage")]
        public async Task<IActionResult> StartPage()
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

        [HttpGet("create/new")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name)
        {
            try
            {
                var category = await _categoryHelper.GetCategory(name);

                if (category != null)
                    throw new CoreException(ErrorCode.CategoryExist, $"You can not create category {name}, because exist in system.");

                await _categoryHelper.Create(name);

                return View("CreatedCategory");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("ErrorCreate");
            }
        }
    }
}
