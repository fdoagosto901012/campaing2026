using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class orderPreferences
    {
        public string References { get; set; }
        public DateTime time { get; set; }
        public int catalogServiceId { get; set; }
        public string colonia { get; set; }
        public string SM { get; set; }
        public string MZ { get; set; }
        public string LT { get; set; }
        public string C { get; set; }
        public string edificio { get; set; }
        public string observations { get; set; }

    }
}
