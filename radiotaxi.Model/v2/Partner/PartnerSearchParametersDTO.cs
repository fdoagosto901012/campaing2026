using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Partner
{
    public class PartnerSearchParametersDTO
    {
        public string gafet { get; set; }
        public string name { get; set; }
        public string lastName1 { get; set; }
        public string lastName2 { get; set; }
        public string placa { get; set; }
        public string serie { get; set; }


        public string EconoResp { get; set; }
        public string Responsable { get; set; }

        public PartnerSearchParametersDTO()
        {
            this.gafet = "";
            this.name = "";
            this.lastName1 = "";
            this.lastName2 = "";
            this.serie = "";
            this.placa = "";

            this.EconoResp = "";
            this.Responsable = "";
        }
    }
}
