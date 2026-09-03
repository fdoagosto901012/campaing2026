using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Sales
{
    public class caja_debs_reporte_DTO : baseModel
    {
        public List<caja_cargos_DTO> debs { get; set; }
        public caja_reporte_DTO report { get; set; }
    }
}
