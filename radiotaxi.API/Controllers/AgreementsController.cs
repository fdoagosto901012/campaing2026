using Microsoft.AspNet.SignalR.Hosting;
using radiotaxi.API.Controllers.Base;
using radiotaxi.API.Hubs;
using radiotaxi.Model;
using radiotaxi.Model.v2.finance;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace radiotaxi.API.Controllers
{
    [Authorize(Roles = "soc")]
    [RoutePrefix("api/agreements")]
    public class AgreementsController : BaseControllerWithHub<CardHub>
    {
        [HttpGet]
        [Route("{gafet}")]
        // GET: api/Agreements
        public HttpResponseMessage Get(string gafet)
        {
            try
            {
                List<Convenio> agreements = new List<Convenio>();
                if (gafet.ToUpper().Contains("OP"))
                {
                    // Es un operador.
                    Operator _operator = new Operator();
                    _operator = _operator.get(gafet); // Obtenemos la informacion del operador.
                    agreements = _operator.agreements();
                }
                else
                {
                    // Es un socio.
                    Partner _operator = new Partner();
                    _operator = _operator.get(gafet); // Obtenemos la informacion del socio.
                    agreements = _operator.agreements();
                }
                return Request.CreateResponse(
                    HttpStatusCode.OK, this.content<List<Convenio>>(agreements),
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

        // GET: api/Agreements/get/5
        [HttpGet]
        [Route("get/{id}")]
        public HttpResponseMessage getsingle(string id)
        {
            try
            {
                Convenio convenio = new Convenio();
                ConvenioResponse response = convenio.get(id);
                return Request.CreateResponse(
                    HttpStatusCode.OK, this.content<ConvenioResponse>(response),
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

        // POST: api/Agreements
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Agreements/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Agreements/5
        public void Delete(int id)
        {
        }
    }
}
