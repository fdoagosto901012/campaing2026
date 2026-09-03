using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Openpay
{
    public class BillingAddress
    {
        public string Line1 { get; set; }      // calle y número
        public string Line2 { get; set; }      // colonia
        public string Line3 { get; set; }      // referencia
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; } // 'MX'
    }

}
