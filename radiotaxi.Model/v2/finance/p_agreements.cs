using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.finance
{
    public class p_agreements
    {
        public string Id_Op { get; set; }
        public string Concepto { get; set; }
        public string CargoANum { get; set; }
        public string CargoANombre { get; set; }

        public string AfavordeNum { get; set; }
        public string AfavordeNombre { get; set; }
        public int partidas { get; set; }
        public string Status { get; set; }
        public Decimal importe { get; set; }
    }
}
