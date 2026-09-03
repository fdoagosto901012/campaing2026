using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class p_debt : baseModel
    {
        public string CLAVE { get; set; }
        public string CARGO { get; set; }
        public string A { get; set; }
        public int DEBE { get; set; }
        public Decimal PRECIO_IVA { get; set; }
        public Decimal IMPORTE_DEBE { get; set; }
    }
}
