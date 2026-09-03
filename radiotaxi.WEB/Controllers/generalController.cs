using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using radiotaxi.Model;
using radiotaxi.WEB.Helper;

namespace radiotaxi.WEB.Controllers
{
    [SessionFilter("Admin, Practi-Emplacamiento")]
    public class generalController : webBaseController
    {
        private radiotaxiEntities db = new radiotaxiEntities();
        private Venta01Entities dbventas = new Venta01Entities();

        [HttpPost]
        public ActionResult getUserTaxi(string PartnerReference)
        {
            string query = "select * from [user] as u \r\nwhere active = 1 and u.partnerReference = '" + PartnerReference + "';";
            user _user = db.Database.SqlQuery<user>(query).FirstOrDefault();
            try
            {
                return Content(this.serialize(_user), "application/json");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}