using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Sales
{
    public class Ticket_Detail_DTO
    {
        public List<Ticket_DTO> tickets { get; set; }
        public decimal total { get; set; }
        public string ticket { get; set; }
        public Venta venta  { get; set; }


    }
}
