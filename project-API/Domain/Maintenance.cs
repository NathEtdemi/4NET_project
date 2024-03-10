namespace project_API.Domain
{
    public class Maintenance
    {
        public int Id { get; set; }

        public Vehicle MaintainedVehicule { get; set; }
        public int VehicleId { get; set; }

        public int CurrentKmNumber { get; set; }

        public string WorkDescription { get; set; }
    }
}
