using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ApiModels
{
    public class MaintenanceModel
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }

        public int CurrentKmNumber { get; set; }

        public string WorkDescription { get; set; }
    }
}
