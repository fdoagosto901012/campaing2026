using Newtonsoft.Json;
using radiotaxi.Model;
using radiotaxi.WEB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace radiotaxi.WEB.Controllers
{
    [RoutePrefix("Coupons")]
    public class CouponsController : webBaseController
    {
        // GET: Coupons
        public ActionResult Index()
        {
            return View();
        }

        // GET: Coupons/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Coupons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Coupons/Create
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

        // GET: Coupons/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Coupons/Edit/5
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

        // GET: Coupons/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Coupons/Delete/5
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

        // MIS PINCHES VISTAS D: //
        // GET: Coupons/Details/5

        //PAGAR CUPONES
        public ActionResult PayCoupon()
        {
            // Obtener todas las perforaciones.
            Coupon _coupon = new Coupon();
            ViewBag.sectores = _coupon.Sectores();
            ViewBag.perforaciones = _coupon.perforaciones();

            return View();
        }
        //REPORTE DE PAGO DE CUPONES POR CAJERA
        public ActionResult CashierCouponPaymentReport()
        {
            return View();
        }

        // REPORTE DE PAGOS DE CUPONES
        public ActionResult CouponPaymentReport()
        {
            return View();
        }
        // FACTURACION
        public ActionResult Invoice()
        {
            return View();
        }

        // REIMPRESIÓN DE FACTURA
        public ActionResult PrintInvoice()
        {
            return View();
        }

        // BLOQUEOS
        public ActionResult Block()
        {
            return View();
        }

        [HttpPost]
        [SessionFilter("Admin")]
        [Route("search/")]
        public ActionResult saerch(string sector, string perforacion)
        {
            try
            {
                Coupon coupon = new Coupon();
                List<TarifaCupone> tarifa = null;// coupon.getRate(sector, perforacion);
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                return Content(this.serialize(tarifa), "application/json");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
