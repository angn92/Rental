using AdministartionConsole.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AdministartionConsole.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerDtoHelper _customerDtoHelper;

        public CustomerController(ICustomerDtoHelper customerDtoHelper)
        {
            _customerDtoHelper = customerDtoHelper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var customerModel = await _customerDtoHelper.GetCustomerViews();

            return View(customerModel);
        }
    }
}
