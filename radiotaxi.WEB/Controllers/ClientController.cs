using radiotaxi.Client.Interface;
using radiotaxi.Client.Request;
using radiotaxi.Model;
using radiotaxi.WEB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Collections.ObjectModel;

namespace radiotaxi.WEB.Controllers
{
    [SessionFilter(Roles:"Admin")]
    public class ClientController : webBaseController
    {
        static readonly iClient clientRequest = new rClient();
        // GET: Client
        public ActionResult Index(int? page)
        {
            if (this.ClientSearchParameters == null) this.ClientSearchParameters = new ClientSearchParametersDTO();
            ViewBag.parameters = this.ClientSearchParameters;
            client Client = new client();
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            ClientPagination clientPagination = Client.get_clients(pageNumber, pageSize, this.ClientSearchParameters.phoneNumber, this.ClientSearchParameters.name, this.ClientSearchParameters.lastName1, this.ClientSearchParameters.lastName2);
            ViewBag.totalClients = clientPagination.TotalItems;
            return View(clientPagination);
        }

        [HttpPost]
        public ActionResult Index(int? page, string phoneNumber, string name, string lastName1, string lastName2)
        {
            this.ClientSearchParameters = new ClientSearchParametersDTO();
            this.ClientSearchParameters.phoneNumber = phoneNumber;
            this.ClientSearchParameters.name = name;
            this.ClientSearchParameters.lastName1 = lastName1;
            this.ClientSearchParameters.lastName2 = lastName2;
            ViewBag.parameters = this.ClientSearchParameters;
            client Client = new client();
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            ClientPagination clientPagination = Client.get_clients(pageNumber, pageSize,phoneNumber, name, lastName1, lastName2);
            ViewBag.totalClients = clientPagination.TotalItems;
            return View(clientPagination);
        }

        // GET: Client/Create
        public ActionResult Create()
        {
            ViewBag.exist = "";
            ViewBag.message = "";
            return View();
        }

        // POST: Client/Create
        [HttpPost]
        public ActionResult Create(client collection)
        {
            try
            {
                
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View(collection);
            }
        }

        // GET: Client/Edit/5
        public ActionResult Edit(string id)
        {
            int statusConnection = 0;
            client Client = new client(id);
            ViewBag.message = "";
            return View(Client);
        }

        // POST: Client/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, client collection)
        {
            try
            {
                
                return View();
            }
            catch(Exception ex)
            {
                ViewBag.message = ex.Message;
                return View(collection);
            }
        }

        // GET: Client/Details/5
        public ActionResult Details(string id)
        {
            int statusConnection = 0;
            client client = new client(id);            
            return View(client);
        }

        // GET: Client/Delete/5
        public ActionResult Delete(string id)
        {
            int statusConnection = 0;
            client client = clientRequest.GetById(id, Session["access_token"].ToString(), ref statusConnection);
            return View(client);
        }

        // POST: Client/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, client collection)
        {
            try
            {
                // TODO: Add delete logic here
                int statusConnection = 0;
                clientRequest.Delete(id, Session["access_token"].ToString(), ref statusConnection);
                //Check the http response for delete
                if (statusConnection == 200)
                {
                    return RedirectToAction("Index", "Search");
                }
                else
                {
                    return View(collection);
                }  
            }
            catch
            {
                return View(collection);
            }
        }
    }
}
