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
    public class EmplaRestriccionsController : webBaseController
    {
        private radiotaxiEntities db = new radiotaxiEntities();

        // GET: EmplaRestriccions
        public ActionResult Index()
        {
            return View(db.EmplaRestriccions.ToList());
        }

        // GET: EmplaRestriccions/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmplaRestriccion emplaRestriccion = db.EmplaRestriccions.Find(id);
            if (emplaRestriccion == null)
            {
                return HttpNotFound();
            }
            return View(emplaRestriccion);
        }

        // GET: EmplaRestriccions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmplaRestriccions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Servicio,Id_Marca,Id_Modelo,AutoAno,Motor,No_Serie,Motivo,Avisar_a,Telefono,FechaOp")] EmplaRestriccion emplaRestriccion)
        {
            if (ModelState.IsValid)
            {
                db.EmplaRestriccions.Add(emplaRestriccion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emplaRestriccion);
        }

        // GET: EmplaRestriccions/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmplaRestriccion emplaRestriccion = db.EmplaRestriccions.Find(id);
            if (emplaRestriccion == null)
            {
                return HttpNotFound();
            }
            return View(emplaRestriccion);
        }

        // POST: EmplaRestriccions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Servicio,Id_Marca,Id_Modelo,AutoAno,Motor,No_Serie,Motivo,Avisar_a,Telefono,FechaOp")] EmplaRestriccion emplaRestriccion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emplaRestriccion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emplaRestriccion);
        }

        // GET: EmplaRestriccions/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmplaRestriccion emplaRestriccion = db.EmplaRestriccions.Find(id);
            if (emplaRestriccion == null)
            {
                return HttpNotFound();
            }
            return View(emplaRestriccion);
        }

        // POST: EmplaRestriccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            EmplaRestriccion emplaRestriccion = db.EmplaRestriccions.Find(id);
            db.EmplaRestriccions.Remove(emplaRestriccion);
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
