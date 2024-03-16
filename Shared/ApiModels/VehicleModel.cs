using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ApiModels
{
    public class VehicleModel
    {
        public int Id { get; set; }

        public CarModelModel VModel { get; set; }

        public int ModelId { get; set; }

        public string NumberPlate { get; set; }

        public int BuildYear { get; set; }

        public int KmNumber { get; set; }

        public Energy EnergySource { get; set; }

        public IList<MaintenanceModel> Maintenances { get; set; } = new List<MaintenanceModel>();
    }
}
