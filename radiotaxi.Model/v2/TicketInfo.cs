using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public  class TicketInfo
    {
        public string folio { get; set; }
        public List<Ticket_DTO> tickets { get; set; }
        public decimal total { get; set; }
        public DateTime fechaOp { get; set; }
    }
}