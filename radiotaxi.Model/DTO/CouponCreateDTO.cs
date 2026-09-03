using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public  class CouponCreateDTO
    {
        public int CuponsID { get; set; }
        public int cant { get; set; }
        public int tarifa { get; set; }
        public bool active { get; set; }
        public string createBy { get; set; }
    }
}