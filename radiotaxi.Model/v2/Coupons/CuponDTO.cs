using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class CuponDTO
    {
        public int CuponsID { get; set; }
        public string Name { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public System.DateTime EditedDate { get; set; }
        public bool active { get; set; }
        public bool @lock { get; set; }
        public Nullable<int> UserID { get; set; }
        public int CouponratesID { get; set; }
        public Nullable<int> cuponsGroupPrintedId { get; set; }

        public virtual CouponrateDTO Couponrate { get; set; }
        public virtual user user { get; set; }
        public virtual cuponsGroupPrinted cuponsGroupPrinted { get; set; }
    }
}
