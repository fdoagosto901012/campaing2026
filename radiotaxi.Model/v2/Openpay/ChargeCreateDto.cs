using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Openpay
{
    public class ChargeCreateDto
    {
        public string CustomerId { get; set; }
        public string SourceId { get; set; }         // card_id guardada o token_id
        public decimal Amount { get; set; }
        public string Currency { get; set; }         // "MXN"
        public string Description { get; set; }
        public string OrderId { get; set; }
        public string DeviceSessionId { get; set; }
    }
}
