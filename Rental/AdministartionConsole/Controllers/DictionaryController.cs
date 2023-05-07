using AdministartionConsole.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AdministartionConsole.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly IDictionaryDtoHelper _dictionaryDtoHelper;

        public DictionaryController(IDictionaryDtoHelper dictionaryDtoHelper)
        {
            _dictionaryDtoHelper = dictionaryDtoHelper;
        }

        // Get
        public async Task<IActionResult> Index()
        {
            var dictionary = await _dictionaryDtoHelper.GetAll();

            return View(dictionary);
        }

        // POST /Dictionary/Create
        public string Create()
        {
            return "test";
        }
    }
}
