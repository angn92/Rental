using AdministartionConsole.Helpers;
using AdministartionConsole.Models.Dto;
using JetBrains.Annotations;
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

        /// <summary>
        /// https://localhost:port/Dictionary/Index
        /// </summary>
        /// <returns></returns>
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
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// https://localhost:port/Dictionary/Edit?dictionaryName=paramName
        /// </summary>
        /// <param name="dictionaryName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit([NotNull] string dictionaryName)
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

        [HttpPost]
        public async Task<IActionResult> Edit([NotNull] DictionaryDto dictionary)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dictionaryData = await _dictionaryDtoHelper.ChangeDictionary(dictionary);
                    return View("DictionarySummary", dictionaryData);
                }

                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
