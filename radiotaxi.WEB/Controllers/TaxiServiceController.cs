using radiotaxi.Client.Interface;
using radiotaxi.Client.Request;
using radiotaxi.Model;
using radiotaxi.WEB.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.Services.Description;
using radiotaxi.Model.DTO;

// Controlador de servicios.
namespace radiotaxi.WEB.Controllers
{
    [SessionFilter(Roles: "Admin")]
    public class TaxiServiceController : webBaseController
    {
        static readonly iStatusService statusServiceRequest = new rStatusService();
        static readonly iBaseService baseServiceRequest = new rBaseService();
        static readonly iCatalogService catalogServiceRequest = new rCatalogService();
        static readonly iTaxiService taxiServiceRequest = new rTaxiService();
        static readonly iClient clientRequest = new rClient();
        static readonly iUnit_Taxi Unit_TaxiRequest = new rUnit_Taxi();

        public ActionResult ExportData()
        {
            int statusConnection = 0;
            IEnumerable<taxiService> services = taxiServiceRequest.GetAllPending(Session["access_token"].ToString(), ref statusConnection);
            GridView gv = new GridView();
            gv.DataSource = services;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ServiciosPendientes.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Search");
        }

        // GET: TaxiService/Create
        public ActionResult Create(string phone, string observation, string address)
        {
            try
            {
                string addressFromClient = "";
                string nameClient = "";

                ViewBag.phone = phone;
                ViewBag.observation = observation;
                ViewBag.address = address;

                // Bases de radio taxi.
                baseService Base = new baseService();
                List<baseService> BaseServices = Base.get();
                ViewBag.baseRT = new SelectList(BaseServices, "id", "name");

                // Status del servicio 
                statusService statusService = new statusService();
                List<statusService> statusList = statusService.get();
                ViewBag.status = new SelectList(statusList, "id", "name");

                // Catalgo que define el tipo de serivico.
                catalogService catalogService = new catalogService();
                List<catalogService> catalogList = catalogService.get();
                ViewBag.catalog = new SelectList(catalogList, "id", "name");

                // Obtenemos el nombre del cliente
                client Client = new client(phone);
                ViewBag.cliente = Client.name + " " + Client.lastname1 + " " + Client.lastname2;
                nameClient = Client.name + " " + Client.lastname1;
                addressFromClient = Client.address;
                return View(new taxiServiceDTO { startPoint = addressFromClient, statusServiceId = 2, note = nameClient });
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // POST: TaxiService/Create
        [HttpPost]
        public ActionResult Create(taxiService collection, string datesServices)
        {
            try
            {
                List<DateTime> DatesServices = this.ParseDates(datesServices);
                taxiService taxiService = null;
                List<taxiService> taxiServices = new List<taxiService>();
                ViewBag.message = "";
                if (DatesServices.Count() > 0) // Varios servicios.
                {
                    foreach (var date in DatesServices)
                    {
                        try
                        {
                            collection.datePickUp = date;
                            collection.dateRequested = DateTime.Now;
                            collection.createdBy = this.UserName; // Nombre del usuario actual.
                            collection.save();
                            if (collection.StatusAction == success)
                            {
                                Console.WriteLine("Sucess");
                            }
                            else
                            {
                                Console.WriteLine("ERROR");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("IndexPendientes");
                }
                ViewBag.message = "El campo fecha es requerido.";
                return View(collection);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // METODOS NUEVOS PARA OBTENER INFORMACION.
        // GET: Client
        public ActionResult Index(int? page)
        {
            if (this.ClientSearchParameters == null) this.ClientSearchParameters = new ClientSearchParametersDTO();
            ViewBag.parameters = this.ClientSearchParameters;
            taxiService TaxiService = new taxiService();
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            TaxiServicesPagination taxiServicesPagination = TaxiService.getTotal(pageNumber, pageSize);
            ViewBag.totalServices = taxiServicesPagination.TotalItems;
            Dictionary<string, int> resume = new Dictionary<string, int>();
            ViewBag.resume = TaxiService.getCounterResume();
            return View(taxiServicesPagination);
        }

        public ActionResult Pending(int? page)
        {
            if (this.ClientSearchParameters == null) this.ClientSearchParameters = new ClientSearchParametersDTO();
            ViewBag.parameters = this.ClientSearchParameters;
            taxiService TaxiService = new taxiService();
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            TaxiServicesPagination taxiServicesPagination = TaxiService.getPending(pageNumber, pageSize);
            ViewBag.totalServices = taxiServicesPagination.TotalItems;
            Dictionary<string, int> resume = new Dictionary<string, int>();
            ViewBag.resume = TaxiService.getCounterResume();
            return View(taxiServicesPagination);
        }

        public ActionResult Canceled(int? page)
        {
            if (this.ClientSearchParameters == null) this.ClientSearchParameters = new ClientSearchParametersDTO();
            ViewBag.parameters = this.ClientSearchParameters;
            taxiService TaxiService = new taxiService();
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            TaxiServicesPagination taxiServicesPagination = TaxiService.getCanceled(pageNumber, pageSize);
            ViewBag.totalServices = taxiServicesPagination.TotalItems;
            Dictionary<string, int> resume = new Dictionary<string, int>();
            ViewBag.resume = TaxiService.getCounterResume();
            return View(taxiServicesPagination);
        }

        public ActionResult Assigned(int? page)
        {
            if (this.ClientSearchParameters == null) this.ClientSearchParameters = new ClientSearchParametersDTO();
            ViewBag.parameters = this.ClientSearchParameters;
            taxiService TaxiService = new taxiService();
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            TaxiServicesPagination taxiServicesPagination = TaxiService.getAsigment(pageNumber, pageSize);
            ViewBag.totalServices = taxiServicesPagination.TotalItems;
            Dictionary<string, int> resume = new Dictionary<string, int>();
            ViewBag.resume = TaxiService.getCounterResume();
            return View(taxiServicesPagination);
        }
        public ActionResult History(int? page)
        {
            if (this.ClientSearchParameters == null) this.ClientSearchParameters = new ClientSearchParametersDTO();
            ViewBag.parameters = this.ClientSearchParameters;
            taxiService TaxiService = new taxiService();
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            TaxiServicesPagination taxiServicesPagination = TaxiService.getAsigment(pageNumber, pageSize);
            ViewBag.totalServices = taxiServicesPagination.TotalItems;
            Dictionary<string, int> resume = new Dictionary<string, int>();
            ViewBag.resume = TaxiService.getCounterResume();
            return View(taxiServicesPagination);
        }


        // GET: TaxiService
        public ActionResult IndexAsignados(int? page)
        {
            IEnumerable<taxiService> services = new List<taxiService>();
            return View(services);            
        }

        // GET: TaxiService
        public ActionResult IndexPendientes(int? page)
        {
            try
            {
                IEnumerable<taxiService> services = new List<taxiService>();
                return View(services);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View();
            }
        }

        // GET: TaxiService
        public ActionResult IndexCancelados(int? page)
        {
            int statusConnection = 0;
            IEnumerable<taxiService> services = taxiServiceRequest.GetAllCancelled(Session["access_token"].ToString(), ref statusConnection);
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            if (services != null)
            {
                services = services.OrderBy(x => x.datePickUp);
                return View(services.ToPagedList(pageNumber, pageSize));
            }
            return View(services); 
        }

        // GET: TaxiService
        public ActionResult IndexEspeciales(int? page)
        {
            IEnumerable<taxiService> services = new List<taxiService>();
            return View(services);
        }

        // GET: TaxiService
        public ActionResult IndexPendingByClient(string id, int? page)
        {
            IEnumerable<taxiService> services = new List<taxiService>();
            return View("ServicesByClient", services);
        }

        // GET: TaxiService
        public ActionResult IndexAssignedByClient(string id, int? page)
        {
            IEnumerable<taxiService> services = new List<taxiService>();
            return View("ServicesAssignedByClient", services);
        }

        // GET: TaxiService/Details/5
        public ActionResult Details(int id)
        {
            int statusConnection = 0;
            taxiService service = taxiServiceRequest.GetById(id, Session["access_token"].ToString(), ref statusConnection);
            return View(service);
        }

        // GET: TaxiService/Edit/5
        public ActionResult Edit(int id)
        {
            int statusConnectionStatus = 0;
            ViewBag.status = new SelectList(statusServiceRequest.GetAll(Session["access_token"].ToString(), ref statusConnectionStatus), "id", "name");
            int statusConnectionBase = 0;
            ViewBag.baseRT = new SelectList(baseServiceRequest.GetAll(Session["access_token"].ToString(), ref statusConnectionBase), "id", "name");
            int statusConnectionCatalog = 0;
            ViewBag.catalog = new SelectList(catalogServiceRequest.GetAll(Session["access_token"].ToString(), ref statusConnectionCatalog), "id", "name");
            int statusConnection = 0;
            taxiServiceDTO service = taxiServiceRequest.GetByIdDTO(id, Session["access_token"].ToString(), ref statusConnection);
            int statusConnectionClient = 0;
            ViewBag.cliente = clientRequest.GetById(service.phoneId, Session["access_token"].ToString(), ref statusConnectionClient).name;
            return View(service);
        }

        // POST: TaxiService/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, taxiService collection)
        {
            try
            {
                // TODO: Add update logic here
                collection.editedBy = Session["name"].ToString();
                int statusConnection = 0;
                taxiServiceRequest.Update(id, collection, Session["access_token"].ToString(), ref statusConnection);
                if (statusConnection == 204)
                {
                    return RedirectToAction("IndexPendientes");
                }
                else
                {
                    int statusConnectionStatus = 0;
                    ViewBag.status = new SelectList(statusServiceRequest.GetAll(Session["access_token"].ToString(), ref statusConnectionStatus), "id", "name");
                    int statusConnectionBase = 0;
                    ViewBag.baseRT = new SelectList(baseServiceRequest.GetAll(Session["access_token"].ToString(), ref statusConnectionBase), "id", "name");
                    int statusConnectionCatalog = 0;
                    ViewBag.catalog = new SelectList(catalogServiceRequest.GetAll(Session["access_token"].ToString(), ref statusConnectionCatalog), "id", "name");
                    return View(collection);
                }
            }
            catch
            {
                int statusConnectionStatus = 0;
                ViewBag.status = new SelectList(statusServiceRequest.GetAll(Session["access_token"].ToString(), ref statusConnectionStatus), "id", "name");
                int statusConnectionBase = 0;
                ViewBag.baseRT = new SelectList(baseServiceRequest.GetAll(Session["access_token"].ToString(), ref statusConnectionBase), "id", "name");
                int statusConnectionCatalog = 0;
                ViewBag.catalog = new SelectList(catalogServiceRequest.GetAll(Session["access_token"].ToString(), ref statusConnectionCatalog), "id", "name");
                return View(collection);
            }
        }

        // GET: TaxiService/Delete/5
        public ActionResult Delete(int id)
        {
            int statusConnection = 0;
            taxiService service = taxiServiceRequest.GetById(id, Session["access_token"].ToString(), ref statusConnection);
            return View(service);
        }

        // POST: TaxiService/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, taxiService collection)
        {
            return RedirectToAction("IndexPendientes");
        }

        //Partial view for last 10 services
        [HttpGet]
        public ActionResult GetLastServices(string phoneNumber)
        {
            try
            {
                int statusConnection = 0;
                IEnumerable<taxiService> services;
                services = taxiServiceRequest.Get10ByClient(phoneNumber, Session["access_token"].ToString(), ref statusConnection);
                return PartialView("lastServices", services);
            }
            catch (Exception)
            {
                return PartialView("lastServices", null);
            }
            
        }

        public JsonResult fastUpdate(string objectService, string rtNumberServices, string baseServiceId, string statusServiceId)
        {
            string mensaje = "";
            try
            {
                taxiService taxiService = new taxiService();
                int j;
                int statusConnection = 0;
                Int32.TryParse(objectService, out j);
                // Obtenemos el servicio de taxi.
                taxiService = taxiServiceRequest.GetById(j, Session["access_token"].ToString(), ref statusConnection);
                Int32.TryParse(baseServiceId, out j);
                taxiService.baseServiceId = j;
                Int32.TryParse(statusServiceId, out j);
                taxiService.statusServiceId = j;

                List<rtNumberService> rtNumber = new List<rtNumberService>();

                //Borrar los rtNumbers de la lista

                rtNumberService rtNumberService = new rtNumberService();
                Int32.TryParse(rtNumberServices, out j);
                rtNumberService.rtNumber = j;
                rtNumberService.serviceId = taxiService.id;
                rtNumber.Add(rtNumberService);
                taxiService.rtNumberServices = rtNumber;

                //Clean Object for update
                taxiService.baseService = null;
                taxiService.catalogService = null;
                taxiService.client = null;
                taxiService.statusService = null;

                // set ho edit de taxiService.
                taxiService.editedBy = Session["name"].ToString();
                taxiServiceRequest.Update(taxiService.id, taxiService, Session["access_token"].ToString(), ref statusConnection);
                mensaje = "OK";
                return Json(mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                mensaje = "Error";
                return Json(mensaje, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
