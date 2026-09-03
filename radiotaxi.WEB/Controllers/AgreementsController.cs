using Microsoft.Ajax.Utilities;
using radiotaxi.Model;
using radiotaxi.WEB.Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace radiotaxi.WEB.Controllers
{
    public class AgreementsController : webBaseController
    {
        // GET: Agreements
        public ActionResult Index()
        {

            return View();
        }

        // GET: Agreements/Details/5
        public ActionResult Details(string id)
        {
            // Obtenemos las el convenio.
            Convenio conv = db.Convenios.Where(x => x.Id_Op == id).FirstOrDefault();
            List<ConveniosDetalle> convdetalles = db.ConveniosDetalles.Where(x => x.Id_Op == id).ToList();
            Dictionary<ConveniosDetalle, Inventario> dic = new Dictionary<ConveniosDetalle, Inventario>();
            foreach (ConveniosDetalle detalle in convdetalles)
            {
                Inventario inventario = db.Inventarios.Where(z => z.Id_Producto == detalle.Id_Producto).FirstOrDefault();
                dic.Add(detalle, inventario);
            }
            ViewBag.conv = conv;
            ViewBag.convdetalles = convdetalles;
            ViewBag.dic = dic;
            return View(conv);
        }

        // GET: Agreements/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agreements/Create
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

        // GET: Agreements/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Agreements/Edit/5
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

        // GET: Agreements/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Agreements/Delete/5
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

        [HttpPost]
        [Route("json/cancelar/convenio")]
        [SessionFilter("Admin")]
        public ActionResult cancelarconvenio(string id_op)
        {
            try
            {
                string EditBy = this.User.firstName + " " + this.User.lastName;
                bool hasPayments = db.ConveniosDetalles.Any(x => 
                x.Id_Op == id_op && x.Status == "PAGADO" 
                );

                bool hasCollected = db.ConveniosDetalles.Any(x =>
                x.Id_Op == id_op && x.Status == "COBRADO" ||
                x.Id_Op == id_op && x.Status == "COBRADO  "
                );
                Convenio conv = db.Convenios.Where(x => x.Id_Op == id_op).FirstOrDefault();
                List<ConveniosDetalle> convdetalles = db.ConveniosDetalles.Where(x => x.Id_Op == id_op).ToList();
                Dictionary<ConveniosDetalle, Inventario> dic = new Dictionary<ConveniosDetalle, Inventario>();
                foreach (ConveniosDetalle detalle in convdetalles)
                {
                    Inventario inventario = db.Inventarios.Where(z => z.Id_Producto == detalle.Id_Producto).FirstOrDefault();
                    dic.Add(detalle, inventario);
                }
                ViewBag.conv = conv;
                ViewBag.convdetalles = convdetalles;
                ViewBag.dic = dic;

                using (var db = new radiotaxiEntities())
                {
                    if (hasCollected)
                    {
                        // SI no tiene partidas cobradas NO ejecutar...
                        db.Database.ExecuteSqlCommand(
                            "EXEC dbo.AJR_ACTUMARCAPAGADOCONVENIO @FOLIO, @FECHAEDIT, @ID_USUARIOEDIT",
                            new SqlParameter("@FOLIO", id_op),
                            new SqlParameter("@FECHAEDIT", DateTime.Now),
                            new SqlParameter("@ID_USUARIOEDIT", "10-1")
                        );
                    }
                    
                    // Cancela el convenio de las cablas de convenio y convenio detalle.
                    db.Database.ExecuteSqlCommand(
                        "EXEC dbo.AJR_CANCELACONVENIOS @FOLIO, @FECHAEDIT, @ID_USUARIOEDIT",
                        new SqlParameter("@FOLIO", id_op),
                        new SqlParameter("@FECHAEDIT", DateTime.Now),
                        new SqlParameter("@ID_USUARIOEDIT", "10-1")
                    );
                    // Este cancela el convenio del inventario.
                    db.Database.ExecuteSqlCommand(
                        "EXEC dbo.AJR_CANCELACONVENIOPARTIDAINV @FOLIO",
                        new SqlParameter("@FOLIO", id_op)
                    );
                }
                return Content(this.serialize(new { httpcode = 200, status = "Success", message = "Convenio cancelado correctamente." }), "application/json");
            }
            catch(Exception ex)
            {
                return Content(this.serialize(new { httpcode = 400, status = "Error", message = ex.Message }), "application/json");
            }
        }

        [HttpPost]
        [Route("json/cancelar/convenio/partida")]
        [SessionFilter("Admin")]
        public ActionResult cancelarconvenio(string id_op, int folio)
        {
            try
            {
                string EditBy = this.User.firstName + " " + this.User.lastName;
                bool hasPayments = db.ConveniosDetalles.Any(x =>
                x.Id_Op == id_op && x.Folio == folio && x.Status == "PAGADO" ||
                x.Id_Op == id_op && x.Folio == folio && x.Status == "COBRADO" ||
                x.Id_Op == id_op && x.Folio == folio && x.Status == "COBRADO  "
                );
                Convenio conv = db.Convenios.Where(x => x.Id_Op == id_op).FirstOrDefault();
                List<ConveniosDetalle> convdetalles = db.ConveniosDetalles.Where(x => x.Id_Op == id_op).ToList();
                Dictionary<ConveniosDetalle, Inventario> dic = new Dictionary<ConveniosDetalle, Inventario>();
                foreach (ConveniosDetalle detalle in convdetalles)
                {
                    Inventario inventario = db.Inventarios.Where(z => z.Id_Producto == detalle.Id_Producto).FirstOrDefault();
                    dic.Add(detalle, inventario);
                }

                ConveniosDetalle convdetalle = db.ConveniosDetalles.Where( 
                    
                    x => 
                    x.Id_Op == id_op && x.Folio == folio && x.Status == "PENDIENTE" ||
                    x.Id_Op == id_op && x.Folio == folio && x.Status == "VIGENTE" 

                    ).FirstOrDefault();
                
                ViewBag.conv = conv;
                ViewBag.convdetalles = convdetalles;
                ViewBag.dic = dic;


                using (var db = new radiotaxiEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC dbo.AJR_CANCELACONVENIOPARTIDAINV @FOLIO",
                        new SqlParameter("@FOLIO", folio)
                    );
                }
                return Content(this.serialize(new { httpcode = 200, status = "Success", message = "Convenio cancelado correctamente." }), "application/json");


                if (!hasPayments) // Si no hay pagos, se puede cancelar el convenio
                {
                    using (var db = new radiotaxiEntities())
                    {
                        db.Database.ExecuteSqlCommand(
                            "EXEC dbo.AJR_CANCELACONVENIOPARTIDAINV @FOLIO",
                            new SqlParameter("@FOLIO", folio)
                        );
                    }
                    return Content(this.serialize(new { httpcode = 200, status = "Success", message = "Convenio cancelado correctamente." }), "application/json");
                }
                else
                {
                    return Content(this.serialize(new { httpcode = 400, status = "Error", message = "No se puede cancelar el convenio, ya que tiene pagos realizados." }), "application/json");
                }
            }
            catch
            {
                return Content(this.serialize(new { httpcode = 400, status = "Error", message = "No se puede cancelar el convenio, error del sistema." }), "application/json");

            }
        }

        [HttpPost]
        [Route("json/convenio/detalles")]
        [SessionFilter("Admin")]
        public ActionResult getagreement(string id_op, string reference)
        {
            try
            {
                // Obtenemos las el convenio.
                Convenio conv = db.Convenios.Where(x => 
                    x.Id_Op == id_op && x.CargoANum == reference ||
                    x.Id_Op == id_op && x.AfavordeNum == reference
                ).FirstOrDefault();
                if (conv == null)
                {
                    return Content(this.serialize(new { httpcode = 400, status = "Error", message = "No se encontro el convenio." }), "application/json");
                }
                List<ConveniosDetalle> convdetalles = db.ConveniosDetalles.Where(x => x.Id_Op == id_op).ToList();
                List<object> dic = new List<object>();
                foreach (var detalle in convdetalles)
                {
                    var inventario = db.Inventarios
                        .FirstOrDefault(x => x.Id_Producto == detalle.Id_Producto);
                    dic.Add(
                        new { detalle = detalle, inventario = inventario }
                    );
                }
                object result = new
                {
                    convenio = conv,
                    detalles = convdetalles,
                    partidas = dic
                };
                return Content(this.serialize(new { 
                    httpcode = 200, 
                    status = "Success", 
                    message = "Convenio encontrado.",
                    result = result
                }), "application/json");
            }
            catch
            {
                return Content(null, "application/json");
            }
        }
    }
}