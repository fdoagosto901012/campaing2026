using Microsoft.AspNet.SignalR.Hosting;
using Newtonsoft.Json;
using radiotaxi.API.Controllers.Base;
using radiotaxi.API.Hubs;
using radiotaxi.Model;
using radiotaxi.Model.API;
using radiotaxi.Model.API.cards;
using radiotaxi.Model.DTO;
using radiotaxi.Model.v2.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace radiotaxi.API.Controllers
{
    [Authorize(Roles = "soc")]
    [RoutePrefix("api/creditcards")]
    public class CreditCardsController : BaseControllerWithHub<CreditCardHub>
    {
        // GET: api/CreditCards
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/CreditCards/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [Route("save/card")]
        public HttpResponseMessage Post([FromBody] creditCardDTO _card)
        {
            try
            {
                creditcard card = new creditcard();
                //
                card.userId = _card.userId;
                card.active = true;
                card.token = _card.token;
                card.DateCreate = _card.DateCreate;
                card.Ultimos4 = _card.Ultimos4;
                card.brand = _card.brand;
                card.type = _card.type;
                card.card_number = _card.card_number;
                card.bank_name = _card.bank_name;
                card.customer_id = _card.customer_id;

                if (card.save())
                {
                    return Request.CreateResponse(
                        HttpStatusCode.OK, this.content<creditcard>(card),
                        Configuration.Formatters.JsonFormatter
                    );
                }
                else {
                    return Request.CreateResponse(
                        HttpStatusCode.BadRequest, this.content<string>(""),
                        Configuration.Formatters.JsonFormatter
                    );
                }
            }
            catch (Exception)
            {
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest, this.content<string>(""),
                    Configuration.Formatters.JsonFormatter
                );
            }
        }

        [HttpPost]
        [Route("save/charges")]
        public HttpResponseMessage savecharges(ChargesDTO charges)
        {
            try
            {
                // RELLENA TODOS LOS CAMPOS QUE PIDE QUE SEAN OBLIGATORIOS
                foreach(var item in charges.Items.Select((value, i) => new { i, value }))
                {
                    charges.Items[item.i].PRODUCTO = ""; 
                    charges.Items[item.i].TYPE = ""; 
                    charges.Items[item.i].Id_Producto = ""; 
                    charges.Items[item.i].Descripcion = ""; 
                    charges.Items[item.i].CONVENIO = "";
                    charges.Items[item.i].FechaOp = DateTime.Now;
                }
                charges.save();
                return Request.CreateResponse(
                    HttpStatusCode.OK, this.content<ChargesDTO>(charges),
                    Configuration.Formatters.JsonFormatter
                );
            }
            catch (Exception)
            {
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest, this.content<string>(""),
                    Configuration.Formatters.JsonFormatter
                );
            }
        }

        [HttpGet]
        [Route("get/cards/{userId}")]
        public HttpResponseMessage getCreditcards(int userId)
        {
            try
            {
                creditcard card = new creditcard();
                CardsResponse response = new CardsResponse();
                response.cards = card.get(userId);
                return Request.CreateResponse(
                    HttpStatusCode.OK, this.content<CardsResponse>(response),
                    Configuration.Formatters.JsonFormatter
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest, this.content<string>(""),
                    Configuration.Formatters.JsonFormatter
                );
            }
        }

        [HttpPost]
        [Route("save/client")]
        public HttpResponseMessage saveClient(string tokenid, int userId)
        {
            try
            {
                user User = new user();
                User = User.get(userId);
                User.OpenPayUpdateClientToken(tokenid);
                return Request.CreateResponse(
                    HttpStatusCode.OK, this.content<string>(""),
                    Configuration.Formatters.JsonFormatter
                );
            }
            catch (Exception)
            {
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest, this.content<string>(""),
                    Configuration.Formatters.JsonFormatter
                );
            }
        }

        [HttpGet]
        [Route("save/user/{userId}/{token}")]
        public HttpResponseMessage savetokenuser(string token, int userId)
        {
            try
            {
                user User = new user();
                User = User.get(userId);
                User.openpayClientID = token;
                User.update();
                return Request.CreateResponse(
                    HttpStatusCode.OK, this.content<CardsResponse>(User),
                    Configuration.Formatters.JsonFormatter
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest, this.content<string>(null),
                    Configuration.Formatters.JsonFormatter
                );
            }
        }


        // Este metodo registra la venta en practicontrol...
        [HttpPost]
        [Route("save/pay/register/{partnerReference}")]
        public HttpResponseMessage savepayOnPracticontrol(string partnerReference,[FromBody] PaymentRegisterDTO payment)
        {
            try
            {
                Console.WriteLine("Hola mundo.");
                // Aqui se contiene el response de la venta
                Sale_Response Response = new Sale_Response();
                // Variable que contiene las deudas por producto.
                Dictionary<string, string> message = new Dictionary<string, string>();
                // Obtenemos el objet de venta.
                Sale obj = new Sale(payment.sale.chashcashier);
                obj.taxi = payment.sale.taxi;
                obj.reference = payment.sale.reference;

                List<caja_cargos_DTO> items = new List<caja_cargos_DTO>();
                foreach (var item in payment.sale.pays)
                {
                    caja_cargos_DTO _item = new caja_cargos_DTO();
                    _item.CHECK = item.CHECK;
                    _item.PRODUCTO = item.PRODUCTO;
                    _item.TYPE = item.TYPE;
                    _item.CLAVE = item.CLAVE;
                    _item.CARGO = item.CARGO;
                    _item.IDA = item.IDA;
                    _item.A = item.A;
                    _item.DEBE = item.DEBE;
                    _item.PAGA = item.PAGA;
                    _item.PRECIO = item.PRECIO;
                    _item.PRECIOFIJO = item.PRECIOFIJO;
                    _item.Id_Producto = item.Id_Producto;
                    _item.FechaOp = item.FechaOp;
                    _item.Descripcion = item.Descripcion;
                    _item.CONVENIO = item.CONVENIO;
                    _item.RTYPE = item.RTYPE; // Define el tipo de Reporte D - Doble 
                    _item.RDOUBLE = item.RDOUBLE; // Define el tipo de Reporte D - Doble 
                    _item.MULTIPLICADOR = item.MULTIPLICADOR; // Define el tipo de Reporte D - Doble 
                    items.Add(_item);
                }
                obj.pays = items;
                obj.chashcashier = payment.sale.chashcashier;
                // Creamos el objeto ventas para obtener los datos de todas las deudas.
                Sale sale = new Sale("");
                // Obtenemos todas las deudas para evitar modificaciones del front al back.
                List<caja_cargos_DTO> Cargos = sale.debs(obj.taxi, obj.reference);
                // Obtenemos las deudas por productos separados.
                caja_debs_reporte_DTO CargosProducto = obj.loadDebsProduct(obj.taxi, obj.reference);
                // Verificamos que la cantidad de los pagos esten dentro del rangos establecidos.
                foreach (var pay in obj.pays)
                {
                    if (pay.PAGA < 0 && pay.PAGA > pay.DEBE)
                    {
                        message.Add(pay.CARGO, "No puede pagar mas de lo que debe.");
                    }
                }
                // Validamos que no contenta mensajes de error de otra manera regresa error.
                if (message.Count != 0)
                {
                    Response.code = 405; // error en cobro
                    Response.message = message;
                    return Request.CreateResponse(
                        HttpStatusCode.Conflict, this.content<Sale_Response>(Response),
                        Configuration.Formatters.JsonFormatter
                    );
                }

                //Sale sale = new Sale(this.UserName);
                Ticket_Detail_DTO Ticket = sale.pay(payment.ChargeToken, payment.ChargeToken, obj.taxi, obj.reference, obj.pays, CargosProducto);// se pasa los pagos y los productos.
                // Validar pagos.
                Response.code = 201; // error en cobro
                Response.message = message;

                // OBTENER LOS CARGOS NUEVAMENTE...
                Response.charges = sale.debs(obj.taxi, obj.reference);
                Response.pays = obj.pays;
                Response.Ticket = Ticket;
                return Request.CreateResponse(
                    HttpStatusCode.OK, this.content<Sale_Response>(Response),
                    Configuration.Formatters.JsonFormatter
                );
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(
                    HttpStatusCode.Conflict, this.content<Sale_Response>(null),
                    Configuration.Formatters.JsonFormatter
                );
            }
        }

        // PUT: api/CreditCards/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CreditCards/5
        public void Delete(int id)
        {
        }
    }
}
