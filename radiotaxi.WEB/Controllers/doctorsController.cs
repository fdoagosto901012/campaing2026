using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace radiotaxi.WEB.Controllers
{
    public class doctorsController : webBaseController
    {
        // GET: doctors
        public ActionResult Index()
        {
            return View();
        }

        // GET: doctors/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: doctors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: doctors/Create
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

        // GET: doctors/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: doctors/Edit/5
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

        // GET: doctors/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: doctors/Delete/5
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
