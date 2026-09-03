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
    //[Authorize(Roles = "soc")]
    [RoutePrefix("api/contacts")]

    public class OfficeContactController : BaseControllerWithHub<PartnerHub>
    {
        // GET: api/OfficeContact
        [HttpGet]
        [Route("all")]
        public HttpResponseMessage Get()
        {
            OfficeContact officeContact = new OfficeContact();
            List<OfficeContact> Response =  officeContact.get();
            return Request.CreateResponse(
                HttpStatusCode.OK, this.content<Partner>(Response),
                Configuration.Formatters.JsonFormatter
            );
        }
    }
}
