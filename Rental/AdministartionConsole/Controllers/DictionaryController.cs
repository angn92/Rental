using AdministartionConsole.Helpers;
using AdministartionConsole.Models.Dto;
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DictionaryDto dictionaryDto)
        {
            _dictionaryDtoHelper.CreateDictionary(dictionaryDto);
            return View();
        }
    }
}
