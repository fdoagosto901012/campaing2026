using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Openpay;
using Openpay.Entities;
using Openpay.Entities.Request;
using radiotaxi.API.Controllers.Base;
using radiotaxi.API.Hubs;
using radiotaxi.API.Providers;
using radiotaxi.Model;
using radiotaxi.Model.API.cards;
using radiotaxi.Model.v2.Openpay;
using radiotaxi.Model.v2.Sales;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Charge = Openpay.Entities.Charge;
using Customer = Openpay.Entities.Customer;

namespace radiotaxi.API.Controllers
{
    [Authorize(Roles = "soc")]
    [RoutePrefix("api/payments")]
    public class PaymentsController : BaseControllerWithHub<PaymentsHub>
    {
        private readonly IConfiguration _cfg;
        //public PaymentsController(IConfiguration cfg) => _cfg = cfg;
        //string privateKey = "sk_e319cc73f5b94667a0d50194102c4af5";
        //private string publicKey = "pk_7ff0068b5825428e9f2f5ea434f32d30";
        //private string mechandID = "mxq5mero3jq4xj38q1fd";
        private SearchParams request;

        [HttpGet]
        [Route("ticket/{ticket}")]
        public HttpResponseMessage Getticket(string ticket)
        {
            Ticket_DTO ticketDTO = new Ticket_DTO();
            List<Ticket_DTO> tickets = ticketDTO.get(ticket);
            decimal total = 0;
            for (int i = 0; i < tickets.Count(); i++)
            {
                tickets[i].Total = tickets[i].Cant * tickets[i].PrecioConiva;
                total += tickets[i].Total;
            }

            TicketInfo ticketInfo = new TicketInfo();
            ticketInfo.tickets = tickets;
            ticketInfo.total = total;
            ticketInfo.folio = ticket;

            if (tickets != null && tickets.Count() > 1)
            {
                Ticket_DTO value = tickets.FirstOrDefault();
                ticketInfo.fechaOp = value.FechaOP.Date + value.HoraOp.TimeOfDay;
            }

            return Request.CreateResponse(
                HttpStatusCode.OK, this.content<TicketInfo>(ticketInfo),
                Configuration.Formatters.JsonFormatter
            );
        }


        // Guardado de tarjeta.
        [HttpPost]
        [Route("save/card")] // Vamos a guardar el token_id en openpay para identificar al cliente
        public async Task<HttpResponseMessage> savecard([FromBody] OpenPayTransportData dto) {
            try
            {
                // Verificar si el usuario existe, de manera externa o interna.
                user user = new user();
                user = user.get(dto.external_id); // Obtenemos los datos del usuario.
                CustomServices OpenPayClient = new CustomServices();
                // Mandamos los datos para crear al cliente.
                dto.customer_id = await OpenPayClient.EnsureOpenpayCustomerAsync(
                    dto.external_id.ToString(),
                    user.firstName,
                    user.lastNameF + " " + user.lastNameM, 
                    dto.payment_result.Email, 
                    "0000000000",
                    dto.payment_result.Address.CountryCode,
                    dto.payment_result.Address.City,
                    dto.payment_result.Address.State,
                    dto.payment_result.Address.Line1,
                    dto.payment_result.Address.PostalCode
                );
                // Guardamos la tarjeta de credito para futuras compras.
                CardSavedDto cardSavedDto = await OpenPayClient.SaveCardAsync(
                        dto.customer_id,
                        dto.payment_result.TokenId,
                        dto.payment_result.DeviceSessionId,
                        dto.payment_result.Email,
                        dto.payment_result.Phone,
                        new AddressDto()
                        {
                            city = dto.payment_result.Address.City,
                            country_code = dto.payment_result.Address.CountryCode,
                            line1 = dto.payment_result.Address.Line1,
                            line2 = dto.payment_result.Address.Line2,
                            line3 = dto.payment_result.Address.Line3,
                            postal_code = dto.payment_result.Address.PostalCode,
                            state = dto.payment_result.Address.State                             
                        }
                    );
                if (cardSavedDto != null)
                {
                    dto.card = cardSavedDto;
                }
                return Request.CreateResponse(
                    HttpStatusCode.OK, this.content<OpenPayTransportData>(dto),
                    Configuration.Formatters.JsonFormatter
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() );
                dto.message = ex.Message;
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest, this.content<OpenPayTransportData>(dto),
                    Configuration.Formatters.JsonFormatter
                );
            }
        }

