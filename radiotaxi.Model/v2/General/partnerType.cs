using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class partnerType : baseModel
    {
        public List<partnerType> get() { 
            return db.partnerTypes.ToList();
        }
    }
}
