using ChoixResto.Models;
using ChoixResto.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace ChoixResto.Controllers
{
    public class LoginController : Controller
    {
        private IDal dal;

        public LoginController()
        {
            this.dal = new Dal();
        }

       

        public ActionResult Index()
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = HttpContext.User.Identity.IsAuthenticated };
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                viewModel.Utilisateur = dal.ObtenirUtilisateur(userId);
                return Redirect("/home/index");
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(UtilisateurViewModel viewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Utilisateur utilisateur = dal.Authentifier(viewModel.Utilisateur.Prenom, viewModel.Utilisateur.Password);
                if (utilisateur != null)
                {
					var userClaims = new List<Claim>()
				    {
                        new Claim(ClaimTypes.Name, utilisateur.Prenom),
                        new Claim(ClaimTypes.NameIdentifier, utilisateur.Id.ToString()),
                        new Claim(ClaimTypes.Role, utilisateur.Role),
                    };

					var ClaimIdentity = new ClaimsIdentity(userClaims, "User Identity");

					var userPrincipal = new ClaimsPrincipal(new[] { ClaimIdentity });
					HttpContext.SignInAsync(userPrincipal);



                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return Redirect("/");
                }
                ModelState.AddModelError("Utilisateur.Prenom", "Prénom et/ou mot de passe incorrect(s)");
            }
            return View(viewModel);
        }

        public ActionResult CreerCompte()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreerCompte(Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                Utilisateur userCreated = dal.AjouterUtilisateur(utilisateur.Prenom, utilisateur.Password);
                
                var userClaims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, userCreated.Prenom),
                        new Claim(ClaimTypes.NameIdentifier, userCreated.Id.ToString()),
                        new Claim(ClaimTypes.Role, userCreated.Role)
                    };

                var ClaimIdentity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { ClaimIdentity });
                HttpContext.SignInAsync(userPrincipal);

                return Redirect("/");
            }
            return View(utilisateur);
        }

        public ActionResult Deconnexion()
        {
            HttpContext.SignOutAsync();
            return Redirect("/");
        }

    }
}