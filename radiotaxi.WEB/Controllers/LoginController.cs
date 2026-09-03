using radiotaxi.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace radiotaxi.WEB.Controllers
{
    public class LoginController : webBaseController
    {
        public ActionResult Login()
        {
            if (Session != null)
            {
                Session.Abandon();
            }
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            try
            {
                this.User = new userlog();
                if (this.User.log(collection.Get("username"), collection.Get("password")))
                {
                    this.Name = this.User.firstName;
                    this.Role = this.User.Role; // DEFINE EL ROLE PARA LA ADMINSITRACION Y NIVELES DE USUARIOS.
                    this.UserName = this.User.firstName + " " + this.User.lastName;
                    this.email = this.User.email;
                    this.IP = this.User.IP;
                    if (Permissions.allow("Admin, Practi-ViewerSOC, Practi-Cred, Practi-BasicView"))
                    {
                        return RedirectToAction("Index", "Partner");
                    }else if (Permissions.allow("Practi-Emplacamiento"))
                    {
                        return RedirectToAction("Index", "Emplacamiento");
                    }
                    else if (Permissions.allow("Practi-FDI"))
                    {
                        return RedirectToAction("fdi", "finance");
                    }
                    return RedirectToAction("Index", "Operator");
                }
                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return base.RedirectToAction("Login", "Login", null);
        }
    }
}