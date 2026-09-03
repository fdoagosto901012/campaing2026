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
    public class EmplaMarcasController : Controller
    {
        private radiotaxiEntities db = new radiotaxiEntities();

        // GET: EmplaMarcas
        public ActionResult Index()
        {
            return View(db.EmplaMarcas.ToList());
        }

        // GET: EmplaMarcas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmplaMarca emplaMarca = db.EmplaMarcas.Find(id);
            if (emplaMarca == null)
            {
                return HttpNotFound();
            }
            return View(emplaMarca);
        }

        // GET: EmplaMarcas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmplaMarcas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Marca,Marca,FechaOp,FechaEdit,Id_Usuario,Id_UsuarioEdit")] EmplaMarca emplaMarca)
        {
            if (ModelState.IsValid)
            {
                db.EmplaMarcas.Add(emplaMarca);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emplaMarca);
        }

        // GET: EmplaMarcas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmplaMarca emplaMarca = db.EmplaMarcas.Find(id);
            if (emplaMarca == null)
            {
                return HttpNotFound();
            }
            return View(emplaMarca);
        }

        // POST: EmplaMarcas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Marca,Marca,FechaOp,FechaEdit,Id_Usuario,Id_UsuarioEdit")] EmplaMarca emplaMarca)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emplaMarca).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emplaMarca);
        }

        // GET: EmplaMarcas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmplaMarca emplaMarca = db.EmplaMarcas.Find(id);
            if (emplaMarca == null)
            {
                return HttpNotFound();
            }
            return View(emplaMarca);
        }

        // POST: EmplaMarcas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            EmplaMarca emplaMarca = db.EmplaMarcas.Find(id);
            db.EmplaMarcas.Remove(emplaMarca);
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
