using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class caja_proveedor_DTO : baseModel
    {
        public string id_proveedor { get; set; }
        public string proveedor { get; set; }
        public string operador { get; set; }
        public string status { get; set; }
    }
}
