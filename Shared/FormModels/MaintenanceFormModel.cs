using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.FormModels
{
	public class MaintenanceFormModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Le champ est requis")]
		public int VehicleId { get; set; }

		[Required(ErrorMessage = "Le champ détail des travaux est requis")]
		public string WorkDescription { get; set; }
	}
}
