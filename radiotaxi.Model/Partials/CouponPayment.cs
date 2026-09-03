using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class CouponPayment : baseModel
    {
        public List<CouponPayment> getdetails(int id) {
			try
			{
                var history = this.db
                    .CouponPayments
                    .Include(x => x.Cupons)
                    .Include(x => x.CouponPaymentDetails)
                    .Include(x => x.user)
                    // ASignamos el id del usuario para obtener ese dinero.
                    .Where(x => x.userId == id)
                    .OrderBy(x => x.CreatedDate)
                    .ToList();
                return history;
			}
			catch (Exception ex)
			{
				return new List<CouponPayment>();
            }
        }

        public List<CouponPayment> getHistory(int id, int? paymentId = null)
        {
            try
            {
                List<CouponPayment> history = db.CouponPayments
                    .Include(p => p.user)
                    .Include(p => p.Cupons)
                    .Include(p => p.Cupons.Select(c => c.Couponrate))
                    .Include(p => p.Cupons.Select(c => c.Couponrate.Cupontype))
                    .Include(p => p.Cupons.Select(c => c.Couponrate.Hotel))
                    //.Where(p => p.Active && !p.Cancelled)
                    .Where(x =>
                        x.userId == id &&
                        (!paymentId.HasValue || x.CouponPaymentId == paymentId.Value)
                    )
                    .OrderByDescending(p => p.CreatedDate)
                    .ToList();
                return history;
            }
            catch (Exception ex)
            {
                return new List<CouponPayment>();
            }
        }
    }
}
