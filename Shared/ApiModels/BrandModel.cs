using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ApiModels
{
    public class BrandModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<CarModelModel> CarModels { get; set; } = new List<CarModelModel>();
    }
}
