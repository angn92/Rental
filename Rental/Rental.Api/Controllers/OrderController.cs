using Microsoft.AspNetCore.Mvc;

namespace Rental.Api.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
