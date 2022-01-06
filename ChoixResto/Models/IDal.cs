using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoixResto.Models
{
	public interface IDal : IDisposable
	{
		void CreerRestaurant(string nom, string telephone, string ville, string description, int id = 0);
		void UpdateRestaurant(int id, string nom, string telephone, string ville, string description);
		void SupprimerRestaurant(int id);
		void SupprimerRestaurant(string nom, string telephone);
		bool RestaurantExiste(string nom);
		List<Resto> ObtientTousLesRestaurants();
		// ajout fonctions à venir

		Utilisateur AjouterUtilisateur(string nom, string password, string role="Basic");
		Utilisateur Authentifier(string nom, string password);
		Utilisateur ObtenirUtilisateur(int id);
		Utilisateur ObtenirUtilisateur(string idStr);

		int CreerUnSondage();
		void AjouterVote(int idSondage, int idResto, int idUtilisateur);
		List<Resultats> ObtenirLesResultats(int idSondage);
		List<Sondage> ObtenirLesSondages();
		bool ADejaVote(int idSondage, string idStr);
	}
}
