using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ApiModels
{
    public class CarModelModel
    {
        public int Id { get; set; }

        public BrandModel Brand { get; set; }

        [Required(ErrorMessage = "Le champ est requis")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Le champ nom du modèle est requis")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Le nom du modèle doit être compris entre 1 et 20 caractères")]
        public string Name { get; set; }

        [Range(0, 100000, ErrorMessage = "La fréquence de maintenance doit être comprise entre 0 et 100 000 km")]
        public int MaintenanceFrequency { get; set; }
    }
}
