using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Sales
{
    public class Sale_Response
    {
        public Sale_Response() { }

        public int code { get; set; }
        public Dictionary<string , string> message { get; set; }
        public List<caja_cargos_DTO> charges { get; set; }
        public List<caja_cargos_DTO> pays { get; set; }
        public Inventario inventario { get; set; }
        public Ticket_Detail_DTO Ticket { get; set; }
    }
}
