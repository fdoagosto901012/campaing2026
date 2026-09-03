using Cajas.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.Services
{
    public interface ITicketPrinterService
    {
        Task<PrintTicketResult> PrintCouponPaymentAsync(
            CouponPaymentTicket ticket,
            CancellationToken cancellationToken = default);
    }
}
