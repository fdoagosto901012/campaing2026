using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Openpay
{
    // Guardar tarjeta
    public class SaveCardDto
    {
        public string token_id { get; set; }
        public string device_session_id { get; set; }
        public string email { get; set; }   // opcional para actualizar Customer
        public string phone { get; set; }   // opcional
        public AddressDto address { get; set; }
    }
}
