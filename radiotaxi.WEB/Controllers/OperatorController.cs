using Amazon.CloudFormation.Model;
using Amazon.DynamoDBv2;
using Amazon.OpsWorks.Model;
using Amazon.SimpleEmail.Model;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using PdfSharp.Drawing;
using radiotaxi.Model;
using radiotaxi.Model.v2.finance;
using radiotaxi.Model.v2.Operator;
using radiotaxi.Model.v2.Partner;
using radiotaxi.WEB.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZXing;
using ZXing.Aztec.Internal;
using ZXing.Common;
using static System.Collections.Specialized.BitVector32;
using static System.Data.Entity.Infrastructure.Design.Executor;
using Image = MigraDoc.DocumentObjectModel.Shapes.Image;
using Orientation = MigraDoc.DocumentObjectModel.Orientation;
using Unit = MigraDoc.DocumentObjectModel.Unit;

namespace radiotaxi.WEB.Controllers
{
    [SessionFilter("Admin,Archivo, Practi-Plusvalia, Practi-Prevision, Practi-Tte")]
    [RoutePrefix("Operator")]
    public class OperatorController : webBaseController
    {
        // GET: Client
        [SessionFilter("Admin, Archivo, Practi-AutoS, tarjeton, Practi-Cajas, Practi-AdminTurkey, Clinica,Practi-Finanzas, Practi-Prevision, Practi-Plusvalia,Practi-Actas, Practi-Tte, Practi-Trabajo, Practi-Viewer, Practi-ViewerOP, Practi-Cred, Practi-BasicView")]
        public ActionResult Index(int? page)
        {
            if (this.OperatorSearchParameters == null) this.OperatorSearchParameters = new OperatorSearchParametersDTO();
            ViewBag.parameters = this.OperatorSearchParameters; // Objeto que contiene los parametros de busqueda, estos se guardan en session pero estransparante para la capa del controlador.
            Operator Operator = new Operator();
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            OperatorPagination operatorPagination = Operator.get(pageNumber, pageSize, this.OperatorSearchParameters.gafet, this.OperatorSearchParameters.name, this.OperatorSearchParameters.lastName1, this.OperatorSearchParameters.lastName2);
            OperatorPagination BajaPagination = Operator.getIfexist(pageNumber, pageSize, this.OperatorSearchParameters.gafet, this.OperatorSearchParameters.name, this.OperatorSearchParameters.lastName1, this.OperatorSearchParameters.lastName2);
            ViewBag.BajaPagination = BajaPagination;

            // Obtener la imagen para N operadores.
            foreach (var (_operator, index) in operatorPagination.Data.Select((v, i) => (v, i)))
            {
                try
                {
                    operatorPagination.Data[index].urlImage = AmazonHelper.getImageUser(_operator.bucket, _operator.key);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            ViewBag.totalClients = operatorPagination.TotalItems;
            return View(operatorPagination);
        }

        [HttpPost]
        [SessionFilter("Admin,Archivo,Practi-AutoS,tarjeton,Practi-Actas,Practi-Cajas,Practi-AdminTurkey, Clinica,Practi-Finanzas, Practi-Prevision, Practi-Plusvalia, Practi-Tte, Practi-Trabajo, Practi-Viewer, Practi-ViewerOP, Practi-Cred, Practi-BasicView")]
        public ActionResult Index(int? page, string gafet, string name, string lastName1, string lastName2)
        {
            this.OperatorSearchParameters = new OperatorSearchParametersDTO();
            this.OperatorSearchParameters.gafet = gafet; // Este es el gafet de operador
            this.OperatorSearchParameters.name = name;
            this.OperatorSearchParameters.lastName1 = lastName1;
            this.OperatorSearchParameters.lastName2 = lastName2;
            ViewBag.parameters = this.OperatorSearchParameters;
            Operator Operator = new Operator();
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            OperatorPagination clientPagination = Operator.get(pageNumber, pageSize, gafet, name, lastName1, lastName2);
            OperatorPagination BajaPagination = Operator.getIfexist(pageNumber, pageSize, gafet, name, lastName1, lastName2);
            ViewBag.BajaPagination = BajaPagination;

            // Obtener la imagen para N operadores.
            foreach (var (_operator, index) in clientPagination.Data.Select((v, i) => (v, i)))
            {
                try
                {
                    clientPagination.Data[index].urlImage = AmazonHelper.getImageUser(_operator.bucket, _operator.key);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            ViewBag.totalClients = clientPagination.TotalItems;
            return View(clientPagination);
        }

        [Route("low/details/{id}")]
        [SessionFilter("Admin, Archivo")]
        public ActionResult lowoperator(int id)
        {
            if (this.OperatorSearchParameters == null) this.OperatorSearchParameters = new OperatorSearchParametersDTO();
            ViewBag.parameters = this.OperatorSearchParameters; // Objeto que contiene los parametros de busqueda, estos se guardan en session pero estransparante para la capa del controlador.
            Operator Operator = new Operator();
            int? page = 0;
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            OperatorPagination operatorPagination = Operator.get(pageNumber, pageSize, this.OperatorSearchParameters.gafet, this.OperatorSearchParameters.name, this.OperatorSearchParameters.lastName1, this.OperatorSearchParameters.lastName2);
            // Obtener la imagen para N operadores.
            foreach (var (_operator, index) in operatorPagination.Data.Select((v, i) => (v, i)))
            {
                try
                {
                    operatorPagination.Data[index].urlImage = AmazonHelper.getImageUser(_operator.bucket, _operator.key);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            ViewBag.totalClients = operatorPagination.TotalItems;
            return View(operatorPagination);
        }

        // GET: Client
        [SessionFilter("Admin, Archivo")]
        public ActionResult low(int? page)
        {

            Operadores_Baja_Datos OP = new Operadores_Baja_Datos();
            if (this.OperatorSearchParameters == null) this.OperatorSearchParameters = new OperatorSearchParametersDTO();
            ViewBag.parameters = this.OperatorSearchParameters; // Objeto que contiene los parametros de busqueda, estos se guardan en session pero estransparante para la capa del controlador.
            Operator Operator = new Operator();
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            OperatorPagination operatorPagination = Operator.get(pageNumber, pageSize, this.OperatorSearchParameters.gafet, this.OperatorSearchParameters.name, this.OperatorSearchParameters.lastName1, this.OperatorSearchParameters.lastName2);
            // Obtener la imagen para N operadores.
            foreach (var (_operator, index) in operatorPagination.Data.Select((v, i) => (v, i)))
            {
                try
                {
                    operatorPagination.Data[index].urlImage = AmazonHelper.getImageUser(_operator.bucket, _operator.key);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            ViewBag.totalClients = operatorPagination.TotalItems;
            return View(operatorPagination);
        }

        [HttpPost]
        [SessionFilter("Admin, Archivo")]
        public ActionResult low(int? page, string gafet, string name, string lastName1, string lastName2)
        {
            this.OperatorSearchParameters = new OperatorSearchParametersDTO();
            this.OperatorSearchParameters.gafet = gafet; // Este es el gafet de operador
            this.OperatorSearchParameters.name = name;
            this.OperatorSearchParameters.lastName1 = lastName1;
            this.OperatorSearchParameters.lastName2 = lastName2;
            ViewBag.parameters = this.OperatorSearchParameters;
            Operator Operator = new Operator();
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            OperatorPagination clientPagination = Operator.get(pageNumber, pageSize, gafet, name, lastName1, lastName2);

            // Obtener la imagen para N operadores.
            foreach (var (_operator, index) in clientPagination.Data.Select((v, i) => (v, i)))
            {
                try
                {
                    clientPagination.Data[index].urlImage = AmazonHelper.getImageUser(_operator.bucket, _operator.key);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            ViewBag.totalClients = clientPagination.TotalItems;
            return View(clientPagination);
        }

        // METODOS PRIVADOS
        // Métodos simulados con parámetros
        private string getImage(Operator _operator)
        {

            if (_operator != null && _operator.bucket != null && _operator.key != null)
            {
                return AmazonHelper.getImageUser(_operator.bucket, _operator.key);
            }
            return "";
        }

        private List<card> getCards(string gafet)
        {
            // Obtenemos todos los tarjetons del operador.
            card _card = new card();
            return _card.get(gafet);
        }

        private List<HistAsistencia> getReports(Operator _operator)
        {
            // Obtenemos los reportes 
            List<HistAsistencia> _reportes = _operator.reportes();
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

        private List<MENSAJE> getMessage(string gafet)
        {
            try
            {
                // Obtenemos los mensajes Bloqueos
                MENSAJE mENSAJE = new MENSAJE();
                return mENSAJE.getGafet(gafet);
            }
            catch (Exception ex)
            {
                return new List<MENSAJE>();
            }
        }

        private List<desbloqueo> getBlock(string gafet)
        {
            try
            {
                // Obtenemos los mensajes Bloqueos
                MENSAJE mENSAJE = new MENSAJE();
                return mENSAJE.Desbloqueos(gafet);
            }
            catch (Exception ex)
            {
                return new List<desbloqueo>();
            }
        }

        private financesOperator getFinances(Operator _operator)
        {
            try
            {
                // Obtener faltas
                return _operator.financesOperator();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<pay> getPays(Operator _operator)
        {
            try
            {
                // Obtener faltas
                return _operator.payCards();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<Convenio> getAgreements(Operator _operator) {
            try
            {
                List<Convenio> agreements = _operator.agreements();
                return agreements;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<C_permissionsDTO> permision(string gafet) {
            try
            {
                C_permissions permissions = new C_permissions();
                return permissions.get(gafet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [SessionFilter("Admin, Archivo, Practi-AutoS, tarjeton, Practi-Actas, Practi-Cajas, Practi-AdminTurkey, Practi-Finanzas, Practi-Prevision, Clinica, Practi-Plusvalia, Practi-Tte, Practi-Trabajo, Practi-Viewer, Practi-ViewerOP, Practi-Cred, Practi-BasicView")]
        [Route("tarjeton/{gafet}")]
        public async Task<ActionResult> tarjeton(string gafet)
        {
            try
            {
                Operator _operator = new Operator();
                _operator = _operator.get(gafet); // Obtenemos la informacion del operador.
                var imageTask = Task.Run(() => getImage(_operator)); // Obtenemos la tarea para obtener la imagen.
                var CardsTask = Task.Run(() => getCards(gafet)); // Obtenemos la tarea para obtener la imagen.
                var ReportsTask = Task.Run(() => getReports(_operator)); // Obtenemos la tarea para obtener la imagen.
                var MessageTask = Task.Run(() => getMessage(gafet)); // Obtenemos la tarea para obtener la imagen.
                var BlockTask = Task.Run(() => getBlock(gafet)); // Obtenemos la tarea para obtener la imagen.
                var financesTask = Task.Run(() => getFinances(_operator)); // Obtenemos la tarea para obtener la imagen.
                var getPaysTask = Task.Run(() => getPays(_operator)); // Obtenemos la tarea para obtener la imagen.
                var getAgreementsTask = Task.Run(() => getAgreements(_operator)); // Obtenemos la tarea para obtener la imagen.
                var permisionTask = Task.Run(() => permision(gafet)); // Obtenemos la tarea para obtener la imagen.
                // Obtenemos los pagos de cupones para el operador.
                var CuponspaymentsTask = Task.Run(() => getCuponsPayments(_operator.userId));

                await Task.WhenAll(imageTask, CardsTask, ReportsTask, MessageTask, BlockTask, financesTask, getPaysTask, getAgreementsTask, permisionTask, CuponspaymentsTask);

                _operator.urlImage = imageTask.Result; // Asignamos la imagen.
                ViewBag.cars = CardsTask.Result; //Enviamos la lista por medio de un viewBag
                ViewBag.reportes = ReportsTask.Result;
                ViewBag.message = MessageTask.Result;
                ViewBag.desbloqueos = BlockTask.Result;
                ViewBag.financesOperator = financesTask.Result;
                ViewBag.Porcentaje = financesTask.Result.porcent;
                ViewBag.faltas = financesTask.Result.faltas;
                ViewBag.pays = getPaysTask.Result;
                ViewBag.agreements = getAgreementsTask.Result;
                ViewBag.permissions = permisionTask.Result;


                ViewBag.Cuponspayments = CuponspaymentsTask.Result;
                // DEUDAS
                Sale sale = new Sale(this.email);
                List<caja_cargos_DTO> cargos = sale.debs(gafet, gafet);
                ViewBag.debs = cargos == null ? new List<caja_cargos_DTO>() : cargos;
                
                decimal amountdebs = 0;
                foreach (caja_cargos_DTO deb in cargos ?? new List<caja_cargos_DTO>()) {
                    amountdebs += (deb.DEBE * deb.PRECIO);
                }
                
                ViewBag.amountdebs = amountdebs;
                
                // Obtener los tickets
                List<Venta> sales = _operator.sales();
                ViewBag.sales = sales;

                // PAVOS
                Christmasgift christmasgift = new Christmasgift();
                christmasgift = christmasgift.get(_operator.userId);
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
                return View(_operator);
            }
            catch (Exception ex)
            {
                return Redirect("/Error/EcoSoc");
            }
        }

        // GET: Operator/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Operator/Create
        [SessionFilter("Admin, Archivo")]
        public ActionResult Create()
        {
            ViewBag.partnerTypes = new partnerType().get();
            ViewBag.relationships = new relationShipStatu().get();
            ViewBag.states = new state().get();
            ViewBag.cities = new city().get();
            return View();
        }

        // POST: Operator/Create
        [HttpPost]
        [SessionFilter("Admin, Archivo")]
        public ActionResult Create(Operator Obj, HttpPostedFileBase file)
        {
            try
            {
                // Variables de control.
                Obj.createdBy = this.UserName;
                Obj.createdDt = DateTime.Now;
                Obj.active = true;
                Operator _operator = Obj.save(); // Se guarda en la base de datos... 
                if (_operator == null)
                {
                    ViewBag.partnerTypes = new partnerType().get();
                    ViewBag.relationships = new relationShipStatu().get();
                    ViewBag.states = new state().get();
                    ViewBag.cities = new city().get();
                    ViewBag.message = "Error al crear operador.";
                    return View();
                }
                if (file != null)
                {
                    try
                    {
                        if (file.ContentLength > 0)
                        {
                            amazonPicture AmazonPicture = AmazonHelper.savePictureWebSite(_operator.userId, _operator.partnerReference, file);
                            if (AmazonPicture != null)
                            {
                                AmazonPicture.userId = _operator.userId; // Seteamos a quien le pertenece esa imagen.
                                AmazonPicture.Message = "";
                                AmazonPicture.DtCreated = DateTime.Now;
                                AmazonPicture.pictureTypeId = 2;
                                AmazonPicture.save();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ViewBag.errorImage = e.Message;
                    }
                }
                return Redirect("~/Operator/tarjeton/" + Obj.partnerReference);
            }
            catch
            {
                return null;
            }
        }

        // POST: Operator/GetCities
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
            catch (JsonWriterException ex) {
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

        // GET: Operator/Edit/5
        [Route("Edit/{gafet}")]
        [SessionFilter("Admin, Archivo")]
        public ActionResult Edit(string gafet)
        {
            userAddress userAddress = null;
            List<city> cities = new city().get();
            int stateId = 0;
            int cityId = 0;
            // Obtenemos la informacion del operador.
            Operator _operator = new Operator();
            _operator = _operator.get(gafet);
            if (_operator != null && _operator.bucket != null && _operator.key != null)
            {
                _operator.urlImage = AmazonHelper.getImageUser(_operator.bucket, _operator.key);
            }
            if (_operator != null)
            {
                ViewBag.message = "";
                // Obtenemos la direccion
                try
                {
                    userAddress = _operator.userAddresses.FirstOrDefault();
                    if (userAddress != null)
                    {
                        if (userAddress.cityId != null)
                        {
                            cityId = (int)userAddress.cityId;
                        }
                        else {
                            cityId = 2657;
                        }
                        // Obtenemos la ciudad.
                        city City = new city().getcity(cityId);
                        cities = City.get(City.stateId);
                        stateId = City.stateId;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                if (_operator != null && _operator.bucket != null && _operator.key != null)
                {
                    _operator.urlImage = AmazonHelper.getImageUser(_operator.bucket, _operator.key);
                }
            }
            else {
                ViewBag.message = "Error : falta información para ver el objeto";
            }
            ViewBag.cities = cities;
            ViewBag.stateId = stateId;
            ViewBag.cityId = cityId;
            ViewBag.userAddress = userAddress;
            ViewBag.partnerTypes = new partnerType().get();
            ViewBag.relationships = new relationShipStatu().get();
            ViewBag.states = new state().get();
            ViewBag.message = "";
            return View(_operator);
        }

        // POST: Operator/Edit/5
        [HttpPost]
        [Route("Edit/{gafet}")]
        [SessionFilter("Admin, Archivo")]
        public ActionResult Edit(string gafet, Operator Obj, HttpPostedFileBase file)
        {
            try
            {
                Obj.editBy = this.UserName;
                Obj.dtLastUpdate = DateTime.Now;
                Obj.update();// Actualizamos el objeto.
                userAddress userAddress = null;
                List<city> cities = new city().get();
                int stateId = 0;
                int cityId = 0;
                // Obtenemos la informacion del operador.
                Operator _operator = new Operator();
                _operator = _operator.get(gafet);
                if (_operator != null)
                {
                    ViewBag.message = "";
                    // Obtenemos la direccion
                    try
                    {
                        userAddress = _operator.userAddresses.FirstOrDefault();
                        if (userAddress != null)
                        {
                            // Obtenemos la ciudad.
                            cityId = (int)userAddress.cityId;
                            city City = new city().getcity(cityId);
                            cities = City.get(City.stateId);
                            stateId = City.stateId;
                        }

                        // Actualizacion de foto
                        if (file != null)
                        {
                            try
                            {
                                if (file.ContentLength > 0)
                                {
                                    amazonPicture AmazonPicture = AmazonHelper.savePictureWebSite(_operator.userId, _operator.partnerReference, file);
                                    if (AmazonPicture != null)
                                    {
                                        AmazonPicture.userId = _operator.userId; // Seteamos a quien le pertenece esa imagen.
                                        AmazonPicture.Message = "";
                                        AmazonPicture.DtCreated = DateTime.Now;
                                        AmazonPicture.pictureTypeId = 2;
                                        // Como se crea uno nuevo simplemente se acutaliza el objeto.
                                        if (_operator.amazonPictures.Where(x => x.pictureTypeId == 2).FirstOrDefault() != null)
                                        {
                                            amazonPicture a = _operator.amazonPictures.Where(x => x.pictureTypeId == 2).FirstOrDefault();
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
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    if (_operator != null && _operator.bucket != null && _operator.key != null)
                    {
                        _operator.urlImage = AmazonHelper.getImageUser(_operator.bucket, _operator.key);
                    }
                }
                else
                {
                    ViewBag.message = "Error : falta información para ver el objeto";
                }
                ViewBag.cities = cities;
                ViewBag.stateId = stateId;
                ViewBag.cityId = cityId;
                ViewBag.userAddress = userAddress;
                ViewBag.partnerTypes = new partnerType().get();
                ViewBag.relationships = new relationShipStatu().get();
                ViewBag.states = new state().get();
                ViewBag.message = "Guardado correctamente";
                return Redirect("~/Operator/tarjeton/" + _operator.partnerReference);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        [Route("QuickEdit/{gafet}")]
        [SessionFilter("Admin, tarjeton")]
        public ActionResult QuickEdit(string gafet, Operator Obj, HttpPostedFileBase file)
        {
            try
            {
                Obj.editBy = this.UserName;
                Obj.dtLastUpdate = DateTime.Now;
                Obj.update();// Actualizamos el objeto.
                userAddress userAddress = null;
                List<city> cities = new city().get();
                int stateId = 0;
                int cityId = 0;
                // Obtenemos la informacion del operador.
                Operator _operator = new Operator();
                _operator = _operator.get(gafet);
                if (_operator != null)
                {
                    ViewBag.message = "";
                    // Obtenemos la direccion
                    try
                    {
                        userAddress = _operator.userAddresses.FirstOrDefault();
                        if (userAddress != null)
                        {
                            // Obtenemos la ciudad.
                            cityId = (int)userAddress.cityId;
                            city City = new city().getcity(cityId);
                            cities = City.get(City.stateId);
                            stateId = City.stateId;
                        }

                        // Actualizacion de foto
                        if (file != null)
                        {
                            try
                            {
                                if (file.ContentLength > 0)
                                {
                                    amazonPicture AmazonPicture = AmazonHelper.savePictureWebSite(_operator.userId, _operator.partnerReference, file);
                                    if (AmazonPicture != null)
                                    {
                                        AmazonPicture.userId = _operator.userId; // Seteamos a quien le pertenece esa imagen.
                                        AmazonPicture.Message = "";
                                        AmazonPicture.DtCreated = DateTime.Now;
                                        AmazonPicture.pictureTypeId = 2;
                                        // Como se crea uno nuevo simplemente se acutaliza el objeto.
                                        if (_operator.amazonPictures.Where(x => x.pictureTypeId == 2).FirstOrDefault() != null)
                                        {
                                            amazonPicture a = _operator.amazonPictures.Where(x => x.pictureTypeId == 2).FirstOrDefault();
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
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    if (_operator != null && _operator.bucket != null && _operator.key != null)
                    {
                        _operator.urlImage = AmazonHelper.getImageUser(_operator.bucket, _operator.key);
                    }
                }
                else
                {
                    ViewBag.message = "Error : falta información para ver el objeto";
                }
                ViewBag.cities = cities;
                ViewBag.stateId = stateId;
                ViewBag.cityId = cityId;
                ViewBag.userAddress = userAddress;
                ViewBag.partnerTypes = new partnerType().get();
                ViewBag.relationships = new relationShipStatu().get();
                ViewBag.states = new state().get();
                ViewBag.message = "Guardado correctamente";
                return Redirect("~/Operator/tarjeton/" + _operator.partnerReference);
            }
            catch
            {
                return null;
            }
        }

        // GET: Operator/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Operator/Delete/5
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

        [HttpPost]
        public ActionResult GeneratePdfDef(string gefet)
        {
            try
            {
                Operator _operator = new Operator().get(gefet); // Obtenemos la informacion del operador.
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
                Operator _operator = new Operator().get(gafet);
                Dictionary<string, object> JsonObj = new Dictionary<string, object>();
                try
                {
                    if (_operator.bucket != null)
                    {
                        // Obtener la imagen.
                        _operator.urlImage = AmazonHelper.getImageUser(_operator.bucket, _operator.key);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                JsonObj.Add("Car", _emplacamiento);
                JsonObj.Add("Operator", _operator);
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
                Operator _operator = new Operator();
                _operator = _operator.get(op.ToUpper());
                EmplacamientoDTO emplacamientoDTO = new Emplacamiento().get(taxi);// Obtenemos los datos el carro.
                card Card = new card();
                Card.turn = turn;
                Card.deliver = false;
                Card.low = false;
                Card.expiration_date = DateTime.Now;
                Card.asgnedBy = this.UserName;
                Card.asignedDate = DateTime.Now;
                Card.isActive = true;
                Card.userId = _operator.userId; // El id del padron
                Card.partnerReference = op; // El numero de gafet del operador
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

        /////////////////////////////////////////
        [HttpGet]
        [SessionFilter("Admin, tarjeton")]
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

        //////// UTILIDADES        
        [HttpGet]
        [SessionFilter("Admin, Archivo, Practi-Cred")]
        [Route("Identity/{gafet}")]
        public ActionResult Identity(string gafet)
        {
            try
            {
                ViewBag.error = "";
                Operator _operator = new Operator();
                _operator = _operator.get(gafet.ToUpper());
                try
                {
                    if (_operator != null && _operator.bucket != null && _operator.key != null)
                    {
                        _operator.urlImage = AmazonHelper.getImageUser(_operator.bucket, _operator.key);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Operadores_Baja_Datos _baja_operatorr = _operator.getIdentity();
                ViewBag.baja = _baja_operatorr;
                return View(_operator);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        [HttpPost]
        [SessionFilter("Admin, Archivo, Practi-Cred")]
        [Route("Identity/{gafet}")]
        public ActionResult Identity(FormCollection data, string gafet)
        {
            try
            {
                Operator _operator = new Operator();
                _operator = _operator.get(gafet.ToUpper());
                try
                {
                    _operator.identityReturn();
                }
                catch (Exception ex)
                {
                    _operator = new Operator();
                    _operator = _operator.get(gafet.ToUpper());
                    ViewBag.error = "";
                    return Redirect("~/Operator/Identity/" + _operator.partnerReference);
                }
                return Redirect("~/Operator/tarjeton/" + _operator.partnerReference);
            }
            catch (Exception ex)
            {
                Operator _operator = new Operator();
                _operator = _operator.get(gafet.ToUpper());
                ViewBag.error = "";
                return Redirect("~/Operator/Identity/" + _operator.partnerReference);
            }
        }

        // BAJAS
        [SessionFilter("Admin, Archivo")]
        public ActionResult Lows()
        {
            return View();
        }

        [HttpPost]
        [SessionFilter("Admin")]
        [Route("search/")]
        public ActionResult saerch(string reference, string name, string lastname)
        {
            try
            {
                int? page = null;
                ViewBag.parameters = this.OperatorSearchParameters;
                Operator Operator = new Operator();
                int statusConnection = 0;
                int pageSize = 1000;
                int pageNumber = (page ?? 1);
                OperatorPagination clientPagination = Operator.get(pageNumber, pageSize, reference, name, lastname, lastname);
                // Obtener la imagen para N operadores.
                foreach (var (_operator, index) in clientPagination.Data.Select((v, i) => (v, i)))
                {
                    try
                    {
                        clientPagination.Data[index].urlImage = AmazonHelper.getImageUser(_operator.bucket, _operator.key);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                return Content(this.serialize(clientPagination), "application/json");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // PERMISOS
        [SessionFilter("Admin,Archivo")]
        public ActionResult Permits()
        {
            try
            {
                permiso Permit = new permiso();
                List<permiso> list = Permit.getall();
                return View(list);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [SessionFilter("Admin, Archivo, Practi-Plusvalia, Practi-Tte, Practi-Trabajo, Practi-AutoS")]
        [HttpGet]
        [Route("bloqueos/{gafet}")]
        public ActionResult Bloqueos(string gafet)
        {
            Operator _operator = new Operator();
            _operator = _operator.get(gafet);
            MENSAJE message = new MENSAJE();
            ViewBag.Desbloqueos = new List<desbloqueo>();
            ViewBag.messages = new List<MENSAJE>();

            if (_operator != null)
            {
                ViewBag.messages = message.getGafet(gafet); // Obtenemos los mensajes.
                ViewBag.Desbloqueos = message.Desbloqueos(gafet);
            }
            if (_operator != null && _operator.bucket != null && _operator.key != null)
            {
                _operator.urlImage = AmazonHelper.getImageUser(_operator.bucket, _operator.key);
            }
            return View(_operator);
        }

        [SessionFilter("Admin, Archivo, Practi-Prevision, Practi-Plusvalia, Practi-Tte, Practi-Trabajo, Practi-AutoS")]
        [HttpPost]
        [Route("bloqueos/op/{gafet}")]
        public ActionResult Bloqueosop(string gafet, FormCollection collection)
        {
            MENSAJE message = new MENSAJE();
            Operator _operator = new Operator();
            try
            {
                //string fbloqueo = collection["fbloqueo"];
                string detener = collection["detener"];
                string Message = collection["mensaje"];
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
                message.bloquearOperador(gafet, Message, this.User.firstName + " " + this.User.lastName, secretaria, detener, id_Secretaria);
            }
            catch (Exception ex)
            {
                // REDIRECCIONAR A PAGINA DE ERROR.
            }
            
            _operator = _operator.get(gafet);
            ViewBag.Desbloqueos = new List<desbloqueo>();
            ViewBag.messages = new List<MENSAJE>();

            if (_operator != null)
            {
                ViewBag.messages = message.getGafet(gafet); // Obtenemos los mensajes.
                ViewBag.Desbloqueos = message.Desbloqueos(gafet);
            }
            if (_operator != null && _operator.bucket != null && _operator.key != null)
            {
                _operator.urlImage = AmazonHelper.getImageUser(_operator.bucket, _operator.key);
            }
            return Redirect("~/Operator/Bloqueos/" + gafet);
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
                SecretariasforMensaje sec = message.Secretarias().Where(x => x.Id_Ubicacion == this.User.id_secretaria).FirstOrDefault();
                Dictionary<string, object> JsonObj = new Dictionary<string, object>();
                
                

                
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
                JsonObj.Add("response", response);
                return Content(this.serialize(JsonObj), "application/json");
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // PERMISOS
        [SessionFilter("Admin, Archivo, Practi-Actas, Practi-Prevision, Practi-Plusvalia, Practi-Trabajo")]
        public ActionResult PermitsList()
        {
            List<C_permissionsDTO> permissions = new List<C_permissionsDTO>(); 
            C_permissions permit = new C_permissions();

            //permissions = permit.getall();
            //ViewBag.permissions = permissions;
            //return View();

            permissions = permit.vencidos();
            return View(permissions);

        }

        // PERMISOS

        [SessionFilter("Admin, Archivo, Practi-Actas, Practi-Prevision, Practi-Plusvalia, Practi-Trabajo")]
        [Route("permisos/vencidos")]
        public ActionResult permissionspending()
        {
            // En este apartado obtenemos los operaodres que estan por vencer.
            List<C_permissions> permissions = new List<C_permissions>();
            C_permissions permit = new C_permissions();
            permissions = permit.getall();
            return View(permissions);
        }

        // PERMISOS

        [SessionFilter("Admin, Archivo, Practi-Plusvalia, Practi-Prevision")]
        public ActionResult CreatePermits()
        {
            List<C_permissions> permissions = new List<C_permissions>();
            C_permissions permit = new C_permissions();
            permissions = permit.getall();
            return View(permissions);
        }

        [HttpPost]

        [SessionFilter("Admin, Archivo, Practi-Plusvalia, Practi-Prevision")]
        public ActionResult CreatePermits(C_permissions obj)
        {
            // Guardar el objeto y crear los procesos de los cargos en practicontrol.
            obj.CreatedBy = this.User.firstName + " " + this.User.lastName;
            obj.dateCreated = DateTime.Now;
            obj.save(); // Aqui adentro hacemos todo procesos.
            return View();
        }

        // PERMISOS
        [HttpGet]
        [Route("permiso/detalle/{id}/{gafet}")]
        public ActionResult PermisoDetalle(int id, string gafet)
        {
            ViewBag.Message = "";
            if (this.Message != null && this.Message.Id == 10)
            {
                ViewBag.Message = this.Message.Message;
                // RESETEO.
                this.Message = null;
            }
            Operator _operator = new Operator();
            _operator = _operator.get(gafet);
            ViewBag.operador = _operator;
            // PERMISOS EXPRESS.
            C_permissions permissions = new C_permissions();
            ViewBag.permissions = permissions.get(id);
            return View();
        }

        // PERMISOS
        [HttpPost]
        [Route("permiso/generar")]
        public ActionResult savePrinter(FormCollection data)
        {
            try
            {
                ViewBag.message = "";
                ViewBag.empla = "";
                // OBtenemos los datos del operador
                string taxi = data["taxi"];
                string gafet = data["gafet"];
                int id = Int32.Parse(data["id"]);
                Operator _operator = new Operator();
                _operator = _operator.get(gafet);
                ViewBag.operador = _operator;
                Emplacamiento empla = new Emplacamiento();
                EmplacamientoDTO emplcado = empla.get(taxi);
                // Obtenemos los datos del taxi (Socio) / De emplacamiento o de soc
                if (emplcado != null)
                {
                    // PERMISOS EXPRESS.
                    C_permissions permissions = new C_permissions();
                    permissions = permissions.get(id);
                    permissions.taxi = taxi;
                    permissions.datePrinted = DateTime.Now;
                    permissions.PrintedByUser = this.UserName;
                    permissions.update();
                    return Redirect("~/Operator/permiso/detalle/" + permissions.id + "/" + _operator.partnerReference);
                }
                else {
                    MessageDTO message = new MessageDTO();
                    message.Message = "Vehiculo no se encuentra emplacado o no existe en la base de datos.";
                    message.Id = 10;
                    message.Type = 500;
                    this.Message = message;
                    ViewBag.empla = emplcado;
                    return Redirect("~/Operator/permiso/detalle/" + id + "/" + _operator.partnerReference);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Redireccionar a error.
                return Redirect("~/Operator/tarjeton/");
            }
        }

        [Route("permiso/print/{id}")]
        public ActionResult PrintPermits(int id)
        {
            // PERMISOS EXPRESS.
            C_permissions permissions = new C_permissions();
            permissions = permissions.get(id);

            Operator _operator = new Operator();
            _operator = _operator.get(permissions.gafet);

            Partner _partner= new Partner();
            _partner = _partner.get(permissions.taxi);

            Emplacamiento emplacamiento = new Emplacamiento();
            List<Emplacamiento> emplacados = emplacamiento.getHistory(permissions.taxi);
            var resp = emplacados.Where(x => x.Operacion == "EMPLACADO").FirstOrDefault();

            if (_operator != null && _operator.bucket != null && _operator.key != null)
            {
                _operator.urlImage = AmazonHelper.getImageUser(_operator.bucket, _operator.key);
            }

            // QR
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 270,
                    Width = 270,
                    Margin = 1
                }
            };

            // Codigo de barras.
            BarcodeWriter writer2 = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    PureBarcode = true,
                    Height = 100,
                    Width = 270,
                    Margin = 1,
                    

                }
            };

            // Prefijo e de express
            var result = writer.Write("e" + id);
            var result2 = writer2.Write("e" + id);
            System.Drawing.Bitmap qrCodeImage = new System.Drawing.Bitmap(result); // CODIGO QR
            System.Drawing.Bitmap BarCodeImage = new System.Drawing.Bitmap(result2); // CODIGO DE BARRAS
                                                                                     // Obtenida toda la informacion procedemos a renderizar el tarjeton...
            // Fecha vencimiento.
            string expDate = permissions.dateEnd?.ToShortDateString().ToString();

            Document document = new Document();
            MigraDoc.DocumentObjectModel.Section section = document.AddSection();
            section.PageSetup.Orientation = Orientation.Landscape;
            section.PageSetup.PageHeight = "14cm";
            section.PageSetup.PageWidth = "11cm";

            // Cabecera
            Image myImage = section.Headers.Primary.AddImage(Server.MapPath("~/Content/images/encabezado.png"));
            myImage.RelativeVertical = RelativeVertical.Page;
            myImage.RelativeHorizontal = RelativeHorizontal.Page;
            myImage.Left = "0.15cm";
            myImage.Top = "0.15cm";
            myImage.Width = "13.7cm";
            myImage.WrapFormat.Style = WrapStyle.Through;

            // Separador
            Image separador = section.Headers.Primary.AddImage(Server.MapPath("~/Content/images/background.png"));
            separador.RelativeVertical = RelativeVertical.Page;
            separador.RelativeHorizontal = RelativeHorizontal.Page;
            separador.Left = "0.15cm";
            separador.Top = "0.15cm";
            separador.Width = "13.7cm";
            separador.WrapFormat.Style = WrapStyle.Through;

            //Image Face
            byte[] imageStream = AmazonHelper.GetImageStream(_operator.urlImage);
            Image image = section.AddImage(AmazonHelper.MigraDocFilenameFromByteArray(imageStream));
            int height = 0;
            using (PdfSharp.Drawing.XImage image2 = PdfSharp.Drawing.XImage.FromGdiPlusImage(AmazonHelper.toImage(imageStream)))
            {
                height = image2.PixelHeight;
            }
            image.Height = "3.75cm";
            image.Width = "3.25cm";
            image.Top = "2.5cm";
            image.Left = "0.37cm";

            image.RelativeVertical = RelativeVertical.Page;
            image.RelativeHorizontal = RelativeHorizontal.Page;
            image.WrapFormat.Style = WrapStyle.Through;

            /*
            // Folio
            TextFrame fol = section.AddTextFrame();
            fol.Width = "10cm";
            fol.Height = "5cm";
            fol.RelativeVertical = RelativeVertical.Page;
            fol.RelativeHorizontal = RelativeHorizontal.Page;
            fol.Top = "5.8cm";
            fol.Left = "0.37cm";
            Paragraph paragraph9 = fol.AddParagraph();
            paragraph9.Format.Font.Size = new Unit(10);
            paragraph9.AddText("FOLIO: e".ToUpper() + id);
            paragraph9.Format.Font.Name = "Roboto";
            paragraph9.Format.Font.Bold = true;
            paragraph9.Format.SpaceAfter = "0.2cm";
            paragraph9.Format.Font.Color = Colors.Black;
            */

            TextFrame tfo3 = section.AddTextFrame();
            tfo3.Width = "70mm";
            tfo3.Height = "20mm";
            tfo3.RelativeVertical = RelativeVertical.Page;
            tfo3.RelativeHorizontal = RelativeHorizontal.Page;
            tfo3.Top = "6.3cm"; // Misma altura
            tfo3.Left = "0.37cm"; // Posición horizontal diferente
            Paragraph paragraph8 = tfo3.AddParagraph(); // Se agrega al TextFrame correcto
            paragraph8.Format.Font.Size = new Unit(9);
            paragraph8.Format.Font.Name = "Roboto";
            paragraph8.Format.Font.Bold = true;
            paragraph8.Format.SpaceAfter = "0.2cm";
            paragraph8.Format.Font.Color = Colors.Black;
            paragraph8.AddFormattedText("PREGAFET: " + _operator.partnerReference + "\n", TextFormat.Bold);
            paragraph8.AddFormattedText(_operator.firstName + "\n", TextFormat.Bold); 
            paragraph8.AddFormattedText(_operator.lastNameF + "\n", TextFormat.Bold);
            paragraph8.AddFormattedText(_operator.lastNameM + "\n", TextFormat.Bold);

            Image objImgLogo = section.AddImage(AmazonHelper.MigraDocFilenameFromByteArray(AmazonHelper.ToByteArray(qrCodeImage)));
            objImgLogo.Height = "2.75cm";
            objImgLogo.Width = "2.75cm";
            objImgLogo.RelativeVertical = RelativeVertical.Page;
            objImgLogo.RelativeHorizontal = RelativeHorizontal.Page;
            objImgLogo.Top = "8cm";
            objImgLogo.Left = "0.20cm";
            objImgLogo.WrapFormat.Style = WrapStyle.Through;
            BarCodeImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            Image objImgBarcode = section.AddImage(AmazonHelper.MigraDocFilenameFromByteArray(AmazonHelper.ToByteArray(BarCodeImage)));
            objImgBarcode.Height = "30mm";
            objImgBarcode.Width = "7.5mm";
            objImgBarcode.RelativeVertical = RelativeVertical.Page;
            objImgBarcode.RelativeHorizontal = RelativeHorizontal.Page;
            objImgBarcode.Top = "7.9cm";
            objImgBarcode.Left = "2.90cm";
            objImgBarcode.WrapFormat.Style = WrapStyle.Through;

            // Titulo
            TextFrame tf = section.AddTextFrame();
            tf.Width = "10cm";
            tf.Height = "5cm";
            tf.RelativeVertical = RelativeVertical.Page;
            tf.RelativeHorizontal = RelativeHorizontal.Page;
            tf.Top = "2.5cm";
            tf.Left = "4.3cm";
            Paragraph paragraph = tf.AddParagraph();
            paragraph.Format.Font.Size = new Unit(13);
            paragraph.AddText("PreGafet".ToUpper());
            paragraph.Format.Font.Name = "Roboto";
            paragraph.Format.Font.Bold = true;
            paragraph.Format.SpaceAfter = "0.2cm";
            paragraph.Format.Font.Color = Colors.Black;

            // Parrafo 1
            TextFrame tp = section.AddTextFrame();
            tp.Width = "90mm";
            tp.Height = "50mm";
            tp.RelativeVertical = RelativeVertical.Page;
            tp.RelativeHorizontal = RelativeHorizontal.Page;
            tp.Top = "3.3cm";
            tp.Left = "4.3cm";
            Paragraph paragraph3 = tp.AddParagraph();
            paragraph3.Format.Font.Size = new Unit(11);
            paragraph3.Format.Alignment = ParagraphAlignment.Justify; 
            paragraph3.AddFormattedText("Se autoriza 30 días al ".ToUpper());
            paragraph3.AddFormattedText("C. " + _operator.NOMBRE + " " + _operator.APELLIDOS, TextFormat.Bold | TextFormat.Italic | TextFormat.Underline);
            paragraph3.AddFormattedText(" para prestar el servicio público en el ".ToUpper());
            paragraph3.AddFormattedText("TAXI " + permissions.taxi, TextFormat.Bold | TextFormat.Italic |TextFormat.Underline);
            paragraph3.AddFormattedText(". Con la licencia vigente, se compromete a respetar los reglamentos internos y a mantener una buena conducta dentro y fuera de la unidad respetando a todos sus compañeros y delegados.\t\t\t\t\t                                 \n\n".ToUpper());
            //paragraph3.AddFormattedText("\tNO DADO DE ALTA (REGISTRO NUEVO ESQUEMA)\t\n\n", TextFormat.Bold);
            // paragraph3.AddFormattedText("NO DADO DE ALTA", TextFormat.Bold | TextFormat.Underline);
            //paragraph3.AddFormattedText("\t\tCON DEUDA ", TextFormat.Bold);
            paragraph3.Format.Font.Name = "Roboto";
            paragraph3.Format.Font.Bold = true;
            paragraph3.Format.SpaceAfter = "0.2cm";
            paragraph3.Format.Font.Color = Colors.Black;

            // Fechas
            //Inicial
            TextFrame fi = section.AddTextFrame();
            fi.Width = "138mm";
            fi.Height = "50mm";
            fi.RelativeVertical = RelativeVertical.Page;
            fi.RelativeHorizontal = RelativeHorizontal.Page;
            fi.Top = "2.5cm";
            fi.Left = "7.5cm";
            Paragraph paragraph11 = fi.AddParagraph();
            paragraph11.Format.Font.Size = new Unit(8);
            //paragraph2.AddText(("Cancun Quintana Roo a " + DateTime.Now.ToString("dd-MM-yyyy", new System.Globalization.CultureInfo("es-ES"))).ToUpper());
            paragraph11.AddText(("INICIA: " + permissions.dateStart?.ToString("dd-MM-yyyy", new System.Globalization.CultureInfo("es-ES"))).ToUpper());
            paragraph11.Format.Font.Name = "Roboto";
            paragraph11.Format.Font.Bold = true;
            paragraph11.Format.SpaceAfter = "0.2cm";
            paragraph11.Format.Font.Color = Colors.Black;

            //Vence
            TextFrame ff = section.AddTextFrame();
            ff.Width = "138mm";
            ff.Height = "50mm";
            ff.RelativeVertical = RelativeVertical.Page;
            ff.RelativeHorizontal = RelativeHorizontal.Page;
            ff.Top = "2.5cm";
            ff.Left = "10.8cm";
            Paragraph paragraph2 = ff.AddParagraph();
            paragraph2.Format.Font.Size = new Unit(8);
            //paragraph2.AddText(("Cancun Quintana Roo a " + DateTime.Now.ToString("dd-MM-yyyy", new System.Globalization.CultureInfo("es-ES"))).ToUpper());
            paragraph2.AddText(("VENCE: " + permissions.dateEnd?.ToString("dd-MM-yyyy", new System.Globalization.CultureInfo("es-ES"))).ToUpper());
            paragraph2.Format.Font.Name = "Roboto";
            paragraph2.Format.Font.Bold = true;
            paragraph2.Format.SpaceAfter = "0.2cm";
            paragraph2.Format.Font.Color = Colors.Black;

            
            // Footer
            TextFrame tfo = section.AddTextFrame();
            tfo.Width = "50mm";
            tfo.Height = "20mm";
            tfo.RelativeVertical = RelativeVertical.Page;
            tfo.RelativeHorizontal = RelativeHorizontal.Page;
            tfo.Top = "100mm"; // Misma altura
            tfo.Left = "43mm"; // Posición horizontal

            Paragraph paragraph6 = tfo.AddParagraph(); // Se agrega al TextFrame correcto
            paragraph6.Format.Font.Size = new Unit(7);
            paragraph6.Format.Alignment = ParagraphAlignment.Center;
            paragraph6.AddFormattedText("C. Julian Romero Alcocer\n\n".ToUpper(), TextFormat.Bold).ToString();
            paragraph6.AddFormattedText("SECRETARIO DE TRABAJO");
            paragraph6.Format.Font.Name = "Roboto";
            paragraph6.Format.Font.Bold = true;
            paragraph6.Format.SpaceAfter = "0.2cm";
            paragraph6.Format.Font.Color = Colors.Black;

            // Footer 2
            TextFrame tfo2 = section.AddTextFrame();
            tfo2.Width = "50mm";
            tfo2.Height = "20mm";
            tfo2.RelativeVertical = RelativeVertical.Page;
            tfo2.RelativeHorizontal = RelativeHorizontal.Page;
            tfo2.Top = "100mm"; // Misma altura
            tfo2.Left = "90mm"; // Posición horizontal diferente

            Paragraph paragraph7 = tfo2.AddParagraph(); // Se agrega al TextFrame correcto
            paragraph7.Format.Font.Size = new Unit(7);
            paragraph7.Format.Alignment = ParagraphAlignment.Center;
            paragraph7.AddFormattedText("C. Marcos Rivero Sánchez\n\n".ToUpper(), TextFormat.Bold).ToString();
            paragraph7.AddFormattedText("SECRETARIO DE AUTO SEGURO");
            paragraph7.Format.Font.Name = "Roboto";
            paragraph7.Format.Font.Bold = true;
            paragraph7.Format.SpaceAfter = "0.2cm";
            paragraph7.Format.Font.Color = Colors.Black;


            //SEGUNDA HOJA
            MigraDoc.DocumentObjectModel.Section section2 = document.AddSection();
            section2.PageSetup.Orientation = Orientation.Landscape;
            section2.PageSetup.PageHeight = "14cm";
            section2.PageSetup.PageWidth = "11cm";

            // Cabecera
            Image _myImage = section2.Headers.Primary.AddImage(Server.MapPath("~/Content/images/encabezado.png"));
            _myImage.RelativeVertical = RelativeVertical.Page;
            _myImage.RelativeHorizontal = RelativeHorizontal.Page;
            _myImage.Left = "0.15cm";
            _myImage.Top = "0.15cm";
            _myImage.Width = "13.7cm";
            _myImage.WrapFormat.Style = WrapStyle.Through;

            
            // Titulo
            TextFrame _tf = section2.AddTextFrame();
            _tf.Width = "10cm";
            _tf.Height = "5cm";
            _tf.RelativeVertical = RelativeVertical.Page;
            _tf.RelativeHorizontal = RelativeHorizontal.Page;
            _tf.Top = "2.5cm";
            _tf.Left = "1cm";
            Paragraph _paragraph = _tf.AddParagraph();
            _paragraph.Format.Font.Size = new Unit(10);
            _paragraph.AddText("FOLIO: e".ToUpper() + id);
            _paragraph.Format.Font.Name = "Roboto";
            _paragraph.Format.Font.Bold = true;
            _paragraph.Format.SpaceAfter = "0.2cm";
            _paragraph.Format.Font.Color = Colors.Black;
            

            // Parrafo 1
            TextFrame _tp = section2.AddTextFrame();
            _tp.Width = "120mm";
            _tp.Height = "100mm";
            _tp.RelativeVertical = RelativeVertical.Page;
            _tp.RelativeHorizontal = RelativeHorizontal.Page;
            _tp.Top = "3.2cm";
            _tp.Left = "1cm";
            Paragraph _paragraph3 = _tp.AddParagraph();
            _paragraph3.Format.Font.Size = new Unit(9);
            //_paragraph3.Format.Alignment = ParagraphAlignment.Justify;
            _paragraph3.AddFormattedText("Para dar cumplimiento a la ley de movilidad del estado de Quintana Roo, en sus artículo: 60 y 19, fracción II inciso B; donde dice que es obligatorio tener el tarjetón vigente a la vista del usuario, y se ratifica en el reglamento de la misma ley de movilidad en sus artículo: 2 fracción XXXV y en el 102 fracción III. \n".ToUpper());
            _paragraph3.AddFormattedText("\nEs obligatorio tener y portar en lugar visible del usuario este tarjetón de identidad. \n".ToUpper());
            _paragraph3.AddFormattedText("\nEs obligatorio presentar este tarjetón de identidad a las autoridades estatales, municipales y sindicales cuando sea requerido, para cualquier trámite dentro del sindicato es indispensable presentar este tarjetón de identidad vigente\n".ToUpper());

            //_paragraph3.AddFormattedText("Se autoriza 30 días al ");
            //_paragraph3.AddFormattedText("C. " + _operator.NOMBRE + " " + _operator.APELLIDOS, TextFormat.Bold);
            //_paragraph3.AddFormattedText(" para prestar el servicio público en el ");
            //_paragraph3.AddFormattedText("TAXI " + permissions.taxi, TextFormat.Bold | TextFormat.Underline);
            //_paragraph3.AddFormattedText(". Se compromete a respetar los reglamentos internos y a mantener buena conducta dentro y fuera del taxi respetando a todos sus compañeros y al cuerpo de delegados. \n\n");
            //paragraph3.AddFormattedText("\tNO DADO DE ALTA (REGISTRO NUEVO ESQUEMA)\t\n\n", TextFormat.Bold);
            // paragraph3.AddFormattedText("NO DADO DE ALTA", TextFormat.Bold | TextFormat.Underline);
            //paragraph3.AddFormattedText("\t\tCON DEUDA ", TextFormat.Bold);
            _paragraph3.Format.Font.Name = "Roboto";
            _paragraph3.Format.Font.Bold = true;
            _paragraph3.Format.SpaceAfter = "0.2cm";
            _paragraph3.Format.Font.Color = Colors.Black;

            // Footer
            TextFrame _tfo = section2.AddTextFrame();
            _tfo.Width = "50mm";
            _tfo.Height = "20mm";
            _tfo.RelativeVertical = RelativeVertical.Page;
            _tfo.RelativeHorizontal = RelativeHorizontal.Page;
            _tfo.Top = "99mm"; // Misma altura
            _tfo.Left = "10mm"; // Posición horizontal
            Paragraph _paragraph6 = _tfo.AddParagraph(); // Se agrega al TextFrame correcto
            _paragraph6.Format.Font.Size = new Unit(8);
            _paragraph6.Format.Alignment = ParagraphAlignment.Center;
            _paragraph6.AddFormattedText(_operator.firstName + " " + _operator.lastNameF + " " + _operator.lastNameM + "\n", TextFormat.Bold);
            _paragraph6.AddFormattedText("OPERADOR");
            _paragraph6.Format.Font.Name = "Roboto";
            _paragraph6.Format.Font.Bold = true;
            _paragraph6.Format.SpaceAfter = "0.2cm";
            _paragraph6.Format.Font.Color = Colors.Black;

            // Footer 2
            TextFrame _tfo2 = section2.AddTextFrame();
            _tfo2.Width = "50mm";
            _tfo2.Height = "20mm";
            _tfo2.RelativeVertical = RelativeVertical.Page;
            _tfo2.RelativeHorizontal = RelativeHorizontal.Page;
            _tfo2.Top = "99mm"; // Misma altura
            _tfo2.Left = "80mm"; // Posición horizontal diferente
            Paragraph _paragraph7 = _tfo2.AddParagraph(); // Se agrega al TextFrame correcto
            _paragraph7.Format.Font.Size = new Unit(8);
            _paragraph7.Format.Alignment = ParagraphAlignment.Center;
            _paragraph7.AddFormattedText(resp.Responsable + "\n", TextFormat.Bold);
            //_paragraph7.AddFormattedText(_partner.socemplacamiento + "\n", TextFormat.Bold);
            //_paragraph7.AddFormattedText(_partner.firstName + " " + _partner.lastNameF + " " + _partner.lastNameM, TextFormat.Bold);
            _paragraph7.AddFormattedText("RESPONSABLE");
            _paragraph7.Format.Font.Name = "Roboto";
            _paragraph7.Format.Font.Bold = true;
            _paragraph7.Format.SpaceAfter = "0.2cm";
            _paragraph7.Format.Font.Color = Colors.Black;


            /*
            // Folio
            TextFrame _fol = section2.AddTextFrame();
            _fol.Width = "10cm";
            _fol.Height = "5cm";
            _fol.RelativeVertical = RelativeVertical.Page;
            _fol.RelativeHorizontal = RelativeHorizontal.Page;
            _fol.Top = "5.8cm";
            _fol.Left = "0.37cm";
            Paragraph _paragraph9 = _fol.AddParagraph();
            _paragraph9.Format.Font.Size = new Unit(10);
            _paragraph9.AddText("FOLIO: e".ToUpper() + id);
            _paragraph9.Format.Font.Name = "Roboto";
            _paragraph9.Format.Font.Bold = true;
            _paragraph9.Format.SpaceAfter = "0.2cm";
            _paragraph9.Format.Font.Color = Colors.Black;
            */
            




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
                return File(stream, "application/pdf", "PE:pdf");
            }

        }

        // POST: Operator/GetCities
        [HttpPost]
        [SessionFilter("Admin")]
        public JsonResult generateexpresspermission(string gafet)
        {
            try
            {
                // ESTE PROGRAMA SE EJECUTA PARA ACUTALIZAR lOS PERMOS DE TODOS LOS OPERADORES Y SE GENERA DE MANERA CONRRECTA.
                C_permissions c_Permissions = new C_permissions();
                c_Permissions.generate(gafet);
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(true, settings);
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

        [Route("correctivo/print/{gafet}")]
        public ActionResult CorrectivoPrint(string gafet)
        {

            Operator _operator = new Operator();
            _operator = _operator.get(gafet);
            ViewBag.operat = _operator;

            Document document = new Document();
            MigraDoc.DocumentObjectModel.Section section = document.AddSection();
            section.PageSetup.Orientation = Orientation.Portrait;
            section.PageSetup.PageHeight = "279.4mm";
            section.PageSetup.PageWidth = "215.9mm";

            // Cabecera
            Image myImage = section.Headers.Primary.AddImage(Server.MapPath("~/Content/images/encabezado.png"));
            myImage.RelativeVertical = RelativeVertical.Page;
            myImage.RelativeHorizontal = RelativeHorizontal.Page;
            myImage.Left = "10mm";
            myImage.Top = "0.15cm";
            myImage.Width = "200mm";
            myImage.WrapFormat.Style = WrapStyle.Through;

            // Fecha
            TextFrame ff = section.AddTextFrame();
            ff.Width = "138mm";
            ff.Height = "50mm";
            ff.RelativeVertical = RelativeVertical.Page;
            ff.RelativeHorizontal = RelativeHorizontal.Page;
            ff.Top = "35mm";
            ff.Left = "120mm";
            Paragraph paragraph2 = ff.AddParagraph();
            paragraph2.Format.Font.Size = new Unit(10);
            paragraph2.AddFormattedText(("Cancun Q.Roo México a " + DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"))).ToUpper(), TextFormat.Bold);
            paragraph2.Format.Font.Name = "Arial";
            paragraph2.Format.Font.Bold = false;
            paragraph2.Format.SpaceAfter = "0.2cm";
            paragraph2.Format.Font.Color = Colors.Black;

            // Cabecera
            TextFrame tc = section.AddTextFrame();
            tc.Width = "150mm";
            tc.Height = "20mm";
            tc.RelativeVertical = RelativeVertical.Page;
            tc.RelativeHorizontal = RelativeHorizontal.Page;
            tc.Top = "50mm"; // Misma altura
            tc.Left = "35mm"; // Posición horizontal
            Paragraph paragraphc = tc.AddParagraph(); // Se agrega al TextFrame correcto
            paragraphc.Format.Font.Size = new Unit(16);
            paragraphc.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
            paragraphc.AddFormattedText("Correctivo disciplinario".ToUpper(), TextFormat.Bold);
            paragraphc.Format.Font.Name = "Arial";
            paragraphc.Format.Font.Bold = false;
            paragraphc.Format.SpaceAfter = "0.1cm";
            paragraphc.Format.Font.Color = Colors.Black;

            // OPERADOR
            TextFrame pn = section.AddTextFrame();
            pn.Width = "200mm";
            pn.Height = "50mm";
            pn.RelativeVertical = RelativeVertical.Page;
            pn.RelativeHorizontal = RelativeHorizontal.Page;
            pn.Top = "70mm";
            pn.Left = "10mm";
            Paragraph paragraph8 = pn.AddParagraph();
            paragraph8.Format.Font.Size = new Unit(12);
            paragraph8.AddFormattedText(("C. " + _operator.NOMBRE + " " + _operator.APELLIDOS + "\n").ToUpper(), TextFormat.Bold);
            paragraph8.AddFormattedText(("Gafete: " + _operator.CHOFER + "\tConduciendo el taxi #: " + "3876(MANUAL)\n").ToUpper(), TextFormat.Bold);
            paragraph8.AddFormattedText(("Presente.").ToUpper(), TextFormat.Bold);
            paragraph8.Format.Font.Name = "Arial";
            paragraph8.Format.Font.Bold = false;
            paragraph8.Format.SpaceAfter = "0.2cm";
            paragraph8.Format.Font.Color = Colors.Black;

            // CORRECTIVO
            TextFrame crr = section.AddTextFrame();
            crr.Width = "200mm";
            crr.Height = "50mm";
            crr.RelativeVertical = RelativeVertical.Page;
            crr.RelativeHorizontal = RelativeHorizontal.Page;
            crr.Top = "90mm";
            crr.Left = "10mm";
            Paragraph paragraph1 = crr.AddParagraph();
            paragraph1.Format.Font.Size = new Unit(12);
            paragraph1.AddFormattedText(("Por este medio se le comunica que se ha hecho acreedor a un(a): \n\n\n").ToUpper(), TextFormat.Bold);
            paragraph1.AddFormattedText((" \t\tLlamada de atención (DROPDOWN-SELECT)\n\n\n").ToUpper());
            paragraph1.AddFormattedText(("Motivo: \n\n\n").ToUpper(), TextFormat.Bold);
            paragraph1.AddFormattedText((" \t\t4.- No tener documentos vigentes y no rep\n\n\n").ToUpper());
            paragraph1.AddFormattedText(("Observaciones: \n\n").ToUpper(), TextFormat.Bold);
            paragraph1.AddFormattedText(("Llamada de atencion por estar bajo de %, si reincide sera sancionado ok march (MANUAL)\n\n\n").ToUpper());
            paragraph1.AddFormattedText(("Fecha inicial: " + " \t" +  DateTime.Now.ToString("dd/MM/yyyy") +  "\n").ToUpper(), TextFormat.Bold);
            paragraph1.AddFormattedText(("Fecha final: " + " \t" + DateTime.Now.AddDays(30).ToString("dd/MM/yyyy") + "\n").ToUpper(), TextFormat.Bold);
            paragraph1.AddFormattedText(("Días: " + " \t\t\t30 días").ToUpper(), TextFormat.Bold);
            paragraph1.Format.Font.Name = "Arial";
            paragraph1.Format.Font.Bold = false;
            paragraph1.Format.SpaceAfter = "0.2cm";
            paragraph1.Format.Font.Color = Colors.Black;

            // Footer
            TextFrame fo = section.AddTextFrame();
            fo.Width = "200mm";
            fo.Height = "50mm";
            fo.RelativeVertical = RelativeVertical.Page;
            fo.RelativeHorizontal = RelativeHorizontal.Page;
            fo.Top = "220mm";
            fo.Left = "10mm";
            Paragraph paragraph3 = fo.AddParagraph();
            paragraph3.Format.Font.Size = new Unit(12);
            paragraph3.AddFormattedText(("Atentamente\n\n\n\n").ToUpper(), TextFormat.Bold);
            paragraph3.AddFormattedText(("\nSecretaria de trabajo \n\n").ToUpper(), TextFormat.Bold);
            paragraph3.AddFormattedText(("10-569  \t\t51,661").ToUpper());
            paragraph3.Format.Font.Name = "Arial";
            paragraph3.Format.Font.Bold = false;
            paragraph3.Format.SpaceAfter = "0.2cm";
            paragraph3.Format.Font.Color = Colors.Black;

            // Enterado
            TextFrame fe = section.AddTextFrame();
            fe.Width = "200mm";
            fe.Height = "50mm";
            fe.RelativeVertical = RelativeVertical.Page;
            fe.RelativeHorizontal = RelativeHorizontal.Page;
            fe.Top = "220mm";
            fe.Left = "140mm";
            Paragraph paragraph4 = fe.AddParagraph();
            paragraph4.Format.Font.Size = new Unit(12);
            paragraph4.AddFormattedText(("Firma de enterado \n\n\n\n\n").ToUpper(), TextFormat.Bold);
            paragraph4.AddFormattedText((_operator.NOMBRE + " " + _operator.APELLIDOS +" \n\n").ToUpper(), TextFormat.Bold);
            paragraph4.AddFormattedText(("Gafete: " + _operator.CHOFER ).ToUpper());
            paragraph4.Format.Font.Name = "Arial";
            paragraph4.Format.Font.Bold = false;
            paragraph4.Format.SpaceAfter = "0.2cm";
            paragraph4.Format.Font.Color = Colors.Black;

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
                return File(stream, "application/pdf", "PE:pdf");
            }
        }

        [Route("correctivo/{gafet}")]
        public async Task<ActionResult> Correctivo(string gafet)
        {
            Operator _operator = new Operator();
            _operator = _operator.get(gafet);
            ViewBag.operat = _operator;

            var imageTask = Task.Run(() => getImage(_operator)); // Obtenemos la tarea para obtener la imagen.

            var financesTask = Task.Run(() => getFinances(_operator)); // Obtenemos la tarea para obtener la imagen.
           

            await Task.WhenAll(imageTask, financesTask);
            _operator.urlImage = imageTask.Result; // Asignamos la imagen.
            ViewBag.Porcentaje = financesTask.Result.porcent;
            ViewBag.faltas = financesTask.Result.faltas;

            return View(_operator);
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
            Operator _Operator = new Operator();
            _Operator = _Operator.get(gafet);// Obtenemos al socio.
            @ViewBag.User = _Operator;
            userlog userlog = _Operator.userlog();
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
            Operator _Operator = new Operator();
            _Operator = _Operator.get(gafet); // Obtenemos al socio.
            ViewBag.exist = false;
            ViewBag.User = _Operator;
            ViewBag.id = _Operator.userId;

            RegisterViewModel registerData = new RegisterViewModel()
            {
                Lastname = _Operator.lastNameF + " " + _Operator.lastNameM,
                Name = User.firstName,
                Email = collection["Email"],
                Password = collection["Password"],
                CountryCode = "+52",
                PhoneNumber = collection["Phone"],
                IdPadron = _Operator.userId
            };

            // Definimos el tipo de Role
            if (_Operator.partnerTypeId == 2)
            {
                registerData.roleID = 18;
            }
            else if (_Operator.partnerTypeId == 1)
            {
                registerData.roleID = 20;
            }
            else
            {
                registerData.roleID = 17;
            }
            registerData = _Operator.generatePassword(registerData);
            if (registerData.success)
            {
                // Redireccionar a detalles del socio.
                return Redirect("~/Operator/tarjeton/" + _Operator.partnerReference);
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
            Operator _Operator = new Operator();
            _Operator = _Operator.get(gafet);// Obtenemos al socio.
            @ViewBag.User = _Operator;
            userlog userlog = _Operator.userlog();
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
            Operator _Operator = new Operator();
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
            _Operator = _Operator.get(gafet); // Obtenemos al socio.

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
                return Redirect("~/Operator/tarjeton/" + _Operator.partnerReference);
            }
            return View();
        }


        [SessionFilter("Admin")]
        [HttpGet]
        [Route("faltas/sindicales/{gafet}")]
        public ActionResult cancelarcuotas(string gafet)
        {
            try
            {
                Operator _Partner = new Operator();
                // Obtenemos todos los tarjetons del operador.
                _Partner = _Partner.get(gafet);
                if (_Partner != null && _Partner.bucket != null && _Partner.key != null)
                {
                    _Partner.urlImage = AmazonHelper.getImageUser(_Partner.bucket, _Partner.key);
                }
                List<Inventario> cuotas = _Partner.getinventario(0); // la familia 12 es de cuotas sindicatles
                decimal amount = 0;
                foreach (var cuota in cuotas)
                {
                    amount += cuota.PrecioConIva * cuota.Exist;
                }
                ViewBag.dues = cuotas;
                ViewBag.amount = amount;
                ViewBag.total = ViewBag.total = cuotas.Sum(x => x.Exist);

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
        [Route("faltas/sindicales/{gafet}")]
        public ActionResult cancelarcuotas(FormCollection data, string gafet)
        {
            try
            {
                string EditBy = this.User.firstName + " " + this.User.lastName;
                int cant = Convert.ToInt32(data["cant"]);
                string autoriza = Convert.ToString(data["autoriza"]);
                string movimiento = Convert.ToString(data["movimiento"]);
                Operator _Partner = new Operator();
                // Obtenemos todos los tarjetons del operador.
                _Partner = _Partner.get(gafet);
                List<Inventario> cuotas = _Partner.getinventario(0); // la familia 12 es de cuotas sindicatles
                _Partner.canclearfaltas(cant, movimiento, autoriza, EditBy);
                // Obtenemos todos los tarjetons del operador.
                _Partner = _Partner.get(gafet);
                if (_Partner != null && _Partner.bucket != null && _Partner.key != null)
                {
                    _Partner.urlImage = AmazonHelper.getImageUser(_Partner.bucket, _Partner.key);
                }
                decimal amount = 0;
                foreach (var cuota in cuotas)
                {
                    amount += cuota.PrecioConIva * cuota.Exist;
                }
                ViewBag.dues = cuotas;
                ViewBag.amount = amount;
                ViewBag.total = cuotas.Sum(x => x.Exist);
                return Redirect("~/Operator/faltas/sindicales/" + _Partner.partnerReference);
            }
            catch (Exception ex)
            {
                // ENVIAR A LA VISTA DE ERROR (GERMAN)
                throw;
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
    }
}