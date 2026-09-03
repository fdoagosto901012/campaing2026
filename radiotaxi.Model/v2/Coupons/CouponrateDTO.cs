using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class CouponrateDTO
    {
        public int CouponrateID { get; set; }
        public string Name { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public System.DateTime EditedDate { get; set; }
        public bool active { get; set; }
        public bool @lock { get; set; }
        public decimal Valor { get; set; }
        public int HotelID { get; set; }
        public int CupontypeID { get; set; }

        public virtual Cupontype Cupontype { get; set; }
        public virtual Hotel Hotel { get; set; }
    }
}
