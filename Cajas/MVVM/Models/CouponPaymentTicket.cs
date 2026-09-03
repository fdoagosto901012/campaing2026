using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public sealed class CouponPaymentTicket
    {
        public int PaymentId { get; init; }

        public DateTime PaymentDate { get; init; }

        public string CashierName { get; init; } = string.Empty;

        public string OperatorNumber { get; init; } = string.Empty;

        public string OperatorName { get; init; } = string.Empty;

        public decimal TotalAmount { get; init; }

        public string title {  get; init; }  = string.Empty;

        public IReadOnlyCollection<CouponTicketItem> Coupons { get; init; }
            = Array.Empty<CouponTicketItem>();
    }
}
