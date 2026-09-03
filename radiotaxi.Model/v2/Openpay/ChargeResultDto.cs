using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Openpay
{
    public class ChargeResultDto
    {
        public string ChargeId { get; set; }
        public string Status { get; set; }           // completed, failed, etc.
        public string OperationType { get; set; }
        public string TransactionType { get; set; }
        public string Authorization { get; set; }
        public DateTime? CreationDate { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string OrderId { get; set; }
        public string Description { get; set; }
    }
}
