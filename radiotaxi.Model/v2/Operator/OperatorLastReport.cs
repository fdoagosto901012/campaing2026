using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class OperatorLastReport : baseModel
    {
        public int userid { get; set; }

        public string Gafete { get; set; }
        public DateTime FechaReporte { get; set; }
    }
}
