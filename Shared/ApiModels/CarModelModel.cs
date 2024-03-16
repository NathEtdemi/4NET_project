using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ApiModels
{
    public class CarModelModel
    {
        public int Id { get; set; }

        public int BrandId { get; set; }

        public string Name { get; set; }

        // Fréquence d'entretien en nombre de km (exemple: tous les 20 000 km)
        public int MaintenanceFrequency { get; set; }
    }
}
