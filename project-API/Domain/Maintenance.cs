using System.ComponentModel.DataAnnotations;

namespace project_API.Domain
{
    public class Maintenance
    {
        public int Id { get; set; }

        public Vehicle MaintainedVehicle { get; set; }

        [Required(ErrorMessage = "Le champ est requis")]
        public int VehicleId { get; set; }

        [Range(0, 999999, ErrorMessage = "La valeur des km doit être comprise entre 0 et 999 999 km")]
        public int CurrentKmNumber { get; set; }

        [Required(ErrorMessage = "Le champ détail des travaux est requis")]
        public string WorkDescription { get; set; }
    }
}
