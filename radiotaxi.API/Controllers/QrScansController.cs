using Microsoft.AspNet.SignalR.Hosting;
using Newtonsoft.Json;
using radiotaxi.API.Controllers.Base;
using radiotaxi.API.Hubs;
using radiotaxi.Model;
using radiotaxi.Model.API.cards;
using radiotaxi.Model.v2;
using radiotaxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using user = radiotaxi.Model.user;

namespace radiotaxi.API.Controllers
{
    [Authorize(Roles = "soc")]
    [RoutePrefix("api/qrscans")]
    public class QrScansController : BaseControllerWithHub<CardHub>
    {
        string patTarjeton = @"\bT-\d+";
        string patPermiso = @"\be\d+";
        string patCredencial = @"\bC-\d+";

        // GET: api/Cards
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("{code}")]
        [ResponseType(typeof(CardResponse))]
        public async Task<HttpResponseMessage> getQRscaninfo(string code)
        {
            try
            {
                Regex permiso = new Regex(patPermiso, RegexOptions.IgnoreCase); // Verificar permiso.
                Regex tarjeton = new Regex(patTarjeton, RegexOptions.IgnoreCase); // Verificar tarjeton.
                Regex credencial = new Regex(patCredencial, RegexOptions.IgnoreCase); // Verificar credencial.

                QrResponse Response = new QrResponse(); // objeto a rellentar.

                if ((tarjeton.Match(code)).Success) // Es un tarjeton... (Renderiza la vista de tarjton)
                {
                    card Card = new card();
                    code = code.ToLower().Replace("t-", "");
                    int id = 0;
                    int.TryParse(code, out id);
                    Card =  Card.get(id);
                    if (Card != null) // Si el permiso es distinto a nulo, entonces rellena todos los datos.
                    {
                        Response.card = Card;
                        Response.global = await getGlobal(Card.partnerReference);
                        Response.emplacamiento = new Emplacamiento().get(Card.taxi);
                        Response.typescan = "t";
                    }
                }
                else if ((permiso.Match(code)).Success)
                { // Verificar si hay permiso.
                    code = code.ToLower().Replace("e", "");
                    int id = 0;
                    int.TryParse(code, out id);
                    C_permissions permissions = new C_permissions();
                    permissions = permissions.get(id); // Obtenemos el permiso.
                    if (permissions != null) // Si el permiso es distinto a nulo, entonces rellena todos los datos.
                    {
                        Response.Permission = permissions;
                        Response.global = await getGlobal(permissions.gafet);
                        Response.emplacamiento = new Emplacamiento().get(permissions.taxi);
                    }
                    Response.typescan = "e";
                }
                else if ((credencial.Match(code)).Success)
                {
                    code = code.ToLower().Replace("c-", "");
                    int id = 0;
                    int.TryParse(code, out id);
                    Response.global = await getGlobal(code);
                    Response.emplacamiento = new Emplacamiento().get(code);
                    Response.typescan = "c";
                }
                // Regresamos el response...
                return Request.CreateResponse(
                    HttpStatusCode.OK, this.content<QrResponse>(Response),
                    Configuration.Formatters.JsonFormatter
                );
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest, this.content<QrResponse>(null),
                    Configuration.Formatters.JsonFormatter
                );
            }
            
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
