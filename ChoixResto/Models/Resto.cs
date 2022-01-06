using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChoixResto.Models
{
	[Table("restaurants")]
	public class Resto
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Le nom du restaurant doit être rempli !")]
		[MaxLength(25)]
		public string Nom { get; set; }

		[MaxLength(10)]
		[Display(Name="Téléphone")]
		[RegularExpression(@"^\d{10}$", ErrorMessage = "Le numéro de téléphone doit contenir 10 chiffres !")]
		public string Telephone { get; set; }

		public string Description { get; set; }
		[MaxLength(20)]
		public string Ville { get; set; }

	}
}
