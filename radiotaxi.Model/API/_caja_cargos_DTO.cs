using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.API
{
    public  class _caja_cargos_DTO
    {
        public _caja_cargos_DTO()
        {
            this.MULTIPLICADOR = 1;
            this.RTYPE = "D";
        }

        private bool chacked = true;

        [DefaultValue(false)]
        public bool CHECK
        {
            get
            {
                return chacked;
            }
            set
            {
                chacked = value;
            }
        }

        private bool _service = false;
        [DefaultValue(false)]
        public bool SERVICE
        {
            get
            {
                return _service;
            }
            set
            {
                _service = value;
            }
        }
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
