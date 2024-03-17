using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ApiModels
{
    public class MaintenanceModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le champ est requis")]
        public int VehicleId { get; set; }

        [Range(0, 999999, ErrorMessage = "La valeur des km doit être comprise entre 0 et 999 999 km")]
        public int CurrentKmNumber { get; set; }

        [Required(ErrorMessage = "Le champ détail des travaux est requis")]
        public string WorkDescription { get; set; }
    }
}
