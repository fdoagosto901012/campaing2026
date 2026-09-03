using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace radiotaxi.WEB
{
    public static class Permissions
    {
        public static bool allow(string roles) {
            roles = roles.Replace(" ", "");
            string[] _roles = roles.Split(',');
            return _roles.Contains(HttpContext.Current.Session["role"].ToString());
        }

        public static bool deny(string roles)
        {
            roles = roles.Replace(" ", "");
            string[] _roles = roles.Split(',');
            return !_roles.Contains(HttpContext.Current.Session["role"].ToString());
        }
    }
}