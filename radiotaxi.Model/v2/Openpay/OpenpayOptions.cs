using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Openpay
{
    public class OpenpayOptions
    {
        public string MerchantId { get; set; }   // ej. "mxq5mero3jq4xj38q1fd"
        public string PrivateKey { get; set; }   // ej. "sk_xxx..."
        public bool Production { get; set; }     // false = sandbox, true = prod
    }
}
