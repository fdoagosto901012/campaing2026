using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public partial class HistAsistencia
    {
        public string Gafete { get; set; }
        public string Tipo { get; set; }
        public string Taxi { get; set; }
        public string Turno { get; set; }
        public DateTime FechaReporte { get; set; }
        public string Concepto { get; set; }
        public string Ticket { get; set; }
        public DateTime? FechaOp { get; set; }
    }
}
