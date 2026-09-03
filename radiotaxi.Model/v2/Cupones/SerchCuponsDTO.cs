using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class SerchCuponsDTO
    {
        // Hotel id Define a que hotel pertenece el cupon, definido en cupon_rate

        public int hotelid { get; set; }    
        public int folio { get; set; }
        public int cupontype { get; set; }
        public int hc { get; set; }
    }
}
