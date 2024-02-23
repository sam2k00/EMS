using EMS.WebMvc.Helper;
using EMS.WebMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebMvc.Controllers
{
    public class SymbolsController : Controller
    {
        private readonly ILogger<SymbolsController> _logger;
        private readonly IConfiguration _configuration;

        public SymbolsController(ILogger<SymbolsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
            var symbols = elactionCommissionHelper.LoadSymbols();
            return View(symbols);
            
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit(int id)
        {
            ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
            //ViewBag.PartiesSymbol = elactionCommissionHelper.GetSymbols();

            var symbol = elactionCommissionHelper.GetSymbol(id);
            return View("Create", symbol);
        }
        

        [HttpPost]
        public async Task<IActionResult> SaveSymbol(Symbol symbol)
        {
            try
            {
                ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
                elactionCommissionHelper.SaveSymbol(symbol);
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("Index");
        }
    }
}
