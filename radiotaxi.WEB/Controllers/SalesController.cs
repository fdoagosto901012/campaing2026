using Amazon.ElasticTranscoder.Model;
using Newtonsoft.Json;
using radiotaxi.Model;
using radiotaxi.Model.v2.Sales;
using radiotaxi.WEB.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Runtime;
using System.Web;
using System.Web.Mvc;

namespace radiotaxi.WEB.Controllers
{
    [RoutePrefix("ticket")]
    public class SalesController : webBaseController
    {
        // GET: Sales
        [Route("box")]
        public ActionResult box()
        {
            string ip  = Request.UserHostAddress;
            ViewBag.ip = ip;
            Servicio servicio = new Servicio();
            ViewBag.services = servicio.get();
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: Sales/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Sales/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Sales/Details
        [Route("detalle/{ticket}")]
        public ActionResult TicketDetails(string ticket)
        {
            Ticket_DTO ticketDTO = new Ticket_DTO();
            List<Ticket_DTO> tickets = ticketDTO.get(ticket);
            decimal total = 0;
            for (int i = 0; i < tickets.Count(); i++) {
                tickets[i].Total = tickets[i].Cant * tickets[i].PrecioConiva;
                total += tickets[i].Total;
            }
            ViewBag.tickets = tickets;
            ViewBag.ticket = ticket;
            ViewBag.total = total;
            return View();
        }

        // POST: Sales/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sales/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sales/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sales/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sales/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Do(int id)
        {
            return View();
        }

        [HttpGet]
        [Route("json/get/cobro/{taxi}/{reference}/")]
        [SessionFilter("Admin")]
        public ActionResult _get(string taxi, string reference)
        {
            try
            {
                Sale sale = new Sale(this.email);
                List<caja_cargos_DTO> Cargos = sale.debs(taxi, reference);
                this.charges = Cargos;
                this.taxi = taxi;
                this.reference = reference;
                return Content(this.serialize(Cargos), "application/json");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("json/pay/")]
        [SessionFilter("Admin")]
        public ActionResult pay(string jsonInput)
        {
            try
            {
                string IP = Request.UserHostAddress;
                // Aqui se contiene el response de la venta
                Sale_Response Response = new Sale_Response();
                // Variable que contiene las deudas por producto.
                Dictionary<string, string> message = new Dictionary<string, string>();
                // Deserialize Input JSON String into target Object Mapper.
                Sale obj = JsonConvert.DeserializeObject<Sale>(jsonInput); 
                // Creamos el objeto ventas para obtener los datos de todas las deudas.
                Sale sale = new Sale(this.email);
                // Obtenemos todas las deudas para evitar modificaciones del front al back.
                List<caja_cargos_DTO> Cargos = sale.debs(obj.taxi, obj.reference);
                // Obtenemos las deudas por productos separados.
                caja_debs_reporte_DTO CargosProducto = obj.loadDebsProduct(obj.taxi,obj.reference);
                // Verificamos que la cantidad de los pagos esten dentro del rangos establecidos.
                foreach (var pay in obj.pays)
                {
                    if (pay.PAGA < 0 && pay.PAGA > pay.DEBE)
                    {
                        message.Add(pay.CARGO,"No puede pagar mas de lo que debe.");
                    }
                }
                // Validamos que no contenta mensajes de error de otra manera regresa error.
                if (message.Count != 0)
                {
                    Response.code = 405; // error en cobro
                    Response.message = message;
                    return Content(this.serialize(Response), "application/json"); // Regresamos que no se puede cobrar por incongruencias.
                }
                //Sale sale = new Sale(this.UserName);
                Ticket_Detail_DTO Ticket = sale.pay(this.email, IP, obj.taxi, obj.reference, obj.pays, CargosProducto);// se pasa los pagos y los productos.
                // Validar pagos.
                Response.code = 201; // error en cobro
                Response.message = message;
                Response.charges = charges;
                Response.pays = obj.pays;
                Response.Ticket = Ticket;
                return Content(this.serialize(Response), "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        [HttpPost]
        [Route("json/pay/detail")]
        [SessionFilter("Admin")]
        public ActionResult detail(string taxi, string reference, string family)
        {
            try
            {
                // Deserialize Input JSON String into target Object Mapper.
                Sale obj = new Sale(this.email);
                Sale_Response Response = new Sale_Response();
                Dictionary<string, string> message = new Dictionary<string, string>();
                List<caja_cargos_DTO> data = obj.getdetails( taxi, reference, family);
                Response.code = 201; // error en cobro
                Response.message = message;
                Response.charges = data;
                Response.pays = data;
                return Content(this.serialize(Response), "application/json");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("json/pay/inventario")]
        [SessionFilter("Admin")]
        public ActionResult getinventario(string reference, string family)
        {
            try
            {
                // Deserialize Input JSON String into target Object Mapper.
                Sale obj = new Sale(this.email);
                Sale_Response Response = new Sale_Response();
                Dictionary<string, string> message = new Dictionary<string, string>();
                Inventario data = obj.GetInventario(reference, family);
                Response.code = 201; // error en cobro
                Response.message = message;
                Response.inventario = data;
                return Content(this.serialize(Response), "application/json");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("print/{Ticket}")]
        [SessionFilter("Admin")]
        public ActionResult PrintTicket(string Ticket)
        {
            // Obtener informacion del ticket.
            return View();
        }

        [HttpGet]
        [Route("price/turn/double/{price}/{type}")]
        [SessionFilter("Admin")]
        public JsonResult pricedouble(decimal price, string type)
        {
            try
            {

                RTYPE rTYPE = new RTYPE();
                decimal priceDouble = 0;
                // Obtener informacion del ticket.
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };

                if (type == "D")
                {
                    priceDouble = rTYPE.getdouble(price);
                    return Json(JsonConvert.SerializeObject(priceDouble, settings), JsonRequestBehavior.AllowGet);
                }
                return Json(JsonConvert.SerializeObject(price, settings), JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                return null;
            }
            
        }


        [HttpGet]
        [Route("bloqueos/{taxi}/{op}")]
        [SessionFilter("Admin")]
        public ActionResult messages(string taxi, string op)
        {
            try
            {
                // Obtenemos los mensajes
                MENSAJE mENSAJE = new MENSAJE();
                List<MENSAJE> messages = new List<MENSAJE>();
                if (taxi == op)
                {
                    messages = mENSAJE.getEco(taxi);
                }
                else { 
                    messages = mENSAJE.getEco(taxi);
                    messages.Concat(mENSAJE.getGafet(op));
                }
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                return Content(this.serialize(messages), "application/json");
            }
            catch (Exception)
            {

                return null;
            }

        }






    }
}