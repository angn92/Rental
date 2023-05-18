using AdministartionConsole.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AdministartionConsole.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductDtoHelper _productDtoHelper;

        public ProductController(ILogger<ProductController> logger, IProductDtoHelper productDtoHelper)
        {
            _logger = logger;
            _productDtoHelper = productDtoHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searching)
        {
            try
            {
                var productList = await _productDtoHelper.GetAll();

                if (!String.IsNullOrWhiteSpace(searching))
                    productList = productList.Where(x => x.Name == searching).ToList();

                return View(productList);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }
    }
}
