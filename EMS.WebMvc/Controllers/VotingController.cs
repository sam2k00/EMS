using Microsoft.AspNetCore.Mvc;

using EMS.WebMvc.Helper;
using EMS.WebMvc.Models;


namespace EMS.WebMvc.Controllers
{
    public class VotingController : Controller
    {

        private readonly ILogger<VotingController> _logger;
        private readonly IConfiguration _configuration;
        public VotingController(ILogger<VotingController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            // Retrieve success message from TempData, if any
            string successMessage = TempData["SuccessMessage"] as string;

            // Pass the success message to the view using ViewBag or a model property
            ViewBag.SuccessMessage = successMessage;

            VoteOnlineLogin voteOnlineLogin=new VoteOnlineLogin { Id=0,Password="",UserName=""};

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Loginrvoter(Voter voter)
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

        
        public IActionResult Vote()
        {
            return View();
        }

        public IActionResult LoginVoteOnline(int id)
        {
            return RedirectToAction("VoteVoteOnline");
        }

        [HttpPost]
        public async Task<IActionResult> VoteOnline(VoteOnlineLogin voteOnlineLogin)
        {
            if (!ModelState.IsValid)
            {
                // If ModelState is not valid, there are validation errors
                // You can inspect each error and handle them accordingly
                foreach (var entry in ModelState.Values)
                {
                    foreach (var error in entry.Errors)
                    {
                        var errorMessage = error.ErrorMessage;
                        // Do something with the error message
                    }
                }

                // Return the view with the model to display validation errors
                return View("Index", voteOnlineLogin);
            }
                        

            ElactionCommissionHelper elactionCommissionHelper = new ElactionCommissionHelper(_configuration);

            var voter = elactionCommissionHelper.GetVoter(voteOnlineLogin.Id);

            var candidates = elactionCommissionHelper.LoadStateCandidates(voter.StateId);
            
            foreach (Candidate candidate in candidates)
            {                
                candidate.Partie = elactionCommissionHelper.LoadpartyDetails(candidate.PartyId);

                candidate.Partie.PartiSymbol = elactionCommissionHelper.GetSymbol(candidate.Partie.SymbolId);

            }

            ViewBag.VoterId = voteOnlineLogin.Id;
            return View(candidates);           

        }

        public IActionResult CandiateVote(CandidateVote candidateVote)
        {
            // Assuming the vote was successfully submitted
            TempData["SuccessMessage"] = "Your vote has been submitted successfully! Thank you for participating.";

            return RedirectToAction("Index");
        }


    }


}
