using radiotaxi.API.Controllers.Base;
using radiotaxi.API.Hubs;
using radiotaxi.Model;
using radiotaxi.Model.v2.GeoLocations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace radiotaxi.API.Controllers
{
    [Authorize(Roles = "soc")]
    [RoutePrefix("api/Geolocalization")]
    public class GeolocalizationController : BaseControllerWithHub<geolocalizationHub>
    {
        // GET: api/Geolocalization
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Geolocalization/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                userGeolocation geolocation = new userGeolocation();
                geolocation = geolocation.getLast(id);
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest, this.content<userGeolocation>(geolocation),
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


        // POST: api/Geolocalization
        [Route("save")]
        public HttpResponseMessage Post([FromBody] userGeolocationDTO obj)
        {
            try
            {
                // Guardamos el objeto
                userGeolocation geolocation = new userGeolocation();
                geolocation.UserId = obj.UserId;
                geolocation.DateCreate = DateTime.Now;
                geolocation.Latitud = obj.Latitud;
                geolocation.Longitud = obj.Longitud;
                geolocation.speed = obj.speed;
                geolocation.type = obj.type;
                geolocation.OpenPayOrderId = obj.OpenPayOrderId;
                if (geolocation.save())
                {
                    // Asignamos al DTO 
                    obj.Id = geolocation.Id;
                    //obj.geolocation = geolocation.geolocation;
                    obj.DateCreate = geolocation.DateCreate;
                    
                    return Request.CreateResponse(
                        HttpStatusCode.OK, this.content<userGeolocationDTO>(obj),
                        Configuration.Formatters.JsonFormatter
                    );
                }
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest, this.content<string>(geolocation.Message),
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

        // POST: api/Geolocalization
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Geolocalization/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Geolocalization/5
        public void Delete(int id)
        {
        }
    }
}
