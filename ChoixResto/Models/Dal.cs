using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChoixResto.Models
{
	public class Dal : IDal
	{
		private BddContext _bddContext;

		public Dal()
		{
			this._bddContext = new BddContext();
		}

		public void Dispose()
		{
			this._bddContext.Dispose();
		}
		public void CreerRestaurant(string nom, string telephone, string ville, string description, int id=0)
		{

			Resto restoToAdd = new Resto { Nom = nom, Telephone = telephone, Ville = ville, Description = description };
			if(id != 0)
			{
				restoToAdd.Id = id;
			}

			this._bddContext.Restos.Add(restoToAdd);
			this._bddContext.SaveChanges();
		}


		public List<Resto> ObtientTousLesRestaurants()
		{
			List<Resto> listeRestaurants = this._bddContext.Restos.ToList();
			return listeRestaurants;
		}

		public void SupprimerRestaurant(int id)
		{
			Resto restoToDelete = this._bddContext.Restos.Find(id);
			this._bddContext.Restos.Remove(restoToDelete);
			this._bddContext.SaveChanges();
		}

		public void SupprimerRestaurant(string nom, string telephone)
		{
			Resto restoToDelete = this._bddContext.Restos.Where(r => r.Nom == nom && r.Telephone == telephone).FirstOrDefault();
			if (restoToDelete != null)
			{
				this._bddContext.Restos.Remove(restoToDelete);
				this._bddContext.SaveChanges();
			}
		}

		public void UpdateRestaurant(int id, string nom, string telephone, string ville, string description)
		{
			Resto restoToUpdate = this._bddContext.Restos.Find(id);
			if (restoToUpdate != null)
			{
				restoToUpdate.Nom = nom;
				restoToUpdate.Telephone = telephone;
				restoToUpdate.Ville = ville; 
				restoToUpdate.Description = description;
				this._bddContext.SaveChanges();
			}
		}


		// addeed for authentification

		public Utilisateur AjouterUtilisateur(string prenom, string password, string role="Basic")
		{
			string motDePasse = EncodeMD5(password);
			Utilisateur user = new Utilisateur() { Prenom = prenom, Password = motDePasse, Role=role };
			this._bddContext.Utilisateurs.Add(user);
			this._bddContext.SaveChanges();

			return user;
		}

		public Utilisateur Authentifier(string prenom, string password)
		{
			string motDePasse = EncodeMD5(password);
			Utilisateur user = this._bddContext.Utilisateurs.FirstOrDefault(u => u.Prenom == prenom && u.Password == motDePasse);
			return user;
		}

		public Utilisateur ObtenirUtilisateur(int id)
		{
			return this._bddContext.Utilisateurs.FirstOrDefault(u => u.Id == id);
		}

		public Utilisateur ObtenirUtilisateur(string idStr)
		{
			int id;
			if (int.TryParse(idStr, out id))
			{
				return this.ObtenirUtilisateur(id);
			}
			return null;
		}

		private string EncodeMD5(string motDePasse)
		{
			string motDePasseSel = "ChoixResto" + motDePasse + "ASP.NET MVC";
			return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(motDePasseSel)));
		}

		public int CreerUnSondage()
		{
			Sondage sondage = new Sondage { Date = DateTime.Now };
			_bddContext.Sondages.Add(sondage);
			_bddContext.SaveChanges();
			return sondage.Id;
		}

		public void AjouterVote(int idSondage, int idResto, int idUtilisateur)
		{
			Vote vote = new Vote
			{
				Resto = _bddContext.Restos.First(r => r.Id == idResto),
				Utilisateur = _bddContext.Utilisateurs.First(u => u.Id == idUtilisateur)
			};
			Sondage sondage = _bddContext.Sondages.First(s => s.Id == idSondage);
			if (sondage.Votes == null)
				sondage.Votes = new List<Vote>();
			sondage.Votes.Add(vote);
			_bddContext.SaveChanges();
		}

		public List<Resultats> ObtenirLesResultats(int idSondage)
		{
			List<Resto> restaurants = ObtientTousLesRestaurants();
			List<Resultats> resultats = new List<Resultats>();
			Sondage sondage = _bddContext.Sondages.First(s => s.Id == idSondage);
			foreach (IGrouping<int, Vote> grouping in sondage.Votes.GroupBy(v => v.Resto.Id))
			{
				int idRestaurant = grouping.Key;
				Resto resto = restaurants.First(r => r.Id == idRestaurant);
				int nombreDeVotes = grouping.Count();
				resultats.Add(new Resultats { Nom = resto.Nom, Telephone = resto.Telephone, NombreDeVotes = nombreDeVotes });
			}
			return resultats;
		}

		public bool ADejaVote(int idSondage, string idStr)
		{
			int id;
			if (int.TryParse(idStr, out id))
			{
				Sondage sondage = _bddContext.Sondages.Include(s => s.Votes).First(s => s.Id == idSondage);
				if (sondage.Votes == null)
					return false;
				return sondage.Votes.Any(v => v.UtilisateurId != 0 && v.UtilisateurId == id);
			}
			return false;
		}

		public List<Sondage> ObtenirLesSondages()
		{
			return _bddContext.Sondages.ToList();
		}

		public bool RestaurantExiste(string nom)
		{
			return _bddContext.Restos.ToList().Any(resto => string.Compare(resto.Nom, nom, StringComparison.CurrentCultureIgnoreCase) == 0);
		}
	}
}
