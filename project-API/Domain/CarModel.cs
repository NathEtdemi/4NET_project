using System.ComponentModel.DataAnnotations;

namespace project_API.Domain
{
    public class CarModel
    {
        public int Id { get; set; }

        public Brand Brand { get; set; }

        [Required(ErrorMessage = "Le champ est requis")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Le champ nom du modèle est requis!")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Le nom du modèle doit être compris entre 1 et 20 caractères")]
        public string Name { get; set; }

        // Fréquence d'entretien en nombre de km (exemple: tous les 20 000 km)
        [Range(0, 100000, ErrorMessage = "La fréquence de maintenance doit être comprise entre 0 et 100 000 km")]
        public int MaintenanceFrequency { get; set; }

        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
