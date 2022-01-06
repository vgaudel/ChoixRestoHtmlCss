using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoixResto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChoixResto.Controllers
{
    [Authorize]
    public class RestaurantController : Controller
    {
        private IDal dal;

        public RestaurantController()
        {
            this.dal = new Dal();
        }

        public ActionResult Index()
        {
            List<Resto> listeDesRestaurants = dal.ObtientTousLesRestaurants();
            return View(listeDesRestaurants);
        }

        public ActionResult CreerRestaurant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreerRestaurant(Resto resto)
        {
            if (dal.RestaurantExiste(resto.Nom))
            {
                ModelState.AddModelError("Nom", "Ce nom de restaurant existe déjà");
                return View(resto);
            }
            if (!ModelState.IsValid)
                return View(resto);
            dal.CreerRestaurant(resto.Nom, resto.Telephone, resto.Ville, resto.Description);
            return RedirectToAction("Index");
        }

        public ActionResult ModifierRestaurant(int? id)
        {
            if (id.HasValue)
            {
                Resto restaurant = dal.ObtientTousLesRestaurants().FirstOrDefault(r => r.Id == id.Value);
                if (restaurant == null)
                    return View("Error");
                return View(restaurant);
            }
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult ModifierRestaurant(Resto resto)
        {
            if (!ModelState.IsValid)
                return View(resto);
            dal.UpdateRestaurant(resto.Id, resto.Nom, resto.Telephone, resto.Ville, resto.Description);
            return RedirectToAction("Index");
        }

        public ActionResult SupprimerRestaurant(int id)
        {
            dal.SupprimerRestaurant(id);
            return RedirectToAction("Index");
        }
    }
}