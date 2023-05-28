using AdministartionConsole.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AdministartionConsole.Controllers
{
    [Route("controller")]
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerDtoHelper _customerDtoHelper;

        public CustomerController(ILogger<CustomerController> logger, ICustomerDtoHelper customerDtoHelper)
        {
            _logger = logger;
            _customerDtoHelper = customerDtoHelper;
        }

        [HttpGet("customers/list")]
        public async Task<IActionResult> GetCustomer()
        {
            try
            {
                var customerModel = await _customerDtoHelper.GetCustomerViews();
                
                

                return View(customerModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
