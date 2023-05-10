using AdministartionConsole.Helpers;
using AdministartionConsole.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AdministartionConsole.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly IDictionaryDtoHelper _dictionaryDtoHelper;
        private readonly ILogger<DictionaryController> _logger;

        public DictionaryController(IDictionaryDtoHelper dictionaryDtoHelper, ILogger<DictionaryController> logger)
        {
            _dictionaryDtoHelper = dictionaryDtoHelper;
            _logger = logger;
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
            try
            {
                _dictionaryDtoHelper.CreateDictionary(dictionaryDto);

                return View("Ok", dictionaryDto);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string dictionaryName)
        {
            try
            {
                if (dictionaryName == null)
                    return NotFound();

                var dictionaryData = await _dictionaryDtoHelper.FindByName(dictionaryName);

                return View("Edit", dictionaryData);
            }
            catch (Exception ex)
            {
                _logger.LogError("Searching dictionary not found", ex.Message);
                throw;
            }
        }
    }
}
