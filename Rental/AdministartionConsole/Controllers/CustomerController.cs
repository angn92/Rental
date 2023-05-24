using AdministartionConsole.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AdministartionConsole.Controllers
{
    [Route("controller")]
    public class CustomerController : Controller
    {
        private readonly ICustomerDtoHelper _customerDtoHelper;

        public CustomerController(ICustomerDtoHelper customerDtoHelper)
        {
            _customerDtoHelper = customerDtoHelper;
        }

        [HttpGet("customers/list")]
        public async Task<IActionResult> GetCustomer()
        {
            var customerModel = await _customerDtoHelper.GetCustomerViews();

            return View(customerModel);
        }
    }
}
