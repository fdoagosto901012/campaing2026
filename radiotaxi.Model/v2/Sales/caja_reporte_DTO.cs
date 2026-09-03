using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class caja_reporte_DTO : baseModel
    {
        public DateTime FECHAREPORTE { get; set; }
        public string TURNO { get; set; }
    }
}
