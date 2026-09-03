using radiotaxi.API.Controllers.Base;
using radiotaxi.API.Hubs;
using radiotaxi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace radiotaxi.API.Controllers
{
    [Authorize(Roles = "soc")]
    [RoutePrefix("api/messages")]
    public class MessageController : BaseControllerWithHub<CardHub>
    {
        // GET: api/Message
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Message/5
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(string id)
        {
            try
            {
                MENSAJE mensaje = new MENSAJE(); // llamamos al objeto de mensaje...
                List<MENSAJE> mensajes = new List<MENSAJE>(); // Contenedor de mensajes
                mensajes = mensaje.getGafet(id); // Obtenemos los mensajes. 
                return Request.CreateResponse(
                    HttpStatusCode.OK, this.content<List<MENSAJE>>(mensajes),
                    Configuration.Formatters.JsonFormatter
                );
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest, this.content<string>(ex.Message),
                    Configuration.Formatters.JsonFormatter
                );
            }
        }

        // POST: api/Message
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Message/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Message/5
        public void Delete(int id)
        {
        }
    }
}
