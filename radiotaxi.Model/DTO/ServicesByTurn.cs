using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class ServicesByTurn
    {
        public DateTime dateOfDay { get; set; }
        public String turn { get; set; }
        public int NumberServices { get; set; }
    }
}
