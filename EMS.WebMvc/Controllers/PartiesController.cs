using EMS.WebMvc.Helper;
using EMS.WebMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebMvc.Controllers
{
    public class PartiesController : Controller
    {
        private readonly ILogger<PartiesController> _logger;
        private readonly IConfiguration _configuration;

        public PartiesController(ILogger<PartiesController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
            var Parties = elactionCommissionHelper.Loadparties();
            return View(Parties);
        }

        public IActionResult CreateParty(int id)
        {
            ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
            ViewBag.PartiesSymbol=elactionCommissionHelper.GetSymbols();

            //ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
            var Party = elactionCommissionHelper.LoadpartyDetails(id);
            return View(Party);

        }

        [HttpPost]
        public async Task<IActionResult> SaveParty(Partie partie)
        {
            try
            {
                ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
                elactionCommissionHelper.SaveParty(partie);



            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index");
        }
    }
}
