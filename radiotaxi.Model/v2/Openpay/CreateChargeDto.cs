using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Openpay
{
    public class CreateChargeDto
    {
        public string TokenId { get; set; }
        public string DeviceSessionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "MXN";
        public string OrderId { get; set; }
        public string Description { get; set; }
        // opcional: datos de cliente
        // public string CustomerId { get; set; }
    }
}
