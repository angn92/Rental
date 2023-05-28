using AdministartionConsole.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AdministartionConsole.Controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductDtoHelper _productDtoHelper;

        public ProductController(ILogger<ProductController> logger, IProductDtoHelper productDtoHelper)
        {
            _logger = logger;
            _productDtoHelper = productDtoHelper;
        }

        [HttpGet("startPage")]
        public async Task<IActionResult> StartPage(string searching)
        {
            try
            {
                var productList = await _productDtoHelper.GetAll();

                if (!String.IsNullOrWhiteSpace(searching))
                {
                    productList = productList.Where(x => x.Name == searching).ToList();
                }
                
                if (productList.Count == 0)
                {
                    ViewBag.SearchingParameter = searching;
                    return View("NotFound");
                }

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
