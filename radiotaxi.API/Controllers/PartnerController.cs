using Microsoft.AspNet.SignalR.Hosting;
using radiotaxi.API.Controllers.Base;
using radiotaxi.API.Hubs;
using radiotaxi.Model;
using radiotaxi.Model.API.cards;
using radiotaxi.Model.v2.finance;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace radiotaxi.API.Controllers
{

    [Authorize]
    [RoutePrefix("api/partner")]
    public class PartnerController : BaseControllerWithHub<PartnerHub>
    {
        [HttpGet]
        // GET: api/Partner
        public HttpResponseMessage Get()
        {
            Partner Response = new Partner();   
            return Request.CreateResponse(
                HttpStatusCode.OK, this.content<CardResponse>(Response),
                Configuration.Formatters.JsonFormatter
            );
        }

        // GET: api/Partner/5
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(string id)
        {
            Partner Response = (new Partner()).get(id); // Obteneoms la informacion del socio.
            return Request.CreateResponse(
                HttpStatusCode.OK, this.content<Partner>(Response),
                Configuration.Formatters.JsonFormatter
            );
        }

        [HttpGet]
        [Route("debs/{gafet}")]
        public HttpResponseMessage debs(string gafet)
        {
            Sale sale = new Sale(gafet);
            List<caja_cargos_DTO> Response = sale.debs(gafet, gafet);
            return Request.CreateResponse(
                HttpStatusCode.OK, this.content<List<caja_cargos_DTO>>(Response),
                Configuration.Formatters.JsonFormatter
            );
        }

        [HttpGet]
        [Route("tickets/{gafet}")]
        public async Task<HttpResponseMessage> ticketsAsync(string gafet)
        {
            try
            {
                _PartnerGlobalDTO user = await this.getTickets(gafet);
                List<Venta> tickets = user.tickets;
                return Request.CreateResponse(
                    HttpStatusCode.OK, this.content<List<Venta>>(tickets),
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

        // POST: api/Partner
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Partner/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Partner/5
        public void Delete(int id)
        {
        }


        // ESTE METODO REGRESA TODA LA INFORMACION DE MANERA GLOBAL.
        // GET: api/Partner/5
        [HttpGet]
        [Route("all/{partnerreference}")]
        public async Task<HttpResponseMessage> getparterglobalAsync(string partnerreference)
        {
            try
            {
                // Declaracion de variables.
                _PartnerGlobalDTO Global = await this.getGlobal(partnerreference);
                return Request.CreateResponse(
                    HttpStatusCode.OK, this.content<_PartnerGlobalDTO>(Global),
                    Configuration.Formatters.JsonFormatter
                );
            }
            catch (Exception ex)
            {
                throw;
            }    
        }




    }
}
