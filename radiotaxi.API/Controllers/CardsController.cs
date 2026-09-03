using Microsoft.AspNet.SignalR.Hosting;
using Newtonsoft.Json;
using radiotaxi.API.Controllers.Base;
using radiotaxi.API.Hubs;
using radiotaxi.Model;
using radiotaxi.Model.API.cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace radiotaxi.API.Controllers
{
    [Authorize(Roles = "soc")]
    [RoutePrefix("api/cards")]
    public class CardsController : BaseControllerWithHub<CardHub>
    {
        // GET: api/Cards
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Cards/5
        
        [Route("{id}")]
        [ResponseType(typeof(CardResponse))]
        public HttpResponseMessage Get(int id)
        {
            CardResponse Response = new CardResponse();
            EmplacamientoDTO _emplacamiento = null;
            Partner _partner = new Partner();
            Operator _operator = new Operator();
            user User = null;
            card _card = new card().get(id);
            if (_card != null)
            {
                Response.Card = _card;
                Response.Car = new Emplacamiento().get(_card.taxi);
                if (_card.partnerReference.Contains("OP-"))
                {
                    Response.Operator = _operator.get(_card.partnerReference);
                }
                else{
                    Response.Partner = _partner.get(_card.partnerReference);
                }
            }
            return Request.CreateResponse(
                HttpStatusCode.OK, this.content<CardResponse>(Response),
                Configuration.Formatters.JsonFormatter
            );
        }

        // POST: api/Cards
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Cards/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Cards/5
        public void Delete(int id)
        {
        }
    }
}
