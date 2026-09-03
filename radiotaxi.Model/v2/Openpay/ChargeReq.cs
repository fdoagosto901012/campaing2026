using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Openpay
{

    // Cargo
    public class ChargeReq
    {
        public string customer_id { get; set; }
        public string source_id { get; set; }       // card_id guardada o token
        public decimal amount { get; set; }
        public string currency { get; set; } = "MXN";
        public string description { get; set; }
        public string order_id { get; set; }
        public string device_session_id { get; set; }
    }
}
