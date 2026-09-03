using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace radiotaxi.WEB.Controllers
{
    public class ErrorController : webBaseController
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        // GET: Error/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Error/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Error/Create
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

        // GET: Error/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Error/Edit/5
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

        // GET: Error/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Error/Delete/5
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

        public ActionResult LoginErrors()
        {
            return View();
        }


        public ActionResult LoginIPERROR()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult EcoSoc()
        {
            return View();
        }


        public ActionResult EmplaBloqueo()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ErrorCredencial(string mensaje)
        {
            ViewBag.error = mensaje; 
            return View();
        }
    }
}
