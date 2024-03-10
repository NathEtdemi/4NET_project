namespace project_API.Domain
{
    public class CarModel
    {
        public int Id { get; set; }

        public Brand ModelBrand { get; set; }
        public int BrandId { get; set; }

        public string Name { get; set; }

        // Fréquence d'entretien en nombre de km (exemple: tous les 20 000 km)
        public int MaintenanceFrequency { get; set; }

        public List<Vehicle> Vehicles { get; set; }
    }
}
