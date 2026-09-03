using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Openpay
{
    public class openpay_cargos_DTO
    {
        public string PRODUCTO { get; set; }
        public string TYPE { get; set; }
        // DEFINE LA FAMILIA
        public string CLAVE { get; set; }
        public string CARGO { get; set; }
        // Define la subfamilia.
        public string IDA { get; set; }
        public string A { get; set; }
        public int DEBE { get; set; }
        public int PAGA { get; set; }
        public Decimal PRECIO { get; set; }
        public Decimal PRECIOFIJO { get; set; }
        public string Id_Producto { get; set; }
        public DateTime FechaOp { get; set; }
        public string Descripcion { get; set; }

        public string CONVENIO { get; set; }
        public string RTYPE { get; set; } // Define el tipo de Reporte D - Doble 
        public bool RDOUBLE { get; set; } // Define el tipo de Reporte D - Doble 
        public int MULTIPLICADOR { get; set; } // Define el tipo de Reporte D - Doble 
    }
}
