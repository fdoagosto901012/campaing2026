using radiotaxi.WEB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace radiotaxi.WEB.Controllers
{
    [SessionFilter("Admin")]
    public class HomeController : Controller
    {
        public ActionResult Index(string email, string access_token, string role, string name, string company, string expires_in, string token_type)
        {
            Session["role"] = role;
            Session["name"] = name;
            Session["company"] = company;
            // Se guarda el objeto en session.
            Session["email"] = email;
            Session["access_token"] = access_token;
            Session["expires_in"] = expires_in;
            Session["token_type"] = token_type;
            // Construir el objeto Session en la plataforma.
            return RedirectToAction("Index", "Search");
        }
    }
}