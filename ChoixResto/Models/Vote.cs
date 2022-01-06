using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoixResto.Models
{
	public class Vote
	{
		public int Id { get; set; }
		public int RestoId { get; set; }
		public Resto Resto { get; set; }
		public int UtilisateurId { get; set; }
		public Utilisateur Utilisateur { get; set; }
	}
}
