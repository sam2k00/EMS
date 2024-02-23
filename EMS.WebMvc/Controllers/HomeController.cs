using EMS.WebMvc.Helper;
using EMS.WebMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EMS.WebMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly IConfiguration _configuration;

		public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
			_configuration = configuration;
		}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

      /*
        public IActionResult Candidates()
        {            
            Candidate candidate = new Candidate();
            candidate.CandidateId = 123;
            candidate.Name = "Nag";

            
            ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
            var candidates=  elactionCommissionHelper.Loadcandidate();
            
            return View(candidates);
        }

        public IActionResult States()
        {

            ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
            var states = elactionCommissionHelper.Loadstates();


            return View(states);
        }

		public IActionResult Parties()
		{

			ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
			var Parties = elactionCommissionHelper.Loadparties();


			return View(Parties);
		}

		public IActionResult Voters()
		{

			ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
			var Voters = elactionCommissionHelper.Loadvoters();


			return View(Voters);
		}
        */
        public IActionResult PartiesCandidates()
        {
            Candidate candidate = new Candidate();
            candidate.CandidateId = 123;
            candidate.Name = "Nag";


            ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
            var candidates = elactionCommissionHelper.Loadcandidate();

            return View(candidates);
        }

        public IActionResult CreatePartiesCandidate(int id) {

            ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
            
            ViewBag.States = elactionCommissionHelper.Loadstates();
            ViewBag.Parties=elactionCommissionHelper.Loadparties();

            var candidate= elactionCommissionHelper.Loadcandidatedetails(id);

            return View(candidate);
        }

        [HttpPost]
        public async Task<IActionResult> Savecandidate(Candidate candidate)
        {
            try
            {
                ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);
                elactionCommissionHelper.SaveCandidate(candidate);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("PartiesCandidates");
        }

        
    }
}
