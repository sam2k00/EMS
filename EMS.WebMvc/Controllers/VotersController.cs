using EMS.WebMvc.Helper;
using EMS.WebMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebMvc.Controllers
{
    public class VotersController : Controller
    {

        private readonly ILogger<PartiesController> _logger;
        private readonly IConfiguration _configuration;
        public VotersController(ILogger<PartiesController> logger, IConfiguration configuration) {
            _logger = logger;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
            var Voters = elactionCommissionHelper.Loadvoters();
            return View(Voters);

        }
        public IActionResult Create()
        {
            ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
            ViewBag.States = elactionCommissionHelper.GetStates();
            return View();
        }

        public IActionResult Edit(int id)
        {
            ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
            ViewBag.States = elactionCommissionHelper.GetStates();

            var symbol = elactionCommissionHelper.GetVoter(id);
            return View("Create", symbol);
        }

        [HttpPost]
        public async Task<IActionResult> SaveVoter(Voter voter)
        {
            try
            {
                ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
                elactionCommissionHelper.SaveVoter(voter);
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("Index");
        }
    }
}
