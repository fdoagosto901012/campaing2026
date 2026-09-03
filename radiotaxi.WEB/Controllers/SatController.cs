using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using radiotaxi.Model;
using radiotaxi.WEB.Helper;

namespace radiotaxi.WEB.Controllers
{
    [RoutePrefix("SAT")]
    public class SatController : Controller
    {
        // GET: Sat
        public ActionResult Index()
        {
            return View();
        }

        // GET: Sat/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Sat/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sat/Create
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

        // GET: Sat/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sat/Edit/5
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

        // GET: Sat/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sat/Delete/5
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


        // GET: Partner/Edit/5
        [SessionFilter("Admin")]
        [Route("Edit/{gafet}")]
        public ActionResult Edit(string gafet)
        {
            Partner _Partner = new Partner();
            _Partner = _Partner.get(gafet);
            
            if (_Partner != null && _Partner.bucket != null && _Partner.key != null)
            {
                _Partner.urlImage = AmazonHelper.getImageUser(_Partner.bucket, _Partner.key);
            }
            return View(_Partner);
        }
    }
}
