using Shared;

namespace project_API.Domain
{
    public class Vehicle
    {
        public int Id { get; set; }
        
        public CarModel VModel { get; set; }

        public string NumberPlate { get; set; }

        public int CarModelId { get; set; }

        public int BuildYear { get; set; }

        public int KmNumber { get; set; }

        public Energy EnergySource { get; set; }

        public List<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
    }
}
