using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class relationShipStatu : baseModel
    {
        public List<relationShipStatu> get() { 
            return db.relationShipStatus.ToList();
        }
    } 
}
