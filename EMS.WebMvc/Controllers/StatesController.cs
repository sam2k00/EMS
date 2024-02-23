using EMS.WebMvc.Helper;
using EMS.WebMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;

namespace EMS.WebMvc.Controllers
{
    public class StatesController : Controller
    {

        private readonly ILogger<PartiesController> _logger;
        private readonly IConfiguration _configuration;
        public StatesController(ILogger<PartiesController> logger, IConfiguration configuration) {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
            var Parties = elactionCommissionHelper.GetStates();
            return View(Parties);

            //return View();
        }

        public IActionResult CreateNewState()
        {
            return View();
        }

        public IActionResult EditState(int id)
        {

            ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
            
            var state = elactionCommissionHelper.GetState(id);
            return View("CreateNewState", state);            
        }

        [HttpPost]
        public async Task<IActionResult> SaveNewState(States states)
        {
            try
            {
                ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
                elactionCommissionHelper.SaveState(states);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index");
        }
    }
}
