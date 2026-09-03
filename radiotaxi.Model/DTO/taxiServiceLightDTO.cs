using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class taxiServiceLightDTO
    {
        public int id { get; set; }
        public string address { get; set; }
        public DateTime date { get; set; }
        public int status { get; set; }
    }
}
