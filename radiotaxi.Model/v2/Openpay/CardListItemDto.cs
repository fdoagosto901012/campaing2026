using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Openpay
{
    public class CardListItemDto
    {
        public string Id { get; set; }              // card_id en Openpay
        public string customer_id { get; set; }

        public string Brand { get; set; }           // Visa/Mastercard
        public string Type { get; set; }            // credit/debit
        public string Last4 { get; set; }           // últimos 4 dígitos
        public string HolderName { get; set; }
        public string ExpirationMonth { get; set; } // "01".."12"
        public string ExpirationYear { get; set; }  // "33" etc.
        public string BankName { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool? AllowsCharges { get; set; }    // si el SDK lo expone
        public bool? AllowsPayouts { get; set; }    // si el SDK lo expone
    }
}