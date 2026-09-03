using radiotaxi.Model;
using radiotaxi.WEB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace radiotaxi.WEB.Controllers
{
    [SessionFilter("Admin")]
    [RoutePrefix("users")]
    public class UsersController : webBaseController
    {
        // GET: Client
        public ActionResult Index(int? page)
        {
            try
            {
                if (this.UserLogSearchParameters == null) this.UserLogSearchParameters = new UserLogSearchParametersDTO();
                ViewBag.parameters = this.UserLogSearchParameters; // Objeto que contiene los parametros de busqueda, estos se guardan en session pero estransparante para la capa del controlador.
                userlog User = new userlog();
                int pageSize = 100;
                int pageNumber = (page ?? 1);
                UserLogPagination UserLogPagination = User.get(pageNumber, pageSize, this.UserLogSearchParameters.gafet, this.UserLogSearchParameters.name, this.UserLogSearchParameters.lastName1);
                ViewBag.totalClients = UserLogPagination.TotalItems;
                return View(UserLogPagination);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        [HttpPost]
        public ActionResult Index(int? page, string taxi, string gafet, string name, string lastName1)
        {
            try
            {
                this.UserLogSearchParameters = new UserLogSearchParametersDTO();
                this.UserLogSearchParameters.taxi = taxi; // Este es el gafet de operador
                this.UserLogSearchParameters.gafet = gafet; // Este es el gafet de operador
                this.UserLogSearchParameters.name = name;
                this.UserLogSearchParameters.lastName1 = lastName1;
                ViewBag.parameters = this.UserLogSearchParameters;
                userlog User = new userlog();
                int pageSize = 100;
                int pageNumber = (page ?? 1);
                UserLogPagination userLogPagination = User.get(pageNumber, pageSize, gafet, name, lastName1);
                ViewBag.totalClients = userLogPagination.TotalItems;
                return View(userLogPagination);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.roles = new role().get().OrderBy(x => x.name);
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(RegisterViewModel Model)
        {
            try
            {
                // TODO: Add insert logic here
                userlog User = new userlog();
                User.firstName = Model.Name;
                User.lastName = Model.LastnameF +  " " + Model.LastnameM;
                User.createdDt = DateTime.Now;
                User.partnerReference = "";
                User.password = Model.Password;
                User.email = Model.Email;
                User.isActive = true;
                User.phone = Model.PhoneNumber;
                User.RoleID = Model.roleID;
                User.IP = Model.IP;
                User.save();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            userlog User = new userlog();
            User = User.get(id);
            ViewBag.roles = new role().get().OrderBy(x => x.name);
            return View(User);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, RegisterViewModel Model)
        {
            try
            {
                int statusConnection = 0;
                int role = 0;
                userlog User = new userlog();
                User = User.get(id);
                ViewBag.roles = new role().get().OrderBy(x => x.name);
                SMTPconnection clientSMTP = new SMTPconnection();
                // Si no se cumplen las primeras dos pues sigue el proceso normalito...
                User.firstName = Model.Name;
                User.lastName = Model.LastnameF + " " + Model.LastnameM;
                User.createdDt = DateTime.Now;
                User.partnerReference = "";
                User.password = Model.Password;
                User.email = Model.Email;
                User.isActive = true;
                User.phone = Model.PhoneNumber;
                User.RoleID = Model.roleID;
                User.removePassword = Model.removePassword;
                User.IP = Model.IP;
                User.isActive = Model.isActive;
                User.update();
                ViewBag.Name = User.firstName + " " + User.lastName;
                ViewBag.email = User.email;
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Users/Delete/5
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
    }
}
