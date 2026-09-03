using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.API
{
    public  class PaymentRegisterDTO
    {
        public SaleDTO sale { get; set; }  
        public string ChargeToken { get; set; }  // Define el token de la venta que este registra en la base de datos como completada.
    }
}
