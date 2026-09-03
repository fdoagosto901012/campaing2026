using radiotaxi.Client.Interface;
using radiotaxi.Client.Request;
using radiotaxi.Model;
using radiotaxi.WEB.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace radiotaxi.WEB.Controllers
{
    //[SessionFilter]
    public class ReportController : Controller
    {
        static readonly iReport reportRequest = new rReport();
        // GET: Report
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(_IntervalDate Dates)
        {
            return View();
        }


        [HttpGet]
        public ActionResult GetTotalUser(string start, string end)
        {
            int statusConnection = 0;
            SearchWithDates dates = new SearchWithDates();
            if ((start == null) && (end == null) || (end == "Invalid date") || (start == "Invalid date"))
            {
                dates.start = DateTime.Now.AddDays(-2);
                dates.end = DateTime.Now.AddDays(-1);
            }
            else
            {
                dates.start = DateTime.Parse(start);
                dates.end = DateTime.Parse(end);
            }

            dates.start = dates.start.ToLocalTime();
            dates.end = dates.end.ToLocalTime();
            IEnumerable<totalReportUser> result = reportRequest.assignedbyUsers(dates, Session["access_token"].ToString(), ref statusConnection);
            if (result == null)
            {
                return null;
            }
            else
            {
                return PartialView("totalUser", result);
            }            
        }

        [HttpGet]
        public ActionResult GetTotal(string start, string end)
        {
            int statusConnection = 0;
            SearchWithDates dates = new SearchWithDates();
            if ((start == null) && (end == null) || (end == "Invalid date") || (start == "Invalid date"))
            {
                dates.start = DateTime.Now.AddDays(-2);
                dates.end = DateTime.Now.AddDays(-1);
            }
            else {
                dates.start = DateTime.Parse(start);
                dates.end = DateTime.Parse(end);
            }
            dates.start = dates.start.ToLocalTime();
            dates.end = dates.end.ToLocalTime();
            int result = reportRequest.allAssigned(dates, Session["access_token"].ToString(), ref statusConnection);
            if (result == null)
            {
                return null;
            }
            else
            {
                return PartialView("total", result);
            }
        }

        /***Created****/

        [HttpGet]
        public ActionResult GetTotalCreatedUser(string start, string end)
        {
            int statusConnection = 0;
            SearchWithDates dates = new SearchWithDates();
            if ((start == null) && (end == null) || (end == "Invalid date") || (start == "Invalid date"))
            {
                dates.start = DateTime.Now.AddDays(-2);
                dates.end = DateTime.Now.AddDays(-1);
            }
            else
            {
                dates.start = DateTime.Parse(start);
                dates.end = DateTime.Parse(end);
            }
            dates.start = dates.start.ToLocalTime();
            dates.end = dates.end.ToLocalTime();
            IEnumerable<totalReportUser> result = reportRequest.createdbyUsers(dates, Session["access_token"].ToString(), ref statusConnection);
            if (result == null)
            {
                return null;
            }
            else
            {
                return PartialView("totalCreatedUser", result);
            }
        }

        [HttpGet]
        public ActionResult GetTotalCreated(string start, string end)
        {
            int statusConnection = 0;
            SearchWithDates dates = new SearchWithDates();
            if ((start == null) && (end == null) || (end == "Invalid date") || (start == "Invalid date"))
            {
                dates.start = DateTime.Now.AddDays(-2);
                dates.end = DateTime.Now.AddDays(-1);
            }
            else
            {
                dates.start = DateTime.Parse(start);
                dates.end = DateTime.Parse(end);
            }
            dates.start = dates.start.ToLocalTime();
            dates.end = dates.end.ToLocalTime();
            int result = reportRequest.allCreated(dates, Session["access_token"].ToString(), ref statusConnection);
            if (result == 0)
            {
                return null;
            }
            else
            {
                return PartialView("totalCreated", result);
            }
        }


        [HttpPost]
        public JsonResult getStadistics(string start, string end)
        {
            try
            {
                CultureInfo _CultureInfo = new CultureInfo("es-MX");
                // BEGIN :: GetTotalUser :: Los servicios asignados por usuario.
                int statusConnection = 0;
                SearchWithDates dates = new SearchWithDates();
                if ((start == null) && (end == null) || (end == "Invalid date") || (start == "Invalid date"))
                {
                    dates.start = DateTime.Now.AddDays(-9);
                    dates.end = DateTime.Now.AddDays(-1);
                }
                else
                {
                    dates.start = DateTime.ParseExact(start, "dd/MM/yyyy",
                                                CultureInfo.InvariantCulture);
                    dates.end = DateTime.ParseExact(end, "dd/MM/yyyy",
                                                CultureInfo.InvariantCulture);
                }

                dates.start = dates.start.ToLocalTime();
                dates.end = dates.end.ToLocalTime();



                // BEGIN :: GET services by day
                ResultsRervicesCounter responseByDay = reportRequest.servicesByIntervalDay(dates, Session["access_token"].ToString(), ref statusConnection);
                // END :: GET services by day


                reports_DTO resp = new reports_DTO()
                {
                    days_report = responseByDay,
                    Created_by_user = null,
                    asigned_by_user = null
                };

                return Json(resp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json("chamara", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
