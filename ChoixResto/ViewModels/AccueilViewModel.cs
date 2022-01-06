using ChoixResto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoixResto.ViewModels
{
	public class AccueilViewModel
	{
		public string Message { get; set; }
		public DateTime Date { get; set; }
		public Resto MainResto { get; set; }
		public List<Resto> ListeRestos { get; set; }

	}
}
