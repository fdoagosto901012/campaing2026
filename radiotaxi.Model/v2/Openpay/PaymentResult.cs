using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Openpay
{
    public class PaymentResult
    {
        public string TokenId { get; set; }
        public string DeviceSessionId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public BillingAddress Address { get; set; } = new BillingAddress();

        // contacto
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
