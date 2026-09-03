using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using radiotaxi.Model;
using radiotaxi.Model.v2.Partner;
using radiotaxi.WEB.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Amazon.DynamoDBv2;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using Amazon.CloudFormation.Model;
using System.Data.Entity.Core.Metadata.Edm;
using System.Web.Routing;
using ZXing.Aztec.Internal;
using Newtonsoft.Json.Schema;
using System.Runtime;
using Antlr.Runtime;
using Amazon.KeyManagementService;
using radiotaxi.Model.v2.finance;
using ZXing.Common;
using ZXing;
using Amazon.SimpleDB.Model;
using Amazon.OpsWorks.Model;
using System.Threading.Tasks;

using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;

namespace radiotaxi.WEB.Controllers
{
    [SessionFilter("Admin")]
    [RoutePrefix("Partner")]
    public class PartnerController : webBaseController
    {
        

        [SessionFilter("Admin, tarjeton, Practi-AutoS, Practi-Actas,Practi-Cajas, Practi-AdminTurkey, Clinica, Practi-Finanzas, Practi-Emplacamiento, Practi-Plusvalia, Archivo, Practi-Trabajo, Practi-Viewer, Practi-ViewerSOC, Practi-Cred, Practi-BasicView, Practi-Tte")]
        public ActionResult Index(int? page)
        {
            if (this.PartnerSearchParameters == null) this.PartnerSearchParameters = new PartnerSearchParametersDTO();
            ViewBag.parameters = this.PartnerSearchParameters; // Objeto que contiene los parametros de busqueda, estos se guardan en session pero estransparante para la capa del controlador.
            Partner Partner = new Partner();
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            PartnerPagination PartnerPagination = Partner.get(pageNumber, pageSize, this.PartnerSearchParameters.gafet, this.PartnerSearchParameters.name, this.PartnerSearchParameters.lastName1, this.PartnerSearchParameters.lastName2);
            // Obtener la imagen para N operadores.
            ViewBag.totalClients = PartnerPagination.TotalItems;
            return View(PartnerPagination);
        }

