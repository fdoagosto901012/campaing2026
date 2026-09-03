using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public class p_agreements
    {
        public string Id_Op { get; set; }
        public string Concepto { get; set; }
        public string CargoANum { get; set; }
        public string CargoANombre { get; set; }
        public int partidas { get; set; }
        public string Status { get; set; }
        public decimal importe { get; set; }
    }
}
