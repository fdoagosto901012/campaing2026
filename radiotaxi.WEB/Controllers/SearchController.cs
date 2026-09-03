using radiotaxi.Model;
using radiotaxi.WEB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace radiotaxi.WEB.Controllers
{
    [SessionFilter]
    public class SearchController : Controller
    {
        //static readonly iClient clientRequest = new rClient();
        //static readonly iTaxiService taxiServiceRequest = new rTaxiService();
        // GET: Search
        public ActionResult Index()//string phoneNumber, string name, string lastName1, string lastName2
        {
            IEnumerable<client> clients = null;
            return View(clients);
        }

        [HttpGet]
        public ActionResult History(string phoneNumber, int? rtNumber, string datePickUp, string datePickUp2, int? page, int? size)//string phoneNumber, string name, string lastName1, string lastName2
        {
            try
            {
                int statusConnection = 0;
                Paginator<taxiService> taxiServices;
                SearchModelHistory search = new SearchModelHistory();
                search.phoneNumber = phoneNumber;
                search.rtNumber = rtNumber;
                search.datePickUp = datePickUp;
                search.datePickUp2 = datePickUp2;

                int pageSize = size ?? 20;
                int pageNumber = (page ?? 1);
                ViewBag.phoneNumberValue = phoneNumber ?? "";
                ViewBag.rtNumberValue = rtNumber;
                ViewBag.datePickUpValue = datePickUp;
                ViewBag.datePickUpValue2 = datePickUp2;
                ViewBag.sizeValue = size;

                if (!String.IsNullOrEmpty(phoneNumber) || rtNumber != null || (!String.IsNullOrEmpty(datePickUp) && !String.IsNullOrEmpty(datePickUp2)))
                {
                    taxiServices = null;// taxiServiceRequest.searchHistory(Session["access_token"].ToString(), search, pageNumber, pageSize, ref statusConnection);
                }
                else
                {
                    taxiServices = null; // taxiServiceRequest.searchHistory(Session["access_token"].ToString(), search, pageNumber, pageSize, ref statusConnection);
                }

                if (taxiServices != null)
                {
                    IPagedList<taxiService> pageOrders = new StaticPagedList<taxiService>(taxiServices.Data, pageNumber, pageSize, taxiServices.itemCount);
                    return View(pageOrders);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            
        }



        // METODO OBSOLETO
        [HttpGet]
        public ActionResult GetClients(string phoneNumber, string name, string lastName1, string lastName2)
        {
            if (!String.IsNullOrEmpty(phoneNumber) || !String.IsNullOrEmpty(name) || !String.IsNullOrEmpty(lastName1) || !String.IsNullOrEmpty(lastName2))
            {
                int statusConnection = 0;
                SearchModel search = new SearchModel();
                IEnumerable<client> clients;
                search.phoneNumber = phoneNumber;
                search.name = name;
                search.lastName1 = lastName1;
                search.lastName2 = lastName2;
                clients = null; // clientRequest.search(Session["access_token"].ToString(), search, 1, ref statusConnection);
                return PartialView("SearchResult", clients);
            }
            else
            {
                return null;
            }
        }





        [HttpGet]
        public ActionResult Clients()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Clients(string phoneNumber, string name, string lastName1, string lastName2)
        {
            if (!String.IsNullOrEmpty(phoneNumber) || !String.IsNullOrEmpty(name) || !String.IsNullOrEmpty(lastName1) || !String.IsNullOrEmpty(lastName2))
            {
                int statusConnection = 0;
                SearchModel search = new SearchModel();
                IEnumerable<client> clients;
                search.phoneNumber = phoneNumber;
                search.name = name;
                search.lastName1 = lastName1;
                search.lastName2 = lastName2;
                clients = null; // clientRequest.search(Session["access_token"].ToString(), search, 1, ref statusConnection);
                return PartialView("SearchResult", clients);
            }
            else
            {
                return null;
            }
        }




        [HttpGet]
        public ActionResult GetServices(string phoneNumber, int? rtNumber, DateTime? datePickUp)
        {
            if (!String.IsNullOrEmpty(phoneNumber) || rtNumber != null || datePickUp != null)
            {
                int statusConnection = 0;
                SearchModelService search = new SearchModelService();
                IEnumerable<taxiService> taxiServices;
                search.phoneNumber = phoneNumber;
                search.rtNumber = rtNumber;
                search.datePickUp = datePickUp;
                taxiServices = null;// taxiServiceRequest.search(Session["access_token"].ToString(), search, ref statusConnection);
                if (taxiServices != null)
                {
                    taxiServices = null;
                }
                return PartialView("SearchServicesResult", taxiServices);
            }
            else
            {
                return null;
            }

        }

    }
}