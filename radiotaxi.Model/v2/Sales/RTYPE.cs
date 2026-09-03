using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial  class RTYPE : baseModel
    {
       public decimal getdouble(decimal value) {
            try
            {
                return this.db.RTYPEs.Where(x => x.simple == value).FirstOrDefault().doble;
            }
            catch (Exception)
            {
                return value*2;
            }
        }
    }
}