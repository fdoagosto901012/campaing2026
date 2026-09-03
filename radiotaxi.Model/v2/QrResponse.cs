using radiotaxi.Model;
using radiotaxi.Model.v2.finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Models
{
    public class QrResponse
    {
        public string typescan { get; set; }
        public _PartnerGlobalDTO global { get; set; }
        public C_permissions Permission { get; set; } // Permiso
        public card card { get; set; } // Tarjeton

        public EmplacamientoDTO emplacamiento { get; set; }
    }
}