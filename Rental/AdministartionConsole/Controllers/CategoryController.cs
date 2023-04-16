using Microsoft.AspNetCore.Mvc;

namespace AdministartionConsole.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
