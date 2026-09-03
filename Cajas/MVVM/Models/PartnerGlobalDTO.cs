using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public class PartnerGlobalDTO
    {
        public PartnerGlobalDTO()
        {

        }
        public Partner Partner { get; set; }
        public List<HistAsistencia> Reportes { get; set; }
        public List<MENSAJE> blocks { get; set; }
        public List<caja_cargos_DTO> debts { get; set; }
        public decimal amountdebs { get; set; }
        public List<Venta> tickets { get; set; }
        public List<p_agreements> agreements { get; set; }
    }
}
