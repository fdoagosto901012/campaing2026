using Microsoft.AspNet.SignalR.Hosting;
using Newtonsoft.Json;
using radiotaxi.API.Controllers.Base;
using radiotaxi.API.Hubs;
using radiotaxi.Model;
using radiotaxi.Model.API;
using radiotaxi.Model.API.cards;
using radiotaxi.Model.DTO;
using radiotaxi.Model.v2.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace radiotaxi.API.Controllers
{
    [Authorize(Roles = "soc")]
    [RoutePrefix("api/places")]
    public class placeController : BaseController
    {
        // GET: api/place
        [Route("state")]
        public HttpResponseMessage Get()
        {
            List<state> list = new state().get();
            return Request.CreateResponse(
                HttpStatusCode.OK, this.content<List<state>>(list),
                Configuration.Formatters.JsonFormatter
            );
        }

        [Route("state/{id}/cities")]
        public HttpResponseMessage Getcity(int id)
        {
            List<city> list = new city().get(id);
            return Request.CreateResponse(
                HttpStatusCode.OK, this.content<List<city>>(list),
                Configuration.Formatters.JsonFormatter
            );
        }

        // GET: api/place/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/place
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/place/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/place/5
        public void Delete(int id)
        {
        }
    }
}
