using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using ChoixResto.Models;
using ChoixResto.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChoixResto.Controllers
{
	public class VoteController : Controller
	{
        private IDal dal;
        public VoteController()
        {
            dal = new Dal();
        }
        public ActionResult Index(int id)
        {
            RestaurantVoteViewModel viewModel = new RestaurantVoteViewModel
            {
                ListeDesResto = dal.ObtientTousLesRestaurants().Select(r => new RestaurantCheckBoxViewModel { Id = r.Id, NomEtTelephone = string.Format("{0} ({1})", r.Nom, r.Telephone) }).ToList()
            };
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (dal.ADejaVote(id, userId))
            {
                return RedirectToAction("AfficheResultat", new { id = id });
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(RestaurantVoteViewModel viewModel, int id)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            Utilisateur utilisateur = dal.ObtenirUtilisateur(userId);
            if (utilisateur == null)
                return new NotFoundResult();
            foreach (RestaurantCheckBoxViewModel restaurantCheckBoxViewModel in viewModel.ListeDesResto.Where(r => r.EstSelectionne))
            {
                dal.AjouterVote(id, restaurantCheckBoxViewModel.Id, utilisateur.Id);
            }
            return RedirectToAction("AfficheResultat", new { id = id });
        }

        public ActionResult AfficheResultat(int id)
        {
           var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!dal.ADejaVote(id, userId))
            {
                return RedirectToAction("Index", new { id = id });
            }
            List<Resultats> resultats = dal.ObtenirLesResultats(id);
            return View(resultats.OrderByDescending(r => r.NombreDeVotes).ToList());
        }

        public ActionResult CreateSondage()
        {
            int idSondage = dal.CreerUnSondage();
            return RedirectToAction("Index", "Vote", new { id = idSondage });
        }

        public ActionResult ListeSondages()
        {
            List<Sondage> listSondages = dal.ObtenirLesSondages();
            return View(listSondages);
        }
    }
}
