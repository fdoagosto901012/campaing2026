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
    public class EmplaModeloesController : Controller
    {
        private radiotaxiEntities db = new radiotaxiEntities();

        // GET: EmplaModeloes
        public ActionResult Index()
        {
            return View(db.EmplaModeloes.ToList());
        }

        // GET: EmplaModeloes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmplaModelo emplaModelo = db.EmplaModeloes.Find(id);
            if (emplaModelo == null)
            {
                return HttpNotFound();
            }
            return View(emplaModelo);
        }

        // GET: EmplaModeloes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmplaModeloes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Modelo,Id_Marca,Modelo,FechaOp,FechaEdit,Id_Usuario,Id_UsuarioEdit")] EmplaModelo emplaModelo)
        {
            if (ModelState.IsValid)
            {
                db.EmplaModeloes.Add(emplaModelo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emplaModelo);
        }

        // GET: EmplaModeloes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmplaModelo emplaModelo = db.EmplaModeloes.Find(id);
            if (emplaModelo == null)
            {
                return HttpNotFound();
            }
            return View(emplaModelo);
        }

        // POST: EmplaModeloes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Modelo,Id_Marca,Modelo,FechaOp,FechaEdit,Id_Usuario,Id_UsuarioEdit")] EmplaModelo emplaModelo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emplaModelo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emplaModelo);
        }

        // GET: EmplaModeloes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmplaModelo emplaModelo = db.EmplaModeloes.Find(id);
            if (emplaModelo == null)
            {
                return HttpNotFound();
            }
            return View(emplaModelo);
        }

        // POST: EmplaModeloes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            EmplaModelo emplaModelo = db.EmplaModeloes.Find(id);
            db.EmplaModeloes.Remove(emplaModelo);
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
