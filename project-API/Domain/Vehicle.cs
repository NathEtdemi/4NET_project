using Shared;

namespace project_API.Domain
{
    public class Vehicle
    {
        public int Id { get; set; }
        
        public CarModel Model { get; set; }
        public int ModelId { get; set; }

        public int BuildYear { get; set; }

        public int KmNumber { get; set; }

        public Energy EnergySource { get; set; }

        public List<Maintenance> Maintenances { get; set; }
    }
}
