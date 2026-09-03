using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Openpay
{
    public class OpenPayTransportData
    {
        public bool isCompleted = false;

        public int external_id { get; set; } // Este es el userID del usuario
        public string customer_id { get; set; } // Y este es como se identifica en openPay.
        public string message { get; set; }
        public PaymentResult payment_result { get; set; } // Aqui se transporta el deviceID y el Token_id(Identificador de tarjeta)
        public List<CardListItemDto> cards { get; set; }
        public CardSavedDto card { get; set; }
        public ChargeCreateDto charge { get; set; }
        public ChargeResultDto chargeResult { get; set; }

        public List<openpay_cargos_DTO> Order { get; set;}
        public Ticket_openpay_DTO ticket { get; set; }
        public Venta venta { get; set; }
    }
}
