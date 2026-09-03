using Amazon.CloudFormation.Model;
using Microsoft.SqlServer.Server;
using radiotaxi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace radiotaxi.WEB.Controllers
{
    [RoutePrefix("finance")]
    public class financeController : webBaseController
    {
        // GET: finance
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult fdi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(DateTime start, DateTime end)
        {
            return View();
        }

        [HttpPost]
        public JsonResult fdi_soc(DateTime? start, DateTime? end)
        {
            try
            {
                List<fdi_result> result = new Partner().fdi(start, end);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        [HttpPost]
        public JsonResult fdi_chof(DateTime start, DateTime end)
        {
            try
            {
                List<fdi_result> result = new Operator().fdi(start, end);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public ActionResult families()
        {
            Cut cut = new Cut();
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddSeconds(-1);
            CutResults objResults = cut.get(firstDayOfMonth, lastDayOfMonth);
            return View(objResults);
        }

        [HttpPost]
        public ActionResult families(DateTime? start, DateTime? end)
        {
            try
            {
                DateTime date = DateTime.Now;
                if (start == null && end == null)
                {
                    start = new DateTime(date.Year, date.Month, 1);
                    end = ((DateTime)start).AddMonths(1).AddSeconds(-1);
                }
                else if (start == null && end != null)
                {
                    start = new DateTime(date.Year, date.Month, 1);
                }
                else if (start != null && end == null)
                {
                    end = date;
                }
                Cut cut = new Cut();
                CutResults objResults = cut.get(((DateTime)start), ((DateTime)end));
                return View(objResults);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("families/{date}/{FamilyID}")]
        public ActionResult families(string date, int FamilyID)
        {
            Cut cut = new Cut();
            DateTime _date = DateTime.Now;
            var firstDayOfMonth = new DateTime(_date.Year, _date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddSeconds(-1);
            CutResults objResults = cut.get(firstDayOfMonth, lastDayOfMonth);
            return View("~/Views/finance/familiesID", objResults);
        }

        // GET: finance/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: finance/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: finance/Create
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

        // GET: finance/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: finance/Edit/5
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

        // GET: finance/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: finance/Delete/5
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