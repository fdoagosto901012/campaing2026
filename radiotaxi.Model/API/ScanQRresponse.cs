using radiotaxi.Model.API;
using radiotaxi.Model.API.cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class ScanQRresponse
    {
        public ScanQRresponse() { }
        private credential credential { get; set; } // Credencial 
        public CuponDTO Cupon { get; set; } // Cupon
        public PermissionsResponse Permissions { get; set; } // Pregafet
        public _PartnerGlobalDTO globalUser { get; set; } // Datos del usuario. 
        public CardResponse Card { get; set; } // Tarjeton

        public string ScanType { get; set; }
    }
}