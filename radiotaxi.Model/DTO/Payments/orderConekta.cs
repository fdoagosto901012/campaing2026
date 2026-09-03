using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class orderConekta
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string description { get; set; }
        public string reference_id { get; set; }
        public float amount { get; set; }
        public string card { get; set; }
        public string currency { get; set; }
        public PackageDTO package { get; set; }

    }
}
