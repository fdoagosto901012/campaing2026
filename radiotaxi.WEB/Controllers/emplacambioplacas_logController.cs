using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using radiotaxi.Model;

namespace radiotaxi.WEB.Controllers
{
    public class emplacambioplacas_logController : Controller
    {
        private radiotaxiEntities db = new radiotaxiEntities();

        // GET: emplacambioplacas_log
        public ActionResult Index()
        {
            return View(db.emplacambioplacas_log.ToList());
        }

        // GET: emplacambioplacas_log/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            emplacambioplacas_log emplacambioplacas_log = db.emplacambioplacas_log.Find(id);
            if (emplacambioplacas_log == null)
            {
                return HttpNotFound();
            }
            return View(emplacambioplacas_log);
        }

        // GET: emplacambioplacas_log/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: emplacambioplacas_log/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idcambio,economico,id_opemplacamiento,placaanterior,placanueva,fecha,id_usuario,usuario")] emplacambioplacas_log emplacambioplacas_log)
        {
            if (ModelState.IsValid)
            {
                db.emplacambioplacas_log.Add(emplacambioplacas_log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emplacambioplacas_log);
        }

        // GET: emplacambioplacas_log/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            emplacambioplacas_log emplacambioplacas_log = db.emplacambioplacas_log.Find(id);
            if (emplacambioplacas_log == null)
            {
                return HttpNotFound();
            }
            return View(emplacambioplacas_log);
        }

        // POST: emplacambioplacas_log/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idcambio,economico,id_opemplacamiento,placaanterior,placanueva,fecha,id_usuario,usuario")] emplacambioplacas_log emplacambioplacas_log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emplacambioplacas_log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emplacambioplacas_log);
        }

        // GET: emplacambioplacas_log/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            emplacambioplacas_log emplacambioplacas_log = db.emplacambioplacas_log.Find(id);
            if (emplacambioplacas_log == null)
            {
                return HttpNotFound();
            }
            return View(emplacambioplacas_log);
        }

        // POST: emplacambioplacas_log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            emplacambioplacas_log emplacambioplacas_log = db.emplacambioplacas_log.Find(id);
            db.emplacambioplacas_log.Remove(emplacambioplacas_log);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