        [SessionFilter("Admin,tarjeton,Practi-AutoS,Practi-Actas,Practi-Cajas,Practi-AdminTurkey, Clinica,Practi-Finanzas,Practi-Emplacamiento, Practi-Plusvalia, Archivo, Practi-Trabajo, Practi-Viewer, Practi-ViewerSOC, Practi-Cred, Practi-BasicView, Practi-Tte")]
        [HttpPost]
        public ActionResult Index(int? page, string gafet, string name, string lastName1, string lastName2)
        {
            try
            {
                this.PartnerSearchParameters = new PartnerSearchParametersDTO();
                this.PartnerSearchParameters.gafet = gafet; // Este es el gafet de operador
                this.PartnerSearchParameters.name = name;
                this.PartnerSearchParameters.lastName1 = lastName1;
                this.PartnerSearchParameters.lastName2 = lastName2;
                ViewBag.parameters = this.PartnerSearchParameters;
                Partner Partner = new Partner();
                int statusConnection = 0;
                int pageSize = 100;
                int pageNumber = (page ?? 1);
                PartnerPagination clientPagination = Partner.get(pageNumber, pageSize, gafet, name, lastName1, lastName2);
                ViewBag.totalClients = clientPagination.TotalItems;
                return View(clientPagination);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [Route("low/details/{id}")]
        [SessionFilter("Admin,tarjeton")]
        public ActionResult lowPartner(int id)
        {
            if (this.PartnerSearchParameters == null) this.PartnerSearchParameters = new PartnerSearchParametersDTO();
            ViewBag.parameters = this.PartnerSearchParameters; // Objeto que contiene los parametros de busqueda, estos se guardan en session pero estransparante para la capa del controlador.
            Partner Partner = new Partner();
            int? page = 0;
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            PartnerPagination PartnerPagination = Partner.get(pageNumber, pageSize, this.PartnerSearchParameters.gafet, this.PartnerSearchParameters.name, this.PartnerSearchParameters.lastName1, this.PartnerSearchParameters.lastName2);
            // Obtener la imagen para N operadores.
            foreach (var (_Partner, index) in PartnerPagination.Data.Select((v, i) => (v, i)))
            {
                try
                {
                    PartnerPagination.Data[index].urlImage = AmazonHelper.getImageUser(_Partner.bucket, _Partner.key);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            ViewBag.totalClients = PartnerPagination.TotalItems;
            return View(PartnerPagination);
        }

        // GET: Client
        [SessionFilter("Admin,tarjeton")]
        public ActionResult low(int? page)
        {
            Operadores_Baja_Datos OP = new Operadores_Baja_Datos();
            if (this.PartnerSearchParameters == null) this.PartnerSearchParameters = new PartnerSearchParametersDTO();
            ViewBag.parameters = this.PartnerSearchParameters; // Objeto que contiene los parametros de busqueda, estos se guardan en session pero estransparante para la capa del controlador.
            Partner Partner = new Partner();
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            PartnerPagination PartnerPagination = Partner.get(pageNumber, pageSize, this.PartnerSearchParameters.gafet, this.PartnerSearchParameters.name, this.PartnerSearchParameters.lastName1, this.PartnerSearchParameters.lastName2);
            // Obtener la imagen para N operadores.
            foreach (var (_Partner, index) in PartnerPagination.Data.Select((v, i) => (v, i)))
            {
                try
                {
                    PartnerPagination.Data[index].urlImage = AmazonHelper.getImageUser(_Partner.bucket, _Partner.key);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            ViewBag.totalClients = PartnerPagination.TotalItems;
            return View(PartnerPagination);
        }

        [HttpPost]
        [SessionFilter("Admin,tarjeton")]
        public ActionResult low(int? page, string gafet, string name, string lastName1, string lastName2)
        {
            this.PartnerSearchParameters = new PartnerSearchParametersDTO();
            this.PartnerSearchParameters.gafet = gafet; // Este es el gafet de operador
            this.PartnerSearchParameters.name = name;
            this.PartnerSearchParameters.lastName1 = lastName1;
            this.PartnerSearchParameters.lastName2 = lastName2;
            ViewBag.parameters = this.PartnerSearchParameters;
            Partner Partner = new Partner();
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            PartnerPagination clientPagination = Partner.get(pageNumber, pageSize, gafet, name, lastName1, lastName2);
            // Obtener la imagen para N operadores.
            foreach (var (_Partner, index) in clientPagination.Data.Select((v, i) => (v, i)))
            {
                try
                {
                    clientPagination.Data[index].urlImage = AmazonHelper.getImageUser(_Partner.bucket, _Partner.key);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            ViewBag.totalClients = clientPagination.TotalItems;
            return View(clientPagination);
        }

        [SessionFilter("Admin,tarjeton,Practi-AutoS,Practi-Actas,Practi-Cajas,Practi-AdminTurkey, Clinica,Practi-Finanzas, Practi-Plusvalia, Archivo, Practi-Trabajo, Practi-Viewer, Practi-ViewerSOC, Practi-Cred, Practi-BasicView, Practi-Tte")]
        [HttpGet]
        [Route("tarjeton/{gafet}")]
        public async Task<ActionResult> tarjeton(string gafet)
        {
            try
            {
                Partner _Partner = new Partner();
                _Partner = _Partner.get(gafet);
                var imageTask = Task.Run(() => getImage(_Partner)); // Obtenemos la tarea para obtener la imagen.
                var CardsTask = Task.Run(() => getCards(gafet)); // Obtenemos la tarea para obtener la imagen.
                var ReportsTask = Task.Run(() => getReports(_Partner));
                var MessageTask = Task.Run(() => getMessage(gafet)); // Obtenemos la tarea para obtener la imagen.
                var financesTask = Task.Run(() => getFinances(_Partner)); // Obtenemos la tarea para obtener la imagen.
                var debsTask = Task.Run(() => getDebs(gafet)); // Obtenemos la tarea para obtener la imagen.
                var paysTask = Task.Run(() => getPays(_Partner)); // Obtenemos la tarea para obtener la imagen.
                var AgrementsTask = Task.Run(() => getAgrements(_Partner)); // Obtenemos la tarea para obtener la imagen.
                var SalesTask = Task.Run(() => getSales(_Partner));
                var EmplacameintoTask = Task.Run(() => getEmplacameinto(gafet));

                // Obtenemos los pagos de cupones para el operador.
                var CuponspaymentsTask = Task.Run(() => getCuponsPayments(_Partner.userId));

                await Task.WhenAll(imageTask, CardsTask, ReportsTask, MessageTask, financesTask, debsTask, paysTask, AgrementsTask, SalesTask, EmplacameintoTask, CuponspaymentsTask);
                _Partner.urlImage = imageTask.Result; // Asignamos la imagen.
                ViewBag.cars = CardsTask.Result;
                ViewBag.reportes = ReportsTask.Result;
                ViewBag.message = MessageTask.Result;
                ViewBag.financesOperator = financesTask.Result;
                ViewBag.Porcentaje = financesTask.Result.porcent;
                ViewBag.faltas = financesTask.Result.faltas;
                ViewBag.pays = paysTask.Result;
                ViewBag.agreements = AgrementsTask.Result;
                ViewBag.sales = SalesTask.Result;
                // Lista de cupones.
                ViewBag.Cuponspayments = CuponspaymentsTask.Result;
                List<Emplacamiento> emplacados = EmplacameintoTask.Result.OrderBy(x => x.FechaOp).ToList<Emplacamiento>();
                foreach (Emplacamiento item in emplacados)
                {
                    EmplaMarca emplaMarca = db.EmplaMarcas.Where(x => x.Id_Marca == item.Id_Marca).FirstOrDefault<EmplaMarca>();
                    EmplaModelo emplaModelo = db.EmplaModeloes.Where(x => x.Id_Modelo == item.Id_Modelo).FirstOrDefault<EmplaModelo>();
                    item.Marca = emplaMarca.Marca;
                    item.Modelo = emplaModelo.Modelo;
                }
                Emplacamiento emplacadoAc = emplacados.Where(x => x.Operacion == "EMPLACADO").FirstOrDefault();
                ViewBag.emplaactual = emplacadoAc is null ? emplacados.FirstOrDefault() : emplacadoAc;
                List<caja_cargos_DTO> debs = debsTask.Result;
                decimal amountdebs = 0;
                foreach (caja_cargos_DTO deb in debs)
                {
                    if (deb.CLAVE != "0")
                    {
                        amountdebs += (deb.DEBE * deb.PRECIO);
                    }
                }
                ViewBag.debs = debs;
                ViewBag.amountdebs = amountdebs;
                // PAVOS
                Christmasgift christmasgift = new Christmasgift();
                christmasgift = christmasgift.get(_Partner.userId);
                ViewBag.christmasgift = null;
                ViewBag.christmasgiftPrinted = new List<ChristmasgiftPrinted>();
                if (christmasgift != null)
                {
                    if (christmasgift.DeliveryDate != null)
                    {
                        ViewBag.christmasgift = christmasgift;
                    }
                    ViewBag.christmasgiftPrinted = christmasgift.ChristmasgiftPrinteds;
                }
                ViewBag.maxdeb = maxdeb;
                return View(_Partner);
            }
            catch (Exception ex)
            {
                // ENVIAR A LA VISTA DE ERROR (GERMAN)
                return Redirect("/Error/EcoSoc");
            }
        }

        private List<Emplacamiento> getEmplacameinto(string gafet)
        {
            try
            {
                Emplacamiento emplacamiento = new Emplacamiento();
                return emplacamiento.getHistory(gafet);
            }
            catch (Exception ex)
            {
                return new List<Emplacamiento>();
            }
        }


        private List<CouponPayment> getCuponsPayments(int id)
        {
            try
            {
                CouponPayment couponPayment = new CouponPayment();
                return couponPayment.getHistory(id);
            }
            catch (Exception ex)
            {
                return new List<CouponPayment>();
            }
        }

        private List<Venta> getSales(Partner _Partner)
        {
            try
            {
                return _Partner.sales();
            }
            catch (Exception ex)
            {
                return new List<Venta>();
            }
        }

        private List<Convenio> getAgrements(Partner _Partner)
        {
            try
            {
                return _Partner.agreements();
            }
            catch (Exception ex)
            {

                return new List<Convenio>();
            }
        }

        private List<pay> getPays(Partner _Partner)
        {
            try
            {
                return _Partner.payCards();
            }
            catch (Exception ex)
            {
                return new List<pay>();
            }
        }
        private List<caja_cargos_DTO> getDebs(string gafet)
        {
            try
            {
                // DEUDAS
                Sale sale = new Sale(this.email);
                return sale.debs(gafet, gafet);
            }
            catch (Exception ex)
            {
                return new List<caja_cargos_DTO>();
            }
        }
        private financesOperator getFinances(Partner partner)
        {
            try
            {
                return partner.financesPartner();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private List<MENSAJE> getMessage(string gafet)
        {
            try
            {
                // Obtenemos los mensajes
                MENSAJE mENSAJE = new MENSAJE();
                return mENSAJE.getEco(gafet);
            }
            catch (Exception ex)
            {
                return new List<MENSAJE>();
            }
        }
        private List<HistAsistencia> getReports(Partner _Partner)
        {
            try
            {
                // Obtenemos los reportes 
                List<HistAsistencia> _reportes = _Partner.reportes();
                List<HistAsistencia> reportes = new List<HistAsistencia>();

                string _color = this.getRandColor();
                string oldticket = "";
                foreach (var reporte in _reportes)
                {
                    if (oldticket != reporte.Ticket)
                    {
                        _color = this.getRandColor();
                    }
                    reporte.color = _color;
                    reportes.Add(reporte);
                    oldticket = reporte.Ticket;
                }
                return reportes;
            }
            catch (Exception ex)
            {
                return new List<HistAsistencia>();
            }
        }
        private List<card> getCards(string gafet)
        {
            try
            {
                // Obtenemos todos los tarjetons del operador.
                card _card = new card();
                List<card> _cards = _card.get(gafet.ToString());
                return _cards; //Enviamos la lista por medio de un viewBag.
            }
            catch (Exception ex)
            {
                return new List<card>();
            }
        }
        private string getImage(Partner _Partner)
        {
            try
            {
                if (_Partner != null && _Partner.bucket != null && _Partner.key != null)
                {
                    return AmazonHelper.getImageUser(_Partner.bucket, _Partner.key);
                }
                return "";

            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [SessionFilter("Admin")]
        [HttpGet]
        [Route("cuotas/sindicales/{gafet}")]
        public ActionResult cancelarcuotas(string gafet)
        {
            try
            {
                Partner _Partner = new Partner();
                // Obtenemos todos los tarjetons del operador.
                _Partner = _Partner.get(gafet);
                if (_Partner != null && _Partner.bucket != null && _Partner.key != null)
                {
                    _Partner.urlImage = AmazonHelper.getImageUser(_Partner.bucket, _Partner.key);
                }
                List<Inventario> cuotas = _Partner.getinventario(12); // la familia 12 es de cuotas sindicatles
                decimal amount = 0;
                foreach (var cuota in cuotas)
                {
                    amount += cuota.PrecioConIva;
                }
                ViewBag.dues = cuotas;
                ViewBag.amount = amount;
                ViewBag.total = cuotas.Count();
                return View(_Partner);
            }
            catch (Exception ex)
            {
                // ENVIAR A LA VISTA DE ERROR (GERMAN)
                throw;
            }
        }

        [SessionFilter("Admin")]
        [HttpPost]
        [Route("cuotas/sindicales/{gafet}")]
        public ActionResult cancelarcuotas(FormCollection data, string gafet)
        {
            try
            {
                string EditBy = this.User.firstName + " " + this.User.lastName;
                int cant = Convert.ToInt32(data["cant"]);
                string autoriza = Convert.ToString(data["autoriza"]);
                string movimiento = Convert.ToString(data["movimiento"]);
                Partner _Partner = new Partner();
                // Obtenemos todos los tarjetons del operador.
                _Partner = _Partner.get(gafet);
                List<Inventario> cuotas = _Partner.getinventario(12); // la familia 12 es de cuotas sindicatles
                _Partner.cancelarcuotas(cant, movimiento, autoriza, EditBy);
                ViewBag.dues = cuotas;
                return Redirect("~/Partner/cuotas/sindicales/" + _Partner.partnerReference);
            }
            catch (Exception ex)
            {
                // ENVIAR A LA VISTA DE ERROR (GERMAN)
                throw;
            }
        }

        // GET: Partner/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [SessionFilter("Admin, Practi-Cred")]
        // GET: Partner/Create
        public ActionResult Create()
        {
            ViewBag.partnerTypes = new partnerType().get();
            ViewBag.relationships = new relationShipStatu().get();
            ViewBag.states = new state().get();
            ViewBag.cities = new city().get();
            return View();
        }

        // POST: Partner/Create
        [SessionFilter("Admin, Practi-Cred")]
        [HttpPost]
        public ActionResult Create(Partner Obj, HttpPostedFileBase file)
        {
            try
            {
                Obj.createdBy = this.UserName;
                Obj.createdDt = DateTime.Now;
                Obj.active = true;
                Partner partner = Obj.save(); // Se guarda en la base de datos... 
                // Actualizacion de foto
                if (file != null)
                {
                    try
                    {
                        if (file.ContentLength > 0)
                        {
                            amazonPicture AmazonPicture = AmazonHelper.savePictureWebSite(partner.userId, partner.partnerReference, file);
                            if (AmazonPicture != null)
                            {
                                AmazonPicture.userId = partner.userId; // Seteamos a quien le pertenece esa imagen.
                                AmazonPicture.Message = "";
                                AmazonPicture.DtCreated = DateTime.Now;
                                AmazonPicture.pictureTypeId = 2;
                                // Como se crea uno nuevo simplemente se acutaliza el objeto.
                                if (partner.amazonPictures.Where(x => x.pictureTypeId == 2).FirstOrDefault() != null)
                                {
                                    amazonPicture a = partner.amazonPictures.Where(x => x.pictureTypeId == 2).FirstOrDefault();
                                    a.key = AmazonPicture.key;
                                    a.bucket = AmazonPicture.bucket;
                                    a.update();
                                }
                                else
                                {
                                    AmazonPicture.save();
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ViewBag.errorImage = e.Message;
                    }
                }
                if (partner == null)
                {
                    return Redirect("~/Error/");
                }
                return Redirect("~/Partner/tarjeton/" + Obj.partnerReference);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        // POST: Partner/GetCities
        [HttpPost]
        public JsonResult GetCities(int id)
        {
            try
            {
                List<city> cities = new city().get(id);
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(cities, settings);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (JsonReaderException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonSerializationException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonWriterException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonSchemaException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // POST: Partner/GetCities
        [HttpPost]
        public JsonResult RemoveReports(string eco, string _user)
        {
            try
            {
                Partner partner = new Partner();
                partner.RemoveReports(eco, _user); // Removemos todas las faltas de ese usuarios.
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (JsonReaderException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonSerializationException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonWriterException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonSchemaException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET: Partner/Edit/5
        [SessionFilter("Admin, tarjeton, Practi-Cred")]
        [Route("Edit/{gafet}")]
        public ActionResult Edit(string gafet)
        {
            userAddress userAddress = null;
            List<city> cities = new city().get();
            int stateId = 0;
            int cityId = 0;
            Partner _Partner = new Partner();
            _Partner = _Partner.get(gafet);
            try
            {
                userAddress = _Partner.userAddresses.FirstOrDefault();
                if (userAddress != null)
                {
                    // Obtenemos la ciudad.
                    cityId = (int)userAddress.cityId;
                    city City = new city().getcity(cityId);
                    cities = City.get(City.stateId);
                    stateId = City.stateId;
                }
            }
            catch (Exception ex)
            {
                userAddress = null;
            }
            if (_Partner != null && _Partner.bucket != null && _Partner.key != null)
            {
                _Partner.urlImage = AmazonHelper.getImageUser(_Partner.bucket, _Partner.key);
            }
            ViewBag.cities = cities;
            ViewBag.stateId = stateId;
            ViewBag.cityId = cityId;
            ViewBag.userAddress = userAddress;
            ViewBag.partnerTypes = new partnerType().get();
            ViewBag.relationships = new relationShipStatu().get();
            ViewBag.states = new state().get();
            return View(_Partner);
        }

        // POST: Partner/Edit/5
        [HttpPost]
        [SessionFilter("Admin,tarjeton, Practi-Cred")]
        [Route("Edit/{gafet}")]
        public ActionResult Edit(int gafet, Partner Obj, HttpPostedFileBase file)
        {
            try
            {
                Partner partner = Obj.update(); // Actualizamos la informacion
                List<city> cities = new city().get();
                int stateId = 0;
                int cityId = 0;
                if (partner != null && partner.bucket != null && partner.key != null)
                {
                    partner.urlImage = AmazonHelper.getImageUser(partner.bucket, partner.key);
                }
                // Actualizacion de foto
                if (file != null)
                {
                    try
                    {
                        if (file.ContentLength > 0)
                        {
                            amazonPicture AmazonPicture = AmazonHelper.savePictureWebSite(partner.userId, partner.partnerReference, file);
                            if (AmazonPicture != null)
                            {
                                AmazonPicture.userId = partner.userId; // Seteamos a quien le pertenece esa imagen.
                                AmazonPicture.Message = "";
                                AmazonPicture.DtCreated = DateTime.Now;
                                AmazonPicture.pictureTypeId = 2;
                                // Como se crea uno nuevo simplemente se acutaliza el objeto.
                                if (partner.amazonPictures.Where(x => x.pictureTypeId == 2).FirstOrDefault() != null)
                                {
                                    amazonPicture a = partner.amazonPictures.Where(x => x.pictureTypeId == 2).FirstOrDefault();
                                    a.key = AmazonPicture.key;
                                    a.bucket = AmazonPicture.bucket;
                                    a.update();
                                }
                                else
                                {
                                    AmazonPicture.save();
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ViewBag.errorImage = e.Message;
                    }
                }

                ViewBag.cities = cities;
                ViewBag.stateId = stateId;
                ViewBag.cityId = cityId;
                ViewBag.partnerTypes = new partnerType().get();
                ViewBag.relationships = new relationShipStatu().get();
                ViewBag.states = new state().get();
                ViewBag.message = "Guardado correctamente";
                return Redirect("~/Partner/tarjeton/" + Obj.partnerReference);
            }
            catch
            {
                return null;
            }
        }

        // GET: Partner/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Partner/Delete/5
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

        [SessionFilter("Admin,tarjeton")]
        [HttpPost]
        public ActionResult GeneratePdfDef(string gefet)
        {
            try
            {
                Partner _Partner = new Partner().get(gefet); // Obtenemos la informacion del operador.
                // Generar QR de tarjeton.
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        [HttpPost]
        public ActionResult SearchCar(string gafet)
        {
            try
            {
                EmplacamientoDTO _emplacamiento = new Emplacamiento().get(gafet);
                return Content(this.serialize(_emplacamiento), "application/json");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]

        public ActionResult SearchCarCreate(string taxi, string gafet)
        {
            try
            {
                // Iinformacion del taxi
                EmplacamientoDTO _emplacamiento = new Emplacamiento().get(taxi);
                // Obtenemos la informacion del operador.
                Partner _Partner = new Partner().get(gafet);
                Dictionary<string, object> JsonObj = new Dictionary<string, object>();
                try
                {
                    if (_Partner.bucket != null)
                    {
                        // Obtener la imagen.
                        _Partner.urlImage = AmazonHelper.getImageUser(_Partner.bucket, _Partner.key);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                JsonObj.Add("Car", _emplacamiento);
                JsonObj.Add("Partner", _Partner);
                return Content(this.serialize(JsonObj), "application/json");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult SaveCard(string taxi, string op, int turn)
        {
            try
            {
                Partner _Partner = new Partner();
                _Partner = _Partner.get(op);
                EmplacamientoDTO emplacamientoDTO = new Emplacamiento().get(taxi);// Obtenemos los datos el carro.
                card Card = new card();
                Card.turn = turn;
                Card.deliver = false;
                Card.low = false;
                Card.expiration_date = DateTime.Now;
                Card.asgnedBy = this.UserName;
                Card.asignedDate = DateTime.Now;
                Card.isActive = true;
                Card.userId = _Partner.userId; // El id del padron
                Card.partnerReference = op.ToString(); // El numero de gafet del operador
                Card.note = "NOTA PENDIENTE";
                Card.taxi = taxi;
                Card.Save(); // Se guarda aqui el objeto en la base de datos.
                return Content(this.serialize(Card), "application/json");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /////////
        [HttpGet]
        [SessionFilter("Admin,tarjeton,autoseguro")]
        [Route("generatepdf/")]
        public ActionResult generatepdf()
        {
            try
            {
                Document document = new Document();
                MigraDoc.DocumentObjectModel.Section section = document.AddSection();
                section.PageSetup.Orientation = Orientation.Portrait;
                section.PageSetup.PageHeight = "21.59cm";
                section.PageSetup.PageWidth = "13.97cm";

                Image myImage = section.Headers.Primary.AddImage(Server.MapPath("~/Content/images/TEST2.png"));
                myImage.Height = "21.59cm";
                myImage.Width = "13.97cm";
                myImage.RelativeVertical = RelativeVertical.Page;
                myImage.RelativeHorizontal = RelativeHorizontal.Page;
                myImage.WrapFormat.Style = WrapStyle.Through;

                // Fecha
                TextFrame tf = section.AddTextFrame();
                string txt = "Cancún, Quintana Roo a 19 de julio de 2024";
                tf.Width = "138mm";
                tf.Height = "50mm";
                tf.RelativeVertical = RelativeVertical.Page;
                tf.RelativeHorizontal = RelativeHorizontal.Page;
                tf.Top = "3.5cm";
                tf.Left = "1cm";
                Paragraph paragraph = tf.AddParagraph();
                paragraph.Format.Font.Size = new Unit(36);
                paragraph.AddText(txt.ToUpper());
                paragraph.Format.Font.Name = "Impact";
                paragraph.Format.Font.Bold = true;
                paragraph.Format.SpaceAfter = "0.2cm";
                paragraph.Format.Font.Color = Colors.Red;


                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
                // Associate the MigraDoc document with a renderer
                pdfRenderer.Document = document;
                // Layout and render document to PDF
                pdfRenderer.RenderDocument();

                // Send PDF to browser                
                using (MemoryStream stream = new MemoryStream())
                {
                    pdfRenderer.Save(stream, false);
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", stream.Length.ToString());
                    Response.BinaryWrite(stream.ToArray());
                    Response.Flush();
                    stream.Position = 0;
                    stream.Close();
                    Response.End();
                    return File(stream, "application/pdf", "Test.pdf");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // ENVIAR A LA VISTA DE ERROR (GERMAN)
                throw;
            }
        }






        [HttpGet]
        [Route("register/{gafet}")]
        public ActionResult GeneratePassword(string gafet)
        {
            ViewBag.exist = false;
            ViewBag.userlog = null;

            ViewBag.email = "";
            ViewBag.phone = "";
            ViewBag.password = "";
            ViewBag.error = "";

            SMTPconnection clientSMTP = new SMTPconnection();
            int statusConnection = 0;
            Partner _Partner = new Partner();
            _Partner = _Partner.get(gafet);// Obtenemos al socio.
            @ViewBag.User = _Partner;
            userlog userlog = _Partner.userlog();
            if (userlog != null)
            {
                ViewBag.exist = true;
                ViewBag.userlog = userlog;
                ViewBag.email = userlog.email;
                ViewBag.phone = userlog.phone;
            }
            return View();
        }

        [HttpPost]
        [Route("register/{gafet}")]
        public ActionResult GeneratePassword(FormCollection collection, string gafet)
        {
            int role = 0;
            SMTPconnection clientSMTP = new SMTPconnection();
            int statusConnection = 0;
            Partner _Partner = new Partner();
            _Partner = _Partner.get(gafet); // Obtenemos al socio.
            ViewBag.exist = false;
            ViewBag.User = _Partner;
            ViewBag.id = _Partner.userId;

            RegisterViewModel registerData = new RegisterViewModel()
            {
                Lastname = _Partner.lastNameF + " " + _Partner.lastNameM,
                Name = User.firstName,
                Email = collection["Email"],
                Password = collection["Password"],
                CountryCode = "+52",
                PhoneNumber = collection["Phone"],
                IdPadron = _Partner.userId
            };

            // Definimos el tipo de Role
            if (_Partner.partnerTypeId == 2)
            {
                registerData.roleID = 18;
            }
            else if (_Partner.partnerTypeId == 1)
            {
                registerData.roleID = 20;
            }
            else
            {
                registerData.roleID = 17;
            }
            registerData = _Partner.generatePassword(registerData);
            if (registerData.success)
            {
                // Redireccionar a detalles del socio.
                return Redirect("~/Partner/tarjeton/" + _Partner.partnerReference);
            }
            else
            {
                ViewBag.email = registerData.Email;
                ViewBag.phone = registerData.PhoneNumber;
                ViewBag.password = collection["Password"];
                ViewBag.error = registerData.error;
            }
            return View();
        }

        [HttpGet]
        [Route("register/edit/{gafet}")]
        public ActionResult GeneratePasswordEDIT(string gafet)
        {
            ViewBag.exist = false;
            ViewBag.userlog = null;

            ViewBag.email = "";
            ViewBag.phone = "";
            ViewBag.password = "";
            ViewBag.error = "";
            ViewBag.id = 0;

            SMTPconnection clientSMTP = new SMTPconnection();
            int statusConnection = 0;
            Partner _Partner = new Partner();
            _Partner = _Partner.get(gafet);// Obtenemos al socio.
            @ViewBag.User = _Partner;
            userlog userlog = _Partner.userlog();
            if (userlog != null)
            {
                ViewBag.exist = false;
                ViewBag.userlog = userlog;
                ViewBag.email = userlog.email;
                ViewBag.phone = userlog.phone;
                ViewBag.userId = userlog.userId; // ENVIAMOS EL ID DE USER LOG.
            }
            return View();
        }

        [HttpPost]
        [Route("register/edit/{gafet}")]
        public ActionResult GeneratePasswordEDIT(FormCollection collection, string gafet)
        {
            // values
            int id = 0;
            userlog User = new userlog(); // Declaramos userlogs
            SMTPconnection clientSMTP = new SMTPconnection();
            Partner _Partner = new Partner();
            // OBTENEMOS DATOS BASICOS VIEWBAGS
            ViewBag.User = null;
            ViewBag.exist = false;
            ViewBag.userlog = null;
            ViewBag.email = "";
            ViewBag.phone = "";
            ViewBag.password = "";
            ViewBag.error = "";
            ViewBag.id = id = Int32.Parse(collection["userId"]);
            User = User.get(id);
            _Partner = _Partner.get(gafet); // Obtenemos al socio.

            // Actualizamos
            User.createdDt = DateTime.Now;
            User.email = collection["Email"];
            User.password = collection["Password"];
            User.isActive = true;
            User.phone = collection["Phone"];
            User.RoleID = 20;
            User.removePassword = true; // Removemos la contraseña:
            // Actualizamos los datos de usuario.
            if (User.update())
            {
                // Redireccionar a detalles del socio.
                return Redirect("~/Partner/tarjeton/" + _Partner.partnerReference);
            }
            return View();
        }


        /// <summary>
        ///     SECCION DE DESBLOQUEO.
        /// </summary>
        /// <param name="gafet"></param>
        /// <returns></returns>

        [SessionFilter("Admin,Archivo, Practi-AutoS")]
        [HttpGet]
        [Route("bloqueos/{gafet}")]
        public ActionResult Bloqueos(string gafet)
        {
            Partner _partner = new Partner();
            _partner = _partner.get(gafet);
            MENSAJE message = new MENSAJE();
            ViewBag.Desbloqueos = new List<desbloqueo>();
            ViewBag.messages = new List<MENSAJE>();

            if (_partner != null)
            {
                ViewBag.messages = message.getEco(gafet); // Obtenemos los mensajes por numero economico.
                ViewBag.Desbloqueos = message.getDesbloqueosEco(gafet); // Obtenemos los bloquoes por medio del economico.
            }
            if (_partner != null && _partner.bucket != null && _partner.key != null)
            {
                _partner.urlImage = AmazonHelper.getImageUser(_partner.bucket, _partner.key);
            }
            return View(_partner);
        }

        [SessionFilter("Admin,Archivo, Practi-AutoS")]
        [HttpPost]
        [Route("bloqueos/soc/{gafet}")]
        public ActionResult Bloqueos(string gafet, FormCollection collection)
        {
            MENSAJE message = new MENSAJE();
            Partner _partner = new Partner();

            string detener = collection["detener"];
            string Message = collection["mensaje"];
            string blockto = collection["blockto"];
            try
            {
                _partner = _partner.get(gafet);
                // Obtenemos las secretarias.
                List<SecretariasforMensaje> secretarias = message.Secretarias();
                string id_Secretaria = "";// Insertar por secretaria.
                string secretaria = "";// Insertar por secretaria.
                foreach (var item in secretarias)
                {
                    if (this.User.id_secretaria == item.Id_Ubicacion)
                    {
                        id_Secretaria = item.Id_Ubicacion;
                        secretaria = item.SecretariaUbicacion;
                    }
                }
                if (id_Secretaria == "")
                {
                    id_Secretaria = "0";
                    secretaria = this.User.Role.ToUpper();
                }
                if (blockto == "taxi")
                {
                    message.bloquearTaxi(gafet, Message, this.User.firstName + " " + this.User.lastName, secretaria, detener, id_Secretaria);
                }
                else { 
                    message.bloquearSoc(gafet, Message, this.User.firstName + " " + this.User.lastName, secretaria, detener, id_Secretaria);
                }
            }
            catch (Exception ex)
            {
                // REDIRECCIONAR A PAGINA DE ERROR.
            }

            _partner = _partner.get(gafet);
            ViewBag.Desbloqueos = new List<desbloqueo>();
            ViewBag.messages = new List<MENSAJE>();

            if (_partner != null)
            {
                ViewBag.messages = message.getGafet(gafet); // Obtenemos los mensajes.
                ViewBag.Desbloqueos = message.Desbloqueos(gafet);
            }
            if (_partner != null && _partner.bucket != null && _partner.key != null)
            {
                _partner.urlImage = AmazonHelper.getImageUser(_partner.bucket, _partner.key);
            }
            return Redirect("~/Partner/Bloqueos/" + gafet);

        }

        [HttpPost]
        [Route("desbloquear/{gafet}/{folio}")]
        public ActionResult Desbloquear(string folio, string gafet)
        {

            MENSAJE message = new MENSAJE();
            Operator _operator = new Operator();
            try
            {
                _operator = _operator.get(gafet);
                // Obtenemos las secretarias.
                List<SecretariasforMensaje> secretarias = message.Secretarias();
                string id_Secretaria = "";// Insertar por secretaria.
                string secretaria = "";// Insertar por secretaria.
                foreach (var item in secretarias)
                {
                    if (this.User.id_secretaria == item.Id_Ubicacion)
                    {
                        id_Secretaria = item.Id_Ubicacion;
                        secretaria = item.SecretariaUbicacion;
                    }
                }
                if (id_Secretaria == "")
                {
                    id_Secretaria = "0";
                    secretaria = this.User.Role.ToUpper();
                }
                bool response = message.DesbloquearOP(folio, _operator.partnerReference, this.User.firstName + " " + this.User.lastName, secretaria);
                Dictionary<string, object> JsonObj = new Dictionary<string, object>();
                JsonObj.Add("response", response);
                return Content(this.serialize(JsonObj), "application/json");
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        ////// 100%
        [SessionFilter("Admin")]
        [HttpGet]
        [Route("100/{gafet}")]
        public async Task<ActionResult> cien(string gafet)
        {
            try
            {
                Partner _Partner = new Partner();
                // Obtenemos todos los tarjetons del operador.
                _Partner = _Partner.get(gafet);
                if (_Partner != null && _Partner.bucket != null && _Partner.key != null)
                {
                    _Partner.urlImage = AmazonHelper.getImageUser(_Partner.bucket, _Partner.key);
                }


                var EmplacameintoTask = Task.Run(() => getEmplacameinto(gafet));
                await Task.WhenAll( EmplacameintoTask);
               
                List<Emplacamiento> emplacados = EmplacameintoTask.Result.OrderBy(x => x.FechaOp).ToList<Emplacamiento>();
                foreach (Emplacamiento item in emplacados)
                {
                    EmplaMarca emplaMarca = db.EmplaMarcas.Where(x => x.Id_Marca == item.Id_Marca).FirstOrDefault<EmplaMarca>();
                    EmplaModelo emplaModelo = db.EmplaModeloes.Where(x => x.Id_Modelo == item.Id_Modelo).FirstOrDefault<EmplaModelo>();
                    item.Marca = emplaMarca.Marca;
                    item.Modelo = emplaModelo.Modelo;
                }
                Emplacamiento emplacadoAc = emplacados.Where(x => x.Operacion == "EMPLACADO").FirstOrDefault();
                ViewBag.emplaactual = emplacadoAc is null ? emplacados.FirstOrDefault() : emplacadoAc;

                return View(_Partner);
            }
            catch (Exception ex)
            {
                // ENVIAR A LA VISTA DE ERROR (GERMAN)
                throw;
            }
        }


        [SessionFilter("Admin")]
        [HttpPost]
        [Route("cien/{gafet}")]
        public async Task<ActionResult> cienAsync(FormCollection data, string gafet)
        {
            try
            {
                string EditBy = this.User.firstName + " " + this.User.lastName;
                int cant = Convert.ToInt32(data["cant"]);
                string autoriza = Convert.ToString(data["autoriza"]);
                string movimiento = Convert.ToString(data["movimiento"]);
                Partner _Partner = new Partner();
                // Obtenemos todos los tarjetons del operador.
                _Partner = _Partner.get(gafet);
                List<Inventario> cuotas = _Partner.getinventario(12); // la familia 12 es de cuotas sindicatles
                var response = await _Partner.cien(autoriza, EditBy);
                return Redirect("~/Partner/tarjeton/" + _Partner.partnerReference);
            }
            catch (Exception ex)
            {
                // ENVIAR A LA VISTA DE ERROR (GERMAN)
                throw;
            }
        }



        /// PADRON /// 

        [SessionFilter("Admin")]
        [HttpGet]
        [Route("pdf/listado-socios")]
        public ActionResult GeneratePartnersListPdf()
        {
            string workDirectory = null;

            try
            {
                // Permite que la generación dure hasta dos horas.
                Server.ScriptTimeout = 7200;

                // ============================================================
                // 1. OBTENER EL LISTADO COMPLETO DE SOCIOS
                // ============================================================

                List<Partner> partners = GetAllPartnersForPdf();

                if (partners == null || partners.Count == 0)
                {
                    return Content("No se encontraron socios para generar el PDF.");
                }

                partners = partners
                    .Where(x => x != null && x.Numero > 0)
                    .GroupBy(x => x.Numero)
                    .Select(x => x.First())
                    .OrderBy(x => x.Numero)
                    .ThenBy(x => x.Nombre)
                    .ToList();

                // ============================================================
                // 2. CREAR DIRECTORIO TEMPORAL
                // ============================================================

                string baseDirectory = Server.MapPath(
                    "~/App_Data/GeneratedPartnerBooks"
                );

                if (!Directory.Exists(baseDirectory))
                {
                    Directory.CreateDirectory(baseDirectory);
                }

                // Elimina libros temporales antiguos.
                DeleteOldPartnerPdfDirectories(baseDirectory);

                string processId =
                    DateTime.Now.ToString("yyyyMMdd_HHmmss") +
                    "_" +
                    Guid.NewGuid().ToString("N");

                workDirectory = Path.Combine(
                    baseDirectory,
                    processId
                );

                Directory.CreateDirectory(workDirectory);

                // ============================================================
                // 3. GENERAR PDF EN BLOQUES
                // ============================================================

                // Es múltiplo de 8 para que cada bloque termine
                // exactamente al final de una página.
                const int partnersPerChunk = 240;

                List<string> chunkFiles = new List<string>();

                int chunkNumber = 1;

                for (
                    int startIndex = 0;
                    startIndex < partners.Count;
                    startIndex += partnersPerChunk
                )
                {
                    List<Partner> currentChunk = partners
                        .Skip(startIndex)
                        .Take(partnersPerChunk)
                        .ToList();

                    // Recuperamos fotografía y datos detallados solamente
                    // para el bloque que se está procesando.
                    LoadPartnerDetailsForPdf(currentChunk);

                    string chunkPath = Path.Combine(
                        workDirectory,
                        "parte_" +
                        chunkNumber.ToString("0000") +
                        ".pdf"
                    );

                    GeneratePartnerPdfChunk(
                        currentChunk,
                        chunkPath
                    );

                    chunkFiles.Add(chunkPath);

                    // Liberamos la información pesada del bloque.
                    foreach (Partner partner in currentChunk)
                    {
                        if (partner == null)
                        {
                            continue;
                        }

                        partner.amazonPictures = null;
                        partner.urlImage = null;
                        partner.bucket = null;
                        partner.key = null;
                    }

                    currentChunk.Clear();

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    chunkNumber++;
                }

                // ============================================================
                // 4. UNIR LOS BLOQUES EN UN ÚNICO LIBRO
                // ============================================================

                string finalFileName =
                    "Libro_Completo_Socios_" +
                    DateTime.Now.ToString("yyyyMMdd_HHmmss") +
                    ".pdf";

                string finalFilePath = Path.Combine(
                    workDirectory,
                    finalFileName
                );

                MergePartnerPdfChunks(
                    chunkFiles,
                    finalFilePath
                );

                // Eliminamos las partes; se conserva solamente el PDF final.
                foreach (string chunkFile in chunkFiles)
                {
                    try
                    {
                        if (System.IO.File.Exists(chunkFile))
                        {
                            System.IO.File.Delete(chunkFile);
                        }
                    }
                    catch
                    {
                        // Si alguna parte no puede eliminarse,
                        // no detenemos la descarga del libro.
                    }
                }

                // FilePathResult transmite el archivo desde disco.
                // No carga todo el libro final en un byte[].
                return File(
                    finalFilePath,
                    "application/pdf",
                    finalFileName
                );
            }
            catch (Exception ex)
            {
                return Content(
                    "Ocurrió un error al generar el libro de socios: " +
                    ex.Message
                );
            }
        }


        /// <summary>
        /// Obtiene todos los socios usando el método paginado existente.
        /// Después consulta el detalle de cada socio para obtener dirección,
        /// fotografía y demás información relacionada.
        /// </summary>
        /// <summary>
        /// Obtiene temporalmente los primeros 100 socios para probar
        /// el diseño, las fotografías y la información del PDF.
        /// </summary>
        /// <summary>
        /// Obtiene los primeros 100 socios directamente desde la consulta
        /// utilizada por la vista Index.
        /// </summary>
        /// <summary>
        /// Obtiene los primeros 100 socios del listado.
        /// Solamente conserva registros que tengan un número de socio válido.
        /// </summary>
        /// <summary>
        /// Obtiene los primeros 100 socios y consulta individualmente
        /// la información necesaria para recuperar su fotografía de Amazon.
        /// </summary>
        /// <summary>
        /// Recorre todas las páginas del padrón y obtiene
        /// el listado completo de socios.
        /// </summary>
        private List<Partner> GetAllPartnersForPdf()
        {
            List<Partner> partners = new List<Partner>();

            const int pageSize = 100;

            try
            {
                Partner partnerModel = new Partner();

                // Primera consulta para conocer TotalItems.
                PartnerPagination firstPage = partnerModel.get(
                    1,
                    pageSize,
                    "",
                    "",
                    "",
                    ""
                );

                if (firstPage == null ||
                    firstPage.Data == null ||
                    firstPage.Data.Count == 0)
                {
                    return partners;
                }

                partners.AddRange(
                    firstPage.Data.Where(x =>
                        x != null &&
                        x.Numero > 0
                    )
                );

                int totalItems = firstPage.TotalItems;

                int totalPages = totalItems > 0
                    ? (int)Math.Ceiling(
                        totalItems / (double)pageSize
                    )
                    : 1;

                // La página 1 ya fue consultada.
                for (int pageNumber = 2;
                     pageNumber <= totalPages;
                     pageNumber++)
                {
                    PartnerPagination page = new Partner().get(
                        pageNumber,
                        pageSize,
                        "",
                        "",
                        "",
                        ""
                    );

                    if (page == null ||
                        page.Data == null ||
                        page.Data.Count == 0)
                    {
                        continue;
                    }

                    partners.AddRange(
                        page.Data.Where(x =>
                            x != null &&
                            x.Numero > 0
                        )
                    );
                }

                return partners
                    .Where(x =>
                        x != null &&
                        x.Numero > 0
                    )
                    .GroupBy(x => x.Numero)
                    .Select(x => x.First())
                    .ToList();
            }
            catch
            {
                return partners;
            }
        }


        /// <summary>
        /// Consulta el detalle de cada socio del bloque actual
        /// para obtener bucket, key y amazonPictures.
        /// </summary>
        private void LoadPartnerDetailsForPdf(
            List<Partner> partners)
        {
            if (partners == null)
            {
                return;
            }

            foreach (Partner partnerItem in partners)
            {
                if (partnerItem == null ||
                    partnerItem.Numero <= 0)
                {
                    continue;
                }

                try
                {
                    Partner partnerDetail = new Partner().get(
                        partnerItem.Numero.ToString()
                    );

                    if (partnerDetail == null)
                    {
                        continue;
                    }

                    partnerItem.bucket = partnerDetail.bucket;
                    partnerItem.key = partnerDetail.key;
                    partnerItem.urlImage = partnerDetail.urlImage;
                    partnerItem.amazonPictures =
                        partnerDetail.amazonPictures;

                    if (string.IsNullOrWhiteSpace(
                        partnerItem.Nombre))
                    {
                        partnerItem.Nombre =
                            partnerDetail.firstName;
                    }

                    if (string.IsNullOrWhiteSpace(
                        partnerItem.Paterno))
                    {
                        partnerItem.Paterno =
                            partnerDetail.lastNameF;
                    }

                    if (string.IsNullOrWhiteSpace(
                        partnerItem.Materno))
                    {
                        partnerItem.Materno =
                            partnerDetail.lastNameM;
                    }
                }
                catch
                {
                    // Si falla un socio, se conserva el registro
                    // del listado y se mostrará la imagen predeterminada.
                }
            }
        }


        /// <summary>
        /// Genera una parte temporal del libro.
        /// </summary>
        private void GeneratePartnerPdfChunk(
            List<Partner> partners,
            string outputPath)
        {
            Document document = new Document();

            document.Info.Title = "Listado de socios";
            document.Info.Subject = "Listado general de socios";
            document.Info.Author = "Radiotaxi";

            Style normalStyle = document.Styles["Normal"];
            normalStyle.Font.Name = "Arial";
            normalStyle.Font.Size = 8;

            const int partnersPerPage = 8;

            for (
                int pageStart = 0;
                pageStart < partners.Count;
                pageStart += partnersPerPage
            )
            {
                Section section = document.AddSection();

                section.PageSetup.Orientation =
                    Orientation.Portrait;

                section.PageSetup.PageWidth =
                    Unit.FromCentimeter(21.59);

                section.PageSetup.PageHeight =
                    Unit.FromCentimeter(27.94);

                section.PageSetup.TopMargin =
                    Unit.FromCentimeter(0.80);

                section.PageSetup.BottomMargin =
                    Unit.FromCentimeter(0.80);

                section.PageSetup.LeftMargin =
                    Unit.FromCentimeter(1.00);

                section.PageSetup.RightMargin =
                    Unit.FromCentimeter(1.00);

                // No agregamos aquí la numeración porque se colocará
                // después de unir todas las partes.
                const double cardWidth = 9.55;
                const double cardHeight = 6.15;
                const double horizontalGap = 0.49;
                const double verticalGap = 0.35;

                for (
                    int position = 0;
                    position < partnersPerPage;
                    position++
                )
                {
                    int partnerIndex =
                        pageStart + position;

                    if (partnerIndex >= partners.Count)
                    {
                        break;
                    }

                    Partner partner =
                        partners[partnerIndex];

                    int row = position / 2;
                    int column = position % 2;

                    double left =
                        1.00 +
                        (column *
                        (cardWidth + horizontalGap));

                    double top =
                        0.70 +
                        (row *
                        (cardHeight + verticalGap));

                    AddPartnerCardToPdf(
                        section,
                        partner,
                        left,
                        top,
                        cardWidth,
                        cardHeight
                    );
                }
            }

            PdfDocumentRenderer renderer =
                new PdfDocumentRenderer();

            renderer.Document = document;
            renderer.RenderDocument();
            renderer.Save(outputPath);
        }

        /// <summary>
        /// Une todas las partes del libro en un solo PDF
        /// y agrega la numeración global de páginas.
        /// </summary>
        private void MergePartnerPdfChunks(
            List<string> chunkFiles,
            string finalFilePath)
        {
            PdfDocument finalDocument =
                new PdfDocument();

            foreach (string chunkFile in chunkFiles)
            {
                if (!System.IO.File.Exists(chunkFile))
                {
                    continue;
                }

                using (
                    PdfDocument inputDocument =
                        PdfReader.Open(
                            chunkFile,
                            PdfDocumentOpenMode.Import
                        )
                )
                {
                    for (
                        int pageIndex = 0;
                        pageIndex < inputDocument.PageCount;
                        pageIndex++
                    )
                    {
                        finalDocument.AddPage(
                            inputDocument.Pages[pageIndex]
                        );
                    }
                }
            }

            int totalPages = finalDocument.PageCount;

            XFont pageFont = new XFont(
                "Arial",
                8,
                XFontStyle.Regular
            );

            for (
                int pageIndex = 0;
                pageIndex < totalPages;
                pageIndex++
            )
            {
                PdfPage page =
                    finalDocument.Pages[pageIndex];

                using (
                    XGraphics graphics =
                        XGraphics.FromPdfPage(page)
                )
                {
                    string pageText =
                        "Página " +
                        (pageIndex + 1) +
                        " de " +
                        totalPages;

                    XRect footerRectangle = new XRect(
                        0,
                        page.Height.Point - 18,
                        page.Width.Point,
                        12
                    );

                    graphics.DrawString(
                        pageText,
                        pageFont,
                        XBrushes.Black,
                        footerRectangle,
                        XStringFormats.Center
                    );
                }
            }

            finalDocument.Save(finalFilePath);
            finalDocument.Close();
        }

        /// <summary>
        /// Elimina carpetas de generación con más de un día
        /// para evitar llenar App_Data.
        /// </summary>
        private void DeleteOldPartnerPdfDirectories(
            string baseDirectory)
        {
            try
            {
                if (!Directory.Exists(baseDirectory))
                {
                    return;
                }

                DirectoryInfo directory =
                    new DirectoryInfo(baseDirectory);

                DateTime expirationDate =
                    DateTime.Now.AddDays(-1);

                foreach (
                    DirectoryInfo childDirectory
                    in directory.GetDirectories()
                )
                {
                    try
                    {
                        if (childDirectory.CreationTime <
                            expirationDate)
                        {
                            childDirectory.Delete(true);
                        }
                    }
                    catch
                    {
                        // Una carpeta podría estar descargándose.
                    }
                }
            }
            catch
            {
                // La limpieza no debe impedir generar el libro.
            }
        }


        /// <summary>
        /// Agrega la tarjeta individual de un socio al PDF.
        /// </summary>
        private void AddPartnerCardToPdf(
            Section section,
            Partner partner,
            double leftCm,
            double topCm,
            double widthCm,
            double heightCm)
        {
            // Marco exterior.
            TextFrame cardFrame = section.AddTextFrame();

            cardFrame.Left = Unit.FromCentimeter(leftCm);
            cardFrame.Top = Unit.FromCentimeter(topCm);
            cardFrame.Width = Unit.FromCentimeter(widthCm);
            cardFrame.Height = Unit.FromCentimeter(heightCm);

            cardFrame.RelativeHorizontal = RelativeHorizontal.Page;
            cardFrame.RelativeVertical = RelativeVertical.Page;

            cardFrame.MarginLeft = Unit.FromCentimeter(0.20);
            cardFrame.MarginRight = Unit.FromCentimeter(0.20);
            cardFrame.MarginTop = Unit.FromCentimeter(0.20);
            cardFrame.MarginBottom = Unit.FromCentimeter(0.20);

            cardFrame.LineFormat.Width = Unit.FromPoint(0.75);
            cardFrame.LineFormat.Color = Colors.Gray;

            // Barra superior con el número del socio.
            TextFrame headerFrame = section.AddTextFrame();

            headerFrame.Left = Unit.FromCentimeter(leftCm + 0.15);
            headerFrame.Top = Unit.FromCentimeter(topCm + 0.15);
            headerFrame.Width = Unit.FromCentimeter(widthCm - 0.30);
            headerFrame.Height = Unit.FromCentimeter(0.65);

            headerFrame.RelativeHorizontal = RelativeHorizontal.Page;
            headerFrame.RelativeVertical = RelativeVertical.Page;

            headerFrame.FillFormat.Color = Color.FromRgb(240, 240, 240);
            headerFrame.LineFormat.Width = Unit.FromPoint(0);

            Paragraph headerParagraph = headerFrame.AddParagraph();
            headerParagraph.Format.Alignment = ParagraphAlignment.Center;
            headerParagraph.Format.Font.Name = "Arial";
            headerParagraph.Format.Font.Size = 12;
            headerParagraph.Format.Font.Bold = true;
            headerParagraph.Format.Font.Color = Colors.Black;

            headerParagraph.AddText("ECO: " + SafePartnerText(partner.Numero));

            // ============================================================
            // FOTOGRAFÍA
            // ============================================================

            string imageFileName = GetPartnerImageForPdf(partner);

            if (!string.IsNullOrWhiteSpace(imageFileName))
            {
                try
                {
                    Image image = section.AddImage(imageFileName);

                    image.Left = Unit.FromCentimeter(leftCm + 0.25);
                    image.Top = Unit.FromCentimeter(topCm + 1.00);

                    image.Width = Unit.FromCentimeter(2.70);
                    image.Height = Unit.FromCentimeter(3.60);

                    image.RelativeHorizontal = RelativeHorizontal.Page;
                    image.RelativeVertical = RelativeVertical.Page;
                    image.WrapFormat.Style = WrapStyle.Through;
                    image.LockAspectRatio = false;
                }
                catch
                {
                    AddNoImageTextToPdf(
                        section,
                        leftCm + 0.25,
                        topCm + 1.00
                    );
                }
            }
            else
            {
                AddNoImageTextToPdf(
                    section,
                    leftCm + 0.25,
                    topCm + 1.00
                );
            }

            // ============================================================
            // NOMBRE COMPLETO
            // ============================================================

            TextFrame nameFrame = section.AddTextFrame();

            nameFrame.Left = Unit.FromCentimeter(leftCm + 3.20);
            nameFrame.Top = Unit.FromCentimeter(topCm + 1.00);
            nameFrame.Width = Unit.FromCentimeter(widthCm - 3.45);
            nameFrame.Height = Unit.FromCentimeter(1.60);

            nameFrame.RelativeHorizontal = RelativeHorizontal.Page;
            nameFrame.RelativeVertical = RelativeVertical.Page;

            Paragraph nameLabel = nameFrame.AddParagraph();
            nameLabel.Format.Font.Name = "Arial";
            nameLabel.Format.Font.Size = 7;
            nameLabel.Format.Font.Bold = true;
            nameLabel.Format.Font.Color = Colors.Gray;
            nameLabel.AddText("NOMBRE");

            Paragraph nameParagraph = nameFrame.AddParagraph();
            nameParagraph.Format.Font.Name = "Arial";
            nameParagraph.Format.Font.Size = 10;
            nameParagraph.Format.Font.Bold = true;
            nameParagraph.Format.SpaceBefore = Unit.FromPoint(2);
            nameParagraph.Format.SpaceAfter = Unit.FromPoint(0);

            string fullName = GetPartnerFullNameForPdf(partner);

            nameParagraph.AddText(
                string.IsNullOrWhiteSpace(fullName)
                    ? "SIN NOMBRE"
                    : fullName.ToUpper()
            );

            // ============================================================
            // FIRMA
            // ============================================================

            TextFrame signatureFrame = section.AddTextFrame();

            signatureFrame.Left = Unit.FromCentimeter(leftCm + 3.20);
            signatureFrame.Top = Unit.FromCentimeter(topCm + 3.25);
            signatureFrame.Width = Unit.FromCentimeter(widthCm - 3.60);
            signatureFrame.Height = Unit.FromCentimeter(2.10);

            signatureFrame.RelativeHorizontal = RelativeHorizontal.Page;
            signatureFrame.RelativeVertical = RelativeVertical.Page;

            // Espacio en blanco para que el responsable firme.
            Paragraph signatureSpace = signatureFrame.AddParagraph();
            signatureSpace.Format.SpaceAfter = Unit.FromCentimeter(0.65);
            signatureSpace.AddText("");

            // Línea de firma.
            Paragraph signatureLine = signatureFrame.AddParagraph();
            signatureLine.Format.Alignment = ParagraphAlignment.Center;
            signatureLine.Format.Font.Name = "Arial";
            signatureLine.Format.Font.Size = 9;
            signatureLine.Format.Font.Bold = false;
            signatureLine.AddText("________________________________");

            // Leyenda.
            Paragraph signatureLabel = signatureFrame.AddParagraph();
            signatureLabel.Format.Alignment = ParagraphAlignment.Center;
            signatureLabel.Format.Font.Name = "Arial";
            signatureLabel.Format.Font.Size = 8;
            signatureLabel.Format.Font.Bold = true;
            signatureLabel.Format.SpaceBefore = Unit.FromPoint(2);
            signatureLabel.AddText("FIRMA");
        }


        /// <summary>
        /// Obtiene la fotografía del socio desde Amazon y devuelve
        /// un identificador compatible con MigraDoc.
        /// </summary>
        /// <summary>
        /// Obtiene la fotografía del socio desde Amazon.
        /// Primero utiliza bucket y key; si no existen, busca la fotografía
        /// principal dentro de amazonPictures.
        /// </summary>
        private string GetPartnerImageForPdf(Partner partner)
        {
            try
            {
                if (partner == null)
                {
                    return GetDefaultPartnerImageForPdf();
                }

                string bucket = partner.bucket;
                string key = partner.key;

                // Si el detalle no colocó la imagen directamente en bucket/key,
                // intentamos obtenerla desde amazonPictures.
                if ((string.IsNullOrWhiteSpace(bucket) ||
                     string.IsNullOrWhiteSpace(key)) &&
                    partner.amazonPictures != null)
                {
                    amazonPicture picture = partner.amazonPictures
                        .Where(x =>
                            x != null &&
                            x.pictureTypeId == 2 &&
                            !string.IsNullOrWhiteSpace(x.bucket) &&
                            !string.IsNullOrWhiteSpace(x.key)
                        )
                        .FirstOrDefault();

                    if (picture == null)
                    {
                        picture = partner.amazonPictures
                            .Where(x =>
                                x != null &&
                                !string.IsNullOrWhiteSpace(x.bucket) &&
                                !string.IsNullOrWhiteSpace(x.key)
                            )
                            .FirstOrDefault();
                    }

                    if (picture != null)
                    {
                        bucket = picture.bucket;
                        key = picture.key;
                    }
                }

                if (!string.IsNullOrWhiteSpace(bucket) &&
                    !string.IsNullOrWhiteSpace(key))
                {
                    string imageUrl = AmazonHelper.getImageUser(
                        bucket,
                        key
                    );

                    if (!string.IsNullOrWhiteSpace(imageUrl))
                    {
                        byte[] imageBytes =
                            AmazonHelper.GetImageStream(imageUrl);

                        if (imageBytes != null &&
                            imageBytes.Length > 0)
                        {
                            return AmazonHelper
                                .MigraDocFilenameFromByteArray(imageBytes);
                        }
                    }
                }
            }
            catch
            {
                // Si falla Amazon o la imagen está dañada,
                // se utiliza la imagen predeterminada.
            }

            return GetDefaultPartnerImageForPdf();
        }

        /// <summary>
        /// Devuelve la imagen predeterminada cuando el socio
        /// no tiene una fotografía válida.
        /// </summary>
        private string GetDefaultPartnerImageForPdf()
        {
            try
            {
                string defaultImage = Server.MapPath(
                    "~/Content/images/users/empty.png"
                );

                if (System.IO.File.Exists(defaultImage))
                {
                    return defaultImage;
                }
            }
            catch
            {
            }

            return null;
        }
        /// <summary>
        /// Agrega un recuadro cuando no existe fotografía.
        /// </summary>
        private void AddNoImageTextToPdf(
            Section section,
            double leftCm,
            double topCm)
        {
            TextFrame noImageFrame = section.AddTextFrame();

            noImageFrame.Left = Unit.FromCentimeter(leftCm);
            noImageFrame.Top = Unit.FromCentimeter(topCm);
            noImageFrame.Width = Unit.FromCentimeter(2.70);
            noImageFrame.Height = Unit.FromCentimeter(3.60);

            noImageFrame.RelativeHorizontal = RelativeHorizontal.Page;
            noImageFrame.RelativeVertical = RelativeVertical.Page;

            noImageFrame.LineFormat.Width = Unit.FromPoint(0.50);
            noImageFrame.LineFormat.Color = Colors.LightGray;
            noImageFrame.FillFormat.Color = Color.FromRgb(248, 248, 248);

            noImageFrame.MarginTop = Unit.FromCentimeter(1.45);

            Paragraph paragraph = noImageFrame.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.Name = "Arial";
            paragraph.Format.Font.Size = 7;
            paragraph.Format.Font.Color = Colors.Gray;
            paragraph.AddText("SIN FOTOGRAFÍA");
        }


        /// <summary>
        /// Construye el nombre completo del socio.
        /// </summary>
        /// <summary>
        /// Construye el nombre completo usando las propiedades
        /// que devuelve la consulta del listado de socios.
        /// </summary>
        private string GetPartnerFullNameForPdf(Partner partner)
        {
            if (partner == null)
            {
                return string.Empty;
            }

            List<string> nameParts = new List<string>();

            if (!string.IsNullOrWhiteSpace(partner.Nombre))
            {
                nameParts.Add(partner.Nombre.Trim());
            }

            if (!string.IsNullOrWhiteSpace(partner.Paterno))
            {
                nameParts.Add(partner.Paterno.Trim());
            }

            if (!string.IsNullOrWhiteSpace(partner.Materno))
            {
                nameParts.Add(partner.Materno.Trim());
            }

            return string.Join(" ", nameParts);
        }


        /// <summary>
        /// Construye la dirección usando las mismas propiedades
        /// mostradas en la vista tarjeton.
        /// </summary>
        /// <summary>
        /// Construye la dirección utilizando las propiedades
        /// que devuelve la consulta paginada del padrón.
        /// </summary>
        private string GetPartnerAddressForPdf(Partner partner)
        {
            if (partner == null)
            {
                return string.Empty;
            }

            List<string> addressParts = new List<string>();

            if (!string.IsNullOrWhiteSpace(partner.fraccionamiento))
            {
                addressParts.Add(
                    "FRACC. " + partner.fraccionamiento.Trim()
                );
            }

            if (!string.IsNullOrWhiteSpace(partner.sm))
            {
                addressParts.Add(
                    "SM " + partner.sm.Trim()
                );
            }

            if (!string.IsNullOrWhiteSpace(partner.mz))
            {
                addressParts.Add(
                    "MZ " + partner.mz.Trim()
                );
            }

            if (!string.IsNullOrWhiteSpace(partner.lote))
            {
                addressParts.Add(
                    "LT " + partner.lote.Trim()
                );
            }

            if (!string.IsNullOrWhiteSpace(partner.calle))
            {
                addressParts.Add(
                    "CALLE " + partner.calle.Trim()
                );
            }

            if (!string.IsNullOrWhiteSpace(partner.num))
            {
                addressParts.Add(
                    "NÚM. " + partner.num.Trim()
                );
            }

            if (!string.IsNullOrWhiteSpace(partner.localidad))
            {
                addressParts.Add(
                    partner.localidad.Trim()
                );
            }

            if (!string.IsNullOrWhiteSpace(partner.municipio))
            {
                addressParts.Add(
                    partner.municipio.Trim()
                );
            }

            if (!string.IsNullOrWhiteSpace(partner.ubicacion))
            {
                addressParts.Add(
                    partner.ubicacion.Trim()
                );
            }

            return string.Join(", ", addressParts);
        }


        /// <summary>
        /// Agrega una parte de la dirección solamente si contiene información.
        /// </summary>
        private void AddAddressPart(
            List<string> addressParts,
            string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                addressParts.Add(value.Trim());
            }
        }


        /// <summary>
        /// Convierte cualquier valor a texto evitando valores nulos.
        /// </summary>
        private string SafePartnerText(object value)
        {
            return value == null
                ? string.Empty
                : Convert.ToString(value).Trim();
        }


        /// <summary>
        /// Obtiene la parte numérica del número de socio para ordenar
        /// correctamente el listado.
        /// </summary>
        private long GetPartnerNumericReference(string partnerReference)
        {
            if (string.IsNullOrWhiteSpace(partnerReference))
            {
                return long.MaxValue;
            }

            string numericValue = new string(
                partnerReference
                    .Where(char.IsDigit)
                    .ToArray()
            );

            long result;

            if (long.TryParse(numericValue, out result))
            {
                return result;
            }

            return long.MaxValue;
        }

        [SessionFilter("Admin,tarjeton")]
        [HttpGet]
        [Route("pdf/revisar-primer-socio")]
        public ActionResult RevisarPrimerSocioPdf()
        {
            try
            {
                Partner partnerModel = new Partner();

                PartnerPagination pagination = partnerModel.get(
                    1,
                    100,
                    "",
                    "",
                    "",
                    ""
                );

                if (pagination == null)
                {
                    return Content(
                        "Partner.get devolvió null.",
                        "text/plain"
                    );
                }

                if (pagination.Data == null)
                {
                    return Content(
                        "pagination.Data devolvió null.",
                        "text/plain"
                    );
                }

                if (pagination.Data.Count == 0)
                {
                    return Content(
                        "pagination.Data no contiene registros.",
                        "text/plain"
                    );
                }

                Partner primerSocio = pagination.Data.FirstOrDefault();

                if (primerSocio == null)
                {
                    return Content(
                        "El primer registro es null.",
                        "text/plain"
                    );
                }

                JsonSerializerSettings settings =
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling =
                            ReferenceLoopHandling.Ignore,

                        Formatting =
                            Formatting.Indented,

                        NullValueHandling =
                            NullValueHandling.Include
                    };

                string json = JsonConvert.SerializeObject(
                    primerSocio,
                    settings
                );

                return Content(
                    json,
                    "application/json",
                    Encoding.UTF8
                );
            }
            catch (Exception ex)
            {
                return Content(
                    "Error: " +
                    ex.Message +
                    Environment.NewLine +
                    Environment.NewLine +
                    ex.StackTrace,
                    "text/plain"
                );
            }
        }


        /// PADRON PARA CANDIDATOS /// 
        /// 

        [SessionFilter("Admin")]
        [HttpGet]
        [Route("pdf/directorio-socios")]
        public ActionResult GeneratePartnersDirectoryPdf()
        {
            string workDirectory = null;

            try
            {
                // Hasta dos horas.
                Server.ScriptTimeout = 7200;

                // ============================================================
                // 1. OBTENER TODOS LOS SOCIOS
                // ============================================================

                //List<Partner> partners = GetAllPartnersForPdf();
                List<Partner> partners = GetFirstPartnersForDirectoryPdf(100);

                if (partners == null || partners.Count == 0)
                {
                    return Content(
                        "No se encontraron socios para generar el directorio."
                    );
                }

                partners = partners
                    .Where(x =>
                        x != null &&
                        x.Numero > 0
                    )
                    .GroupBy(x => x.Numero)
                    .Select(x => x.First())
                    .OrderBy(x => x.Numero)
                    .ThenBy(x => x.Nombre)
                    .ToList();

                // ============================================================
                // 2. DIRECTORIO TEMPORAL
                // ============================================================

                string baseDirectory = Server.MapPath(
                    "~/App_Data/GeneratedPartnerDirectories"
                );

                if (!Directory.Exists(baseDirectory))
                {
                    Directory.CreateDirectory(baseDirectory);
                }

                DeleteOldPartnerPdfDirectories(baseDirectory);

                string processId =
                    DateTime.Now.ToString("yyyyMMdd_HHmmss") +
                    "_" +
                    Guid.NewGuid().ToString("N");

                workDirectory = Path.Combine(
                    baseDirectory,
                    processId
                );

                Directory.CreateDirectory(workDirectory);

                // ============================================================
                // 3. GENERAR EN BLOQUES
                // ============================================================

                // 240 es múltiplo de 12:
                // 20 páginas por archivo temporal.
                const int partnersPerChunk = 240;

                List<string> chunkFiles = new List<string>();

                int chunkNumber = 1;

                for (
                    int startIndex = 0;
                    startIndex < partners.Count;
                    startIndex += partnersPerChunk
                )
                {
                    List<Partner> currentChunk = partners
                        .Skip(startIndex)
                        .Take(partnersPerChunk)
                        .ToList();

                    // Este método ya existe en tu controlador.
                    // Obtiene bucket, key y amazonPictures.
                    LoadPartnerDetailsForPdf(currentChunk);

                    string chunkPath = Path.Combine(
                        workDirectory,
                        "directorio_parte_" +
                        chunkNumber.ToString("0000") +
                        ".pdf"
                    );

                    GeneratePartnerDirectoryPdfChunk(
                        currentChunk,
                        chunkPath
                    );

                    chunkFiles.Add(chunkPath);

                    // Liberar propiedades pesadas.
                    foreach (Partner partner in currentChunk)
                    {
                        if (partner == null)
                        {
                            continue;
                        }

                        partner.amazonPictures = null;
                        partner.urlImage = null;
                        partner.bucket = null;
                        partner.key = null;
                    }

                    currentChunk.Clear();

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    chunkNumber++;
                }

                // ============================================================
                // 4. UNIR TODAS LAS PARTES
                // ============================================================

                string finalFileName =
                    "Directorio_Completo_Socios_" +
                    DateTime.Now.ToString("yyyyMMdd_HHmmss") +
                    ".pdf";

                string finalFilePath = Path.Combine(
                    workDirectory,
                    finalFileName
                );

                // Este método ya existe en tu controlador
                // y agrega la numeración global.
                MergePartnerPdfChunks(
                    chunkFiles,
                    finalFilePath
                );

                foreach (string chunkFile in chunkFiles)
                {
                    try
                    {
                        if (System.IO.File.Exists(chunkFile))
                        {
                            System.IO.File.Delete(chunkFile);
                        }
                    }
                    catch
                    {
                        // No detener la descarga si una parte no puede eliminarse.
                    }
                }

                return File(
                    finalFilePath,
                    "application/pdf",
                    finalFileName
                );
            }
            catch (Exception ex)
            {
                return Content(
                    "Ocurrió un error al generar el directorio de socios: " +
                    ex.Message
                );
            }
        }


        /// <summary>
        /// Genera una parte temporal del directorio de socios.
        /// Formato carta horizontal, 12 socios por página.
        /// </summary>
        /// <summary>
        /// Genera una parte temporal del directorio de socios.
        /// Formato carta vertical, 12 socios por página.
        /// </summary>
        private void GeneratePartnerDirectoryPdfChunk(
            List<Partner> partners,
            string outputPath)
        {
            Document document = new Document();

            document.Info.Title = "Directorio de socios";
            document.Info.Subject =
                "Directorio general con fotografías, domicilios y teléfonos";
            document.Info.Author = "Radiotaxi";

            Style normalStyle = document.Styles["Normal"];
            normalStyle.Font.Name = "Arial";
            normalStyle.Font.Size = 7;

            const int partnersPerPage = 12;

            for (
                int pageStart = 0;
                pageStart < partners.Count;
                pageStart += partnersPerPage
            )
            {
                Section section = document.AddSection();

                // Tamaño carta vertical.
                section.PageSetup.Orientation =
                    Orientation.Portrait;

                section.PageSetup.PageWidth =
                    Unit.FromCentimeter(21.59);

                section.PageSetup.PageHeight =
                    Unit.FromCentimeter(27.94);

                section.PageSetup.TopMargin =
                    Unit.FromCentimeter(0.60);

                section.PageSetup.BottomMargin =
                    Unit.FromCentimeter(0.75);

                section.PageSetup.LeftMargin =
                    Unit.FromCentimeter(0.60);

                section.PageSetup.RightMargin =
                    Unit.FromCentimeter(0.60);

                // Encabezado del directorio.
                AddPartnerDirectoryHeaderToPdf(section);

                const double rowLeft = 0.60;
                const double rowTop = 1.85;
                const double rowWidth = 20.39;
                const double rowHeight = 1.88;
                const double rowGap = 0.08;

                for (
                    int position = 0;
                    position < partnersPerPage;
                    position++
                )
                {
                    int partnerIndex =
                        pageStart + position;

                    if (partnerIndex >= partners.Count)
                    {
                        break;
                    }

                    Partner partner =
                        partners[partnerIndex];

                    double top =
                        rowTop +
                        (position * (rowHeight + rowGap));

                    AddPartnerDirectoryRowToPdf(
                        section,
                        partner,
                        rowLeft,
                        top,
                        rowWidth,
                        rowHeight
                    );
                }
            }

            PdfDocumentRenderer renderer =
                new PdfDocumentRenderer();

            renderer.Document = document;
            renderer.RenderDocument();
            renderer.Save(outputPath);
        }

        /// <summary>
        /// Agrega los títulos del directorio.
        /// </summary>
        /// <summary>
        /// Agrega el título y los encabezados de las columnas
        /// del directorio en formato carta vertical.
        /// </summary>
        private void AddPartnerDirectoryHeaderToPdf(
            Section section)
        {
            // ============================================================
            // TÍTULO
            // ============================================================

            TextFrame titleFrame = section.AddTextFrame();

            titleFrame.Left =
                Unit.FromCentimeter(0.60);

            titleFrame.Top =
                Unit.FromCentimeter(0.25);

            titleFrame.Width =
                Unit.FromCentimeter(20.39);

            titleFrame.Height =
                Unit.FromCentimeter(0.55);

            titleFrame.RelativeHorizontal =
                RelativeHorizontal.Page;

            titleFrame.RelativeVertical =
                RelativeVertical.Page;

            Paragraph titleParagraph =
                titleFrame.AddParagraph();

            titleParagraph.Format.Alignment =
                ParagraphAlignment.Center;

            titleParagraph.Format.Font.Name =
                "Arial";

            titleParagraph.Format.Font.Size =
                11;

            titleParagraph.Format.Font.Bold =
                true;

            titleParagraph.AddText(
                "DIRECTORIO GENERAL DE SOCIOS"
            );

            // ============================================================
            // FONDO DEL ENCABEZADO
            // ============================================================

            TextFrame headerFrame = section.AddTextFrame();

            headerFrame.Left =
                Unit.FromCentimeter(0.60);

            headerFrame.Top =
                Unit.FromCentimeter(1.02);

            headerFrame.Width =
                Unit.FromCentimeter(20.39);

            headerFrame.Height =
                Unit.FromCentimeter(0.58);

            headerFrame.RelativeHorizontal =
                RelativeHorizontal.Page;

            headerFrame.RelativeVertical =
                RelativeVertical.Page;

            headerFrame.FillFormat.Color =
                Color.FromRgb(225, 225, 225);

            headerFrame.LineFormat.Width =
                Unit.FromPoint(0.75);

            headerFrame.LineFormat.Color =
                Colors.Gray;

            // ============================================================
            // TÍTULOS DE LAS COLUMNAS
            // ============================================================

            AddDirectoryColumnHeader(
                section,
                "FOTO",
                0.68,
                1.16,
                1.35
            );

            AddDirectoryColumnHeader(
                section,
                "ECO",
                2.12,
                1.16,
                1.15
            );

            AddDirectoryColumnHeader(
                section,
                "NOMBRE",
                3.38,
                1.16,
                4.10
            );

            AddDirectoryColumnHeader(
                section,
                "DIRECCIÓN",
                7.58,
                1.16,
                9.05
            );

            AddDirectoryColumnHeader(
                section,
                "TELÉFONOS",
                16.72,
                1.16,
                4.10
            );
        }

        private void AddDirectoryColumnHeader(
    Section section,
    string text,
    double leftCm,
    double topCm,
    double widthCm)
        {
            TextFrame frame = section.AddTextFrame();

            frame.Left = Unit.FromCentimeter(leftCm);
            frame.Top = Unit.FromCentimeter(topCm);
            frame.Width = Unit.FromCentimeter(widthCm);
            frame.Height = Unit.FromCentimeter(0.30);

            frame.RelativeHorizontal =
                RelativeHorizontal.Page;

            frame.RelativeVertical =
                RelativeVertical.Page;

            Paragraph paragraph = frame.AddParagraph();

            paragraph.Format.Alignment =
                ParagraphAlignment.Center;

            paragraph.Format.Font.Name = "Arial";
            paragraph.Format.Font.Size = 7;
            paragraph.Format.Font.Bold = true;

            paragraph.AddText(text);
        }


        /// <summary>
        /// Agrega un socio como un renglón del directorio.
        /// </summary>
        /// <summary>
        /// Agrega un socio como un renglón del directorio vertical.
        /// </summary>
        private void AddPartnerDirectoryRowToPdf(
            Section section,
            Partner partner,
            double leftCm,
            double topCm,
            double widthCm,
            double heightCm)
        {
            // ============================================================
            // MARCO DEL RENGLÓN
            // ============================================================

            TextFrame rowFrame = section.AddTextFrame();

            rowFrame.Left =
                Unit.FromCentimeter(leftCm);

            rowFrame.Top =
                Unit.FromCentimeter(topCm);

            rowFrame.Width =
                Unit.FromCentimeter(widthCm);

            rowFrame.Height =
                Unit.FromCentimeter(heightCm);

            rowFrame.RelativeHorizontal =
                RelativeHorizontal.Page;

            rowFrame.RelativeVertical =
                RelativeVertical.Page;

            rowFrame.LineFormat.Width =
                Unit.FromPoint(0.45);

            rowFrame.LineFormat.Color =
                Colors.LightGray;

            // ============================================================
            // FOTOGRAFÍA
            // ============================================================

            string imageFileName =
                GetPartnerImageForPdf(partner);

            if (!string.IsNullOrWhiteSpace(imageFileName))
            {
                try
                {
                    Image image =
                        section.AddImage(imageFileName);

                    image.Left =
                        Unit.FromCentimeter(leftCm + 0.08);

                    image.Top =
                        Unit.FromCentimeter(topCm + 0.08);

                    image.Width =
                        Unit.FromCentimeter(1.30);

                    image.Height =
                        Unit.FromCentimeter(1.70);

                    image.RelativeHorizontal =
                        RelativeHorizontal.Page;

                    image.RelativeVertical =
                        RelativeVertical.Page;

                    image.WrapFormat.Style =
                        WrapStyle.Through;

                    image.LockAspectRatio = false;
                }
                catch
                {
                    AddPartnerDirectoryNoImage(
                        section,
                        leftCm + 0.08,
                        topCm + 0.08
                    );
                }
            }
            else
            {
                AddPartnerDirectoryNoImage(
                    section,
                    leftCm + 0.08,
                    topCm + 0.08
                );
            }

            // ============================================================
            // ECO
            // ============================================================

            AddPartnerDirectoryText(
                section,
                SafePartnerText(partner.Numero),
                leftCm + 1.48,
                topCm + 0.62,
                1.12,
                0.55,
                8.5,
                true,
                ParagraphAlignment.Center
            );

            // ============================================================
            // NOMBRE
            // ============================================================

            string fullName =
                GetPartnerFullNameForPdf(partner);

            AddPartnerDirectoryText(
                section,
                string.IsNullOrWhiteSpace(fullName)
                    ? "SIN NOMBRE"
                    : fullName.ToUpper(),
                leftCm + 2.75,
                topCm + 0.18,
                4.05,
                1.45,
                6.7,
                true,
                ParagraphAlignment.Left
            );

            // ============================================================
            // DIRECCIÓN
            // ============================================================

            string address =
                GetPartnerAddressForPdf(partner);

            AddPartnerDirectoryText(
                section,
                string.IsNullOrWhiteSpace(address)
                    ? "SIN DIRECCIÓN REGISTRADA"
                    : address.ToUpper(),
                leftCm + 6.95,
                topCm + 0.15,
                8.95,
                1.50,
                6.1,
                false,
                ParagraphAlignment.Left
            );

            // ============================================================
            // TELÉFONOS
            // ============================================================

            string phones =
                GetPartnerPhonesForPdf(partner);

            AddPartnerDirectoryText(
                section,
                string.IsNullOrWhiteSpace(phones)
                    ? "SIN TELÉFONO"
                    : phones,
                leftCm + 16.05,
                topCm + 0.25,
                4.05,
                1.30,
                6.5,
                false,
                ParagraphAlignment.Center
            );
        }


        private void AddPartnerDirectoryText(
    Section section,
    string text,
    double leftCm,
    double topCm,
    double widthCm,
    double heightCm,
    double fontSize,
    bool bold,
    ParagraphAlignment alignment)
        {
            TextFrame frame = section.AddTextFrame();

            frame.Left = Unit.FromCentimeter(leftCm);
            frame.Top = Unit.FromCentimeter(topCm);
            frame.Width = Unit.FromCentimeter(widthCm);
            frame.Height = Unit.FromCentimeter(heightCm);

            frame.RelativeHorizontal =
                RelativeHorizontal.Page;

            frame.RelativeVertical =
                RelativeVertical.Page;

            frame.MarginLeft = Unit.FromCentimeter(0.05);
            frame.MarginRight = Unit.FromCentimeter(0.05);
            frame.MarginTop = Unit.FromCentimeter(0.02);
            frame.MarginBottom = Unit.FromCentimeter(0.02);

            Paragraph paragraph = frame.AddParagraph();

            paragraph.Format.Alignment = alignment;
            paragraph.Format.Font.Name = "Arial";
            paragraph.Format.Font.Size = fontSize;
            paragraph.Format.Font.Bold = bold;
            paragraph.Format.SpaceBefore = 0;
            paragraph.Format.SpaceAfter = 0;
            paragraph.Format.LineSpacingRule =
                LineSpacingRule.Single;

            paragraph.AddText(
                string.IsNullOrWhiteSpace(text)
                    ? ""
                    : text
            );
        }

        private void AddPartnerDirectoryNoImage(
    Section section,
    double leftCm,
    double topCm)
        {
            TextFrame frame = section.AddTextFrame();

            frame.Left =
                Unit.FromCentimeter(leftCm);

            frame.Top =
                Unit.FromCentimeter(topCm);

            frame.Width =
                Unit.FromCentimeter(1.30);

            frame.Height =
                Unit.FromCentimeter(1.70);

            frame.RelativeHorizontal =
                RelativeHorizontal.Page;

            frame.RelativeVertical =
                RelativeVertical.Page;

            frame.LineFormat.Width =
                Unit.FromPoint(0.35);

            frame.LineFormat.Color =
                Colors.LightGray;

            frame.FillFormat.Color =
                Color.FromRgb(245, 245, 245);

            frame.MarginTop =
                Unit.FromCentimeter(0.65);

            Paragraph paragraph =
                frame.AddParagraph();

            paragraph.Format.Alignment =
                ParagraphAlignment.Center;

            paragraph.Format.Font.Name =
                "Arial";

            paragraph.Format.Font.Size =
                5;

            paragraph.Format.Font.Color =
                Colors.Gray;

            paragraph.AddText("SIN FOTO");
        }

        /// <summary>
        /// Construye el texto de teléfonos del socio.
        /// </summary>
        private string GetPartnerPhonesForPdf(
            Partner partner)
        {
            if (partner == null)
            {
                return string.Empty;
            }

            List<string> phones = new List<string>();

            if (!string.IsNullOrWhiteSpace(
                partner.Telefono))
            {
                phones.Add(
                    partner.Telefono.Trim()
                );
            }

            if (!string.IsNullOrWhiteSpace(
                partner.celular))
            {
                string cellphone =
                    partner.celular.Trim();

                bool alreadyExists = phones.Any(x =>
                    string.Equals(
                        x,
                        cellphone,
                        StringComparison.OrdinalIgnoreCase
                    )
                );

                if (!alreadyExists)
                {
                    phones.Add(cellphone);
                }
            }

            return string.Join(
                Environment.NewLine,
                phones
            );
        }


        /// <summary>
        /// Obtiene únicamente los primeros socios para probar
        /// el diseño del directorio PDF.
        /// </summary>
        private List<Partner> GetFirstPartnersForDirectoryPdf(int quantity)
        {
            List<Partner> partners = new List<Partner>();

            try
            {
                Partner partnerModel = new Partner();

                PartnerPagination pagination = partnerModel.get(
                    1,
                    quantity,
                    "",
                    "",
                    "",
                    ""
                );

                if (pagination == null ||
                    pagination.Data == null ||
                    pagination.Data.Count == 0)
                {
                    return partners;
                }

                return pagination.Data
                    .Where(x =>
                        x != null &&
                        x.Numero > 0
                    )
                    .Take(quantity)
                    .OrderBy(x => x.Numero)
                    .ThenBy(x => x.Nombre)
                    .ToList();
            }
            catch
            {
                return partners;
            }
        }


    }
}
