using Shared.ApiModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.FormModels
{
	public class VehicleFormModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Le champ est requis")]
		public int CarModelId { get; set; }

		[Required(ErrorMessage = "Le champ Immatriculatino est requis!")]
		[StringLength(9, MinimumLength = 7, ErrorMessage = "L'immatriculation doit être comprise entre 7 et 9 caractères")]
		public string NumberPlate { get; set; }

		public int BuildYear { get; set; }

		[Range(0, 999999, ErrorMessage = "La valeur des km doit être comprise entre 0 et 999 999 km")]
		public int KmNumber { get; set; }

		public Energy EnergySource { get; set; }
	}
}
