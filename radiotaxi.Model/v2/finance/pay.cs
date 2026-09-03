using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.finance
{
    public class pay : baseModel
    {
        public string Id_Cliente { get; set; } 
        public string Id_Op { get; set; }
        public string Id_Producto { get; set; }
        public decimal IMPORTE_MES { get; set; }
        public string Familia { get; set; }
        public DateTime FechaOp { get; set; }

    }
}
