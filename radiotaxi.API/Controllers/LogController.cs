using radiotaxi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using radiotaxi.API.Providers;
using System.Web.Http.Description;
using radiotaxi.Model.v2.General;
using radiotaxi.API.Controllers.Base;

namespace radiotaxi.API.Controllers
{
    [RoutePrefix("api/users")]
    public class LogController : BaseController
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("token/")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult Login(LoginDTO _userlog) {
            try
            {
                // Obtenemos los datos.
                var userName = _userlog.userName;
                var password = _userlog.password;
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                {
                    // Error falta usuario y contraseña
                    //AddToastMessage("", "Please enter valid username and password", ToastType.Error);
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, "Error : ingrese un usuario y contraseña."));
                }
                // Obtenemos la informacion del usuario.
                userlog User = new userlog();
                if (!User.log(userName, password))
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, "Error : Usuario o contraseña incorrectos."));
                }
                // Jwt Authentication code
                string encryptedPwd = password;
                var userPassword = password;
                var username = User.email;
                try
                {
                    var role = User.Role;
                    string jsonUser = null;
                    try
                    {
                        jsonUser = this.serialize((new user()).get((int)User.padronID));
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    var jwtToken = Authentication.GenerateJWTAuthetication(User, jsonUser, userName, role);
                    return Ok(jwtToken);
                }
                catch (Exception ex)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Error : error en login intente más tarde."));
                }
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Error : error server."));
            }    
        }

        [HttpPost]
        [Route("token/islog/")]
        public IHttpActionResult isLog()
        {
            try
            {
                string jwtToken = "";
                var validUserName = Authentication.ValidateToken(jwtToken);
                if (string.IsNullOrEmpty(validUserName))
                {
                    return Ok(validUserName);
                }
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Unauthorized, "Error : error server."));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Error : error server."));
            }
        }
    }
}