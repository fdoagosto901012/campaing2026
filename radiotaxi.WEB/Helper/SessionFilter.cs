using Amazon.Runtime.Internal;
using radiotaxi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace radiotaxi.WEB.Helper
{
    public class SessionFilter : ActionFilterAttribute
    {
        public List<string> roles { get; set; }
        public SessionFilter() { 
        
        }

        public SessionFilter(string Roles = "")
        {
            this.roles = Roles.Replace(" ","").Split(',').ToList();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string ip = filterContext.RequestContext.HttpContext.Request.UserHostAddress; // Obtenemos la IP.
            var Session = filterContext.HttpContext.Session;
            if (String.IsNullOrEmpty((string)(Session["role"])))
            {
                //Redirect him to somewhere.
                var redirectTarget = new RouteValueDictionary { { "action", "Login" }, { "controller", "Login" } };
                filterContext.Result = new RedirectToRouteResult(redirectTarget);
            }
#if RELEASE
            else if (!this.roles.Contains("admin") && (Session["_IP"].ToString()) != ip)
            {
                var redirectTarget = new RouteValueDictionary { { "action", "LoginIPERROR" }, { "controller", "Error" } };
                filterContext.Result = new RedirectToRouteResult(redirectTarget);
            }
#endif
            else if (this.roles != null && !this.roles.Contains(Session["role"].ToString()))
            {
                var redirectTarget = new RouteValueDictionary { { "action", "LoginErrors" }, { "controller", "Error" } };
                filterContext.Result = new RedirectToRouteResult(redirectTarget);
            }



                
        }
    }
}