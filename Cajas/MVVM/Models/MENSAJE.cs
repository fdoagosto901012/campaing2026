using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public partial class MENSAJE
    {
        public string TAXI { get; set; }
        public string CHOFER { get; set; }
        public string ECONOMICO { get; set; }
        public string MENSAJE1 { get; set; }
        public DateTime FECHA { get; set; }
        public string DETENER { get; set; }
        public string ID_SECRETARIA { get; set; }
        public string SECRETARIA { get; set; }
        public bool DELSISTEMA { get; set; }
        public bool TEMPORAL { get; set; }
        public string FOLIO { get; set; }
        public string ID_USUARIO { get; set; }
        public string USUARIO { get; set; }
        public DateTime? fechacompletaserver { get; set; }
        public string usuarioultimaedicion { get; set; }
    }
}
