using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public sealed class CouponTicketItem
    {
        public int CouponId { get; init; }

        public string Code { get; init; } = string.Empty;

        public decimal Amount { get; init; }
    }
}