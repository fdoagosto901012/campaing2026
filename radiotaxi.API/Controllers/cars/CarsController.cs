using radiotaxi.API.Controllers.Base;
using radiotaxi.API.Hubs;
using radiotaxi.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace radiotaxi.API.Controllers.cars
{
    [RoutePrefix("api/cars")]
    public class CarsController : BaseControllerWithHub<CarsHub>
    {
        // GET: api/Cars
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Cars/5
        [HttpGet]
        [Route("{id}")]
        public EmplacamientoDTO Get(string id)
        {
            EmplacamientoDTO _emplacamiento = new Emplacamiento().get(id);
            return _emplacamiento;
        }

        // POST: api/Cars
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Cars/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Cars/5
        public void Delete(int id)
        {
        }
    }
}
