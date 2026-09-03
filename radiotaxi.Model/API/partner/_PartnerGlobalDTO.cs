using radiotaxi.Model.v2.finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class _PartnerGlobalDTO
    {
        public _PartnerGlobalDTO()
        {

        }
        public Partner Partner { get; set; }
        public Operator Operator { get; set; }
        public int userID { get; set; }
        public string partnerReference { get; set; }
        public string firstName { get; set; }
        public string lastNameF { get; set; }
        public string lastNameM { get; set; }
        public List<amazonPicture> amazonPictures { get; set; }
        public DateTime LastReportDate { get; set; }
        public DateTime ingreso { get; set; }
        public List<HistAsistencia> Reportes { get; set; }
        public List<MENSAJE> blocks { get; set; }
        public List<caja_cargos_DTO> debts { get; set; }
        public decimal amountdebs { get; set; }
        public List<Venta> tickets { get; set; }
        public List<Convenio> agreements { get; set; }
        public financesOperator finances { get; set; }
        public EmplacamientoDTO Emplacamiento { get; set; }
        public RTYPE priceReport { get; set; }

        public string status { get; set; }
    }
}