        // Guardado de tarjeta.
        [HttpPost]
        [Route("get/cards")] // Vamos a guardar el token_id en openpay para identificar al cliente
        public async Task<HttpResponseMessage> getcards([FromBody] OpenPayTransportData dto)
        {
            try
            {
                // Verificar si el usuario existe, de manera externa o interna.
                user user = new user();
                List<CardListItemDto> cards = new List<CardListItemDto>();
                user = user.get(dto.external_id); // Obtenemos los datos del usuario.
                CustomServices OpenPayClient = new CustomServices();
                var found = await OpenPayClient.FindCustomerIdByExternalIdAsync(dto.external_id.ToString());
                if (string.IsNullOrEmpty(found))
                {
                    dto.message = "Sin Tarjetas registradas";
                    dto.cards = cards;
                    return Request.CreateResponse(
                        HttpStatusCode.BadRequest, this.content<OpenPayTransportData>(dto),
                        Configuration.Formatters.JsonFormatter
                    );
                }
                // Obtenemos todas las tarjetas de credito...
                cards = await OpenPayClient.GetCustomerCardsAsync(found);
                dto.cards = cards; // Asigamos cards a el objeto returnoado
                return Request.CreateResponse(
                    HttpStatusCode.OK, this.content<OpenPayTransportData>(dto),
                    Configuration.Formatters.JsonFormatter
                );
            }
            catch (Exception ex)
            {
                dto.message= ex.Message;
                Console.WriteLine(ex.ToString());
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest, this.content<OpenPayTransportData>(dto),
                    Configuration.Formatters.JsonFormatter
                );
            }
        }

        [HttpPost]
        [Route("save/charge")] // Vamos a guardar el token_id en openpay para identificar al cliente
        public async Task<HttpResponseMessage> saveCharge([FromBody] OpenPayTransportData dto)
        {
            try
            {
                /************         Seccion informativa y de control       **********************/
                // Verificar si el usuario existe, de manera externa o interna.    
                user User = new user();
                CustomServices OpenPayClient = new CustomServices();
                User = User.get(dto.external_id); // Obtenemos los datos del usuario.
                Sale sale = new Sale(User.partnerReference);
                List<caja_cargos_DTO> Items = sale.debs(User.partnerReference, User.partnerReference);
                caja_debs_reporte_DTO CargosProducto = sale.loadDebsProduct(User.partnerReference, User.partnerReference);
                List<caja_cargos_DTO> _pays = new List<caja_cargos_DTO>(); // Aqui se van a colocar los items a pagar ya filtrados.
                
                /**********   Pagos normales   **********/
                foreach (var order in dto.Order)
                {
                    caja_cargos_DTO a = Items.Where(x => x.DEBE == order.DEBE && x.CLAVE == order.CLAVE).FirstOrDefault<caja_cargos_DTO>();
                    if (a != null)
                    {
                        a.PAGA = order.PAGA; // ASiganamos cuantos elementos va a pagar.
                        _pays.Add(a); // Agregamos ese elemento a los pagos ya filtrados.
                    }
                }

                /**********  Los reportes se tratan de manera diferente  **********/
                openpay_cargos_DTO reportCharge = dto.Order.Where(x => x.CLAVE == "36").FirstOrDefault();
                if (reportCharge != null)
                {// Significa existe un cargo por reporte.
                    caja_cargos_DTO a = new caja_cargos_DTO() {
                        PRODUCTO = reportCharge.PRODUCTO, 
                        TYPE = reportCharge.TYPE,
                        CLAVE = reportCharge.CLAVE,
                        CARGO = reportCharge.CARGO,
                        IDA = reportCharge.IDA,
                        A = reportCharge.A,
                        DEBE = reportCharge.DEBE,
                        PAGA = reportCharge.PAGA,
                        PRECIO = reportCharge.PRECIO,
                        PRECIOFIJO = reportCharge.PRECIOFIJO,
                        Id_Producto = reportCharge.Id_Producto,
                        FechaOp = reportCharge.FechaOp,
                        Descripcion = reportCharge.Descripcion,
                        CONVENIO = reportCharge.CONVENIO,
                        RTYPE = reportCharge.RTYPE,
                        RDOUBLE = reportCharge.RDOUBLE,
                        MULTIPLICADOR = reportCharge.MULTIPLICADOR
                    };
                    _pays.Add(a);
                }
                /**********************************************************************/


                decimal _amount = _pays.Sum(x => x.PRECIO * x.PAGA);
                if (_pays.Count > 0 && _amount == dto.charge.Amount ) // Si hay algo que pagar entonces procede a generar el cargo en OpenPay.
                {
                    // Generemos el pago a OpenPay
                    Console.WriteLine("a");
                    ChargeResultDto charge =  await OpenPayClient.CreateChargeAsync(dto.charge);
                    if (charge != null && charge.Status.ToLower() == "completed") // Si el cargo fue completado. 
                    {
                        // Entonces se acutaliza el inventario.
                        Ticket_Detail_DTO ticket = sale.paytwo("OpenPay", "", User.partnerReference, User.partnerReference, _pays, CargosProducto);
                        Ticket_openpay_DTO openTicket = new Ticket_openpay_DTO();
                        if (ticket != null)
                        {
                            openTicket.ticket = ticket.ticket;
                            openTicket.total = ticket.total;
                            openTicket.tickets = ticket.tickets;
                            dto.ticket = openTicket;
                            dto.venta = sale.getventa(ticket.ticket);
                            dto.isCompleted = true;
                            return Request.CreateResponse(
                                HttpStatusCode.OK, this.content<OpenPayTransportData>(dto),
                                Configuration.Formatters.JsonFormatter
                            );
                        }
                    }
                }
                dto.message = "unsoported";
                return Request.CreateResponse(
                    HttpStatusCode.OK, this.content<OpenPayTransportData>(dto),
                    Configuration.Formatters.JsonFormatter
                );
            }
            catch (Exception ex)
            {
                dto.message = ex.Message;
                Console.WriteLine(ex.ToString());
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest, this.content<OpenPayTransportData>(dto),
                    Configuration.Formatters.JsonFormatter
                );
            }
        }


        [HttpPost]
        [Route("test")]
        public HttpResponseMessage test([FromBody] OpenPayTransportData dto)
        {
            List<CardListItemDto> cards = new List<CardListItemDto>();
            return Request.CreateResponse(
                HttpStatusCode.OK, this.content<List<CardListItemDto>>(cards),
                Configuration.Formatters.JsonFormatter
            );
        }


        [HttpPost]
        [Route("charges")]
        public HttpResponseMessage CreateCharge([FromBody] CreateChargeDto dto)
        {
            var merchantId = _cfg["Openpay:MerchantId"];
            var privateKey = _cfg["Openpay:PrivateKey"];
            var production = bool.Parse(_cfg["Openpay:Production"] ?? "false");
            var api = new OpenpayAPI(privateKey, merchantId) { Production = production };
            var req = new ChargeRequest
            {
                Method = "card",
                SourceId = dto.TokenId,          // token_id que generaste en MAUI
                Amount = dto.Amount,
                Currency = dto.Currency,
                Description = dto.Description,
                OrderId = dto.OrderId,
                DeviceSessionId = dto.DeviceSessionId
            };
            var charge = api.ChargeService.Create(req);
            return Request.CreateResponse(
                HttpStatusCode.OK, this.content<Charge>(charge),
                Configuration.Formatters.JsonFormatter
            );
        }

        [HttpPost]
        [Route("pay")]
        [ResponseType(typeof(CardResponse))]
        public HttpResponseMessage pay(List<int> pays)
        {
            // Configura tus credenciales
            string merchantId = "MERCANT_ID_AQUI";
            string privateKey = "LLAVE_PRIVADA_AQUI";
            bool production = false; // Cambiar a true si estás en producción
            // Crea el cliente de OpenPay
            OpenpayAPI openpayAPI = new OpenpayAPI(merchantId, privateKey, "MX", production);
            try
            {
                // Crea una tarjeta (puedes guardar previamente o usar directamente)
                Card card = new Card
                {
                    CardNumber = "4111111111111111",
                    HolderName = "Juan Perez",
                    Cvv2 = "110",
                    ExpirationMonth = "12",
                    ExpirationYear = "25"
                };
                // Crea el objeto de cargo
                ChargeRequest chargeRequest = new ChargeRequest
                {
                    Method = "card",
                    SourceId = null, // null si vas a enviar los datos de la tarjeta directamente
                    Card = card,
                    Amount = 100.00m,
                    Description = "Compra de ejemplo",
                    Currency = "MXN",
                    OrderId = "ORD-12345"
                };

                // Realiza el cargo
                Charge charge = openpayAPI.ChargeService.Create(chargeRequest);
                Console.WriteLine("Cargo exitoso. ID: " + charge.Id);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en la transacción: " + ex.Message);
            }



            // Crear cliente...
            Customer customer = new Customer();
            customer.Name = "Net Client";
            customer.LastName = "C#";
            customer.Email = "net@c.com";
            customer.Address = new Address();
            customer.Address.Line1 = "line 1";
            customer.Address.PostalCode = "12355";
            customer.Address.City = "Queretaro";
            customer.Address.CountryCode = "MX";
            customer.Address.State = "Queretaro";


            CardResponse Response = new CardResponse();
            EmplacamientoDTO _emplacamiento = null;
            Partner _partner = new Partner();
            Operator _operator = new Operator();
            user User = null;
            card _card = new card().get(0);
            if (_card != null)
            {
                Response.Card = _card;
                Response.Car = new Emplacamiento().get(_card.taxi);
                if (_card.partnerReference.Contains("OP-"))
                {
                    Response.Operator = _operator.get(_card.partnerReference);
                }
                else
                {
                    Response.Partner = _partner.get(_card.partnerReference);
                }
            }
            return Request.CreateResponse(
                HttpStatusCode.OK, this.content<CardResponse>(Response),
                Configuration.Formatters.JsonFormatter
            );
        }
        //Guardamos tarjeta... 
        [HttpPost]
        [Route("pay")]
        [ResponseType(typeof(CardResponse))]
        public HttpResponseMessage saveCard(List<int> pays)
        {
            // Configura tus credenciales
            string merchantId = "MERCANT_ID_AQUI";
            string privateKey = "LLAVE_PRIVADA_AQUI";
            bool production = false; // Cambiar a true si estás en producción
            // Crea el cliente de OpenPay
            OpenpayAPI openpayAPI = new OpenpayAPI(merchantId, privateKey, "MX", production);
            try
            {
                // Crea una tarjeta (puedes guardar previamente o usar directamente)
                Card card = new Card
                {
                    CardNumber = "4111111111111111",
                    HolderName = "Juan Perez",
                    Cvv2 = "110",
                    ExpirationMonth = "12",
                    ExpirationYear = "25"
                };
                // Crea el objeto de cargo
                ChargeRequest chargeRequest = new ChargeRequest
                {
                    Method = "card",
                    SourceId = null, // null si vas a enviar los datos de la tarjeta directamente
                    Card = card,
                    Amount = 100.00m,
                    Description = "Compra de ejemplo",
                    Currency = "MXN",
                    OrderId = "ORD-12345"
                };

                // Realiza el cargo
                Charge charge = openpayAPI.ChargeService.Create(chargeRequest);
                Console.WriteLine("Cargo exitoso. ID: " + charge.Id);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en la transacción: " + ex.Message);
            }
            // Crear cliente...
            Customer customer = new Customer();
            customer.Name = "Net Client";
            customer.LastName = "C#";
            customer.Email = "net@c.com";
            customer.Address = new Address();
            customer.Address.Line1 = "line 1";
            customer.Address.PostalCode = "12355";
            customer.Address.City = "Queretaro";
            customer.Address.CountryCode = "MX";
            customer.Address.State = "Queretaro";


            CardResponse Response = new CardResponse();
            EmplacamientoDTO _emplacamiento = null;
            Partner _partner = new Partner();
            Operator _operator = new Operator();
            user User = null;
            card _card = new card().get(0);
            if (_card != null)
            {
                Response.Card = _card;
                Response.Car = new Emplacamiento().get(_card.taxi);
                if (_card.partnerReference.Contains("OP-"))
                {
                    Response.Operator = _operator.get(_card.partnerReference);
                }
                else
                {
                    Response.Partner = _partner.get(_card.partnerReference);
                }
            }
            return Request.CreateResponse(
                HttpStatusCode.OK, this.content<CardResponse>(Response),
                Configuration.Formatters.JsonFormatter
            );
        }


        [HttpGet]
        [Route("charges/{id}")]
        public HttpResponseMessage charges() {

            string response = "";
            return Request.CreateResponse(
                    HttpStatusCode.OK, this.content<string>(""),
                    Configuration.Formatters.JsonFormatter
                );
        }


        
        // OPEN PAY...
        [HttpDelete]
        [Route("{customerId}/cards/{cardId}")]
        public IHttpActionResult DeleteCard(string customerId, string cardId)
        {
            try
            {
                // TLS 1.2 para .NET 4.5
                System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                var api = new OpenpayAPI("", "") { Production = false };

                // 1) Eliminar en Openpay
                api.CardService.Delete(customerId, cardId);

                // 2) (Opcional pero recomendado) Marcar inactiva / borrar en tu BD local
                // repo.Cards.MarkInactive(userId, cardId);  // o delete físico

                return StatusCode(HttpStatusCode.NoContent); // 204
            }
            catch (OpenpayException ex)
            {
                // Devuelve detalle útil al cliente
                return Content((HttpStatusCode)ex.HResult,
                    new
                    {
                        error_code = ex.ErrorCode,
                        category = ex.Category,
                        description = ex.Description
                    });
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = e.Message });
            }
        }

        // OPEN PAY.. Aqui voy a meter las operaciones de 


        // CREAR UN USUARIO, GUARDAR ESE ID EN LOGIN.


        // OBTENRE TODAS LAS TARJETAS DE CREDITO DEL USUARIO.

        protected void displayOpenPay(OpenpayException ex)
        {
            string Message = "";
            Console.WriteLine($"HResult: {ex.HResult}");
            Console.WriteLine($"Tipo de excepción: {ex.GetType().Name}");
            if (ex.ErrorCode == 1001)
            {
                Message = "CVV erroneo.";
            }
            else if (ex.ErrorCode == 301)
            {
                Message = "La tarjeta fue rechazada.";
            }
            else if (ex.ErrorCode == 3002)
            {
                Message = "La tarjeta ha expirado.";
            }
            else if (ex.ErrorCode == 3003)
            {
                Message = "La tarjeta no tiene fondos suficientes.";
            }
            else if (ex.ErrorCode == 3004)
            {
                Message = "La tarjeta ha sido identificada como una tarjeta robada.";
            }
            else if (ex.ErrorCode == 3005)
            {
                Message = "La tarjeta ha sido rechazada por el sistema antifraudes.";
            }
            else
            {
                Console.WriteLine(ex.ErrorCode);
            }
            Console.WriteLine(ex.Message);
        }


    }
}
