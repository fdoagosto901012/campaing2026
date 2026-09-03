using Microsoft.IdentityModel.Tokens;
using radiotaxi.API.Controllers.Base;
using radiotaxi.API.Hubs;
using radiotaxi.Model;
using radiotaxi.Model.API;
using radiotaxi.Model.API.cards;
using radiotaxi.Model.v2.finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace radiotaxi.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/qrscan")]
    public class QrScanController : BaseControllerWithHub<PartnerHub>
    {
        // ESTE METODO REGRESA TODA LA INFORMACION DE MANERA GLOBAL.
        // GET: api/Partner/5
        [HttpGet]
        [Route("{code}")]
        public async Task<HttpResponseMessage> getparterglobalAsync(string code)
        {
            try
            {

                // Declaracion de variables.
                _PartnerGlobalDTO Global = new _PartnerGlobalDTO(); // Esta variable se encarga de regresar toda la respuesta.
                ScanQRresponse response = new ScanQRresponse();
                response.ScanType = "N/A"; // Significa que no esta definido que tipo de scaner es.
                // Mediente el tipo de codigo obtenemos el objeto y hacemos la busqueda del partner reference.

                // Creamos un objeto global para poder obtener la informacion y transportarla.

                string partnerreference = "";
                string _id = ""; // Esta variable es para hacer el parse to int.. 
                // ESCANEO DE CUPON --- PENDIENTE ...
                if (!string.IsNullOrEmpty(code) && code.ToUpper().Contains("K"))
                {

                }
                // ESCANEO DE TARJETON *************** HECHO
                else if (!string.IsNullOrEmpty(code) && code.ToUpper().Contains("T-"))
                {
                    code = code.ToUpper().Replace("T-", "");
                    Cupon cupon = new Cupon();
                    int id = int.Parse(code);
                    CardResponse __CARD = new CardResponse();
                    EmplacamientoDTO _emplacamiento = null;
                    Partner _partner = new Partner();
                    Operator _operator = new Operator();
                    user User = null;
                    card _card = new card().get(id);
                    if (_card != null)
                    {
                        __CARD.Card = _card;
                        __CARD.Car = new Emplacamiento().get(_card.taxi);
                        // Asignamos el valor para que sea regresado... informacion del tarjeton...
                        response.Card = __CARD;
                        response.ScanType = "T"; // Significa que es un scaner de tipo terjeton....
                        partnerreference = _card.partnerReference; // asignamos la informacion del operador.
                        // Este paso es inecesario ya que se va a buscar al operador directamente del objeto global.
                        if (_card.partnerReference.Contains("OP-"))
                        {
                            //Response.Operator = _operator.get(_card.partnerReference);
                        }
                        else
                        {
                            //Response.Partner = _partner.get(_card.partnerReference);
                        }
                    }
                    //response.Cupon = cupon;
                }
                // ESCANEO DE CREDENCIAL ************* HECHO
                else if (!string.IsNullOrEmpty(code) && code.ToUpper().Contains("C-"))
                {
                    partnerreference = code.ToUpper().Replace("C-", "");
                    response.ScanType = "C"; // coloca que el tipo de scaner es credencial.
                }
                // ESCANEO DE PERMISO. *************** HECHO
                else if (!string.IsNullOrEmpty(code) && code.ToUpper().Contains("E"))
                {

                    PermissionsResponse permissionsResponse = new PermissionsResponse();
                    code = code.ToUpper().Replace("E", "");
                    int id = int.Parse(code);
                    // Buscamos el permiso que vamos localizar...
                    C_permissions c_Permissions = new C_permissions();
                    permissionsResponse.current = c_Permissions.get(id); // Obtenemos los vencidos.
                    partnerreference = permissionsResponse.current.user.partnerReference;
                    permissionsResponse.permissions = c_Permissions.get(partnerreference); // Obtenemos los vencidos.
                    response.Permissions = permissionsResponse;
                    response.ScanType = "E";

                    // Obtenemos si esta pagado o no esta pagado el permiso.
                    foreach (var permission in permissionsResponse.permissions) {
                        if (permissionsResponse.current.id == permission.id)
                        {
                            permissionsResponse.Exist = permission.Exist;
                        }
                    }
                }


                if (!partnerreference.IsNullOrEmpty()) {
                    // VAMOS A OBTENER TODA LA INFORMACION
                    if (partnerreference.ToUpper().Contains("OP-"))
                    {
                        // SE BUSCA EN OPERAORES.
                        Global.Operator = new Operator().get(partnerreference);
                        // Rellenamos los datos generales.
                        Global.userID = Global.Operator.userId;
                        Global.partnerReference = Global.Operator.partnerReference;
                        Global.firstName = Global.Operator.firstName;
                        Global.lastNameF = Global.Operator.lastNameF;
                        Global.lastNameM = Global.Operator.lastNameM;
                        Global.amazonPictures = Global.Operator.amazonPictures != null ? Global.Operator.amazonPictures.ToList() : new List<amazonPicture>();
                    }
                    else
                    {
                        Global.Partner = (new Partner()).get(partnerreference);
                        // Rellenamos los datos generales.
                        Global.userID = Global.Partner.userId;
                        Global.partnerReference = Global.Partner.partnerReference;
                        Global.firstName = Global.Partner.firstName;
                        Global.lastNameF = Global.Partner.lastNameF;
                        Global.lastNameM = Global.Partner.lastNameM;
                        Global.amazonPictures = Global.Partner.amazonPictures != null ? Global.Partner.amazonPictures.ToList() : new List<amazonPicture>();
                    }

                    var financesTask = Task.Run(() => getFinances(Global)); // Obtenemos las finanzas del usuario.
                    var debsTask = Task.Run(() => getDebs(Global.partnerReference)); // Obtenemos las deudas de un usuario.
                    var AsistTask = Task.Run(() => getAsist(Global)); // Obtenemos la asistencia de un usuario.
                    var blockTask = Task.Run(() => getBlocks(Global.partnerReference)); // Obtenemos la asistencia de un usuario.
                    var TicketsTask = Task.Run(() => getTickets(Global)); // Obtenemos los tickets de un usuario.
                    var AgreementsTask = Task.Run(() => getAgreements(Global)); // Obtenemos convenios.
                    var EmplacadoTask = Task.Run(() => getEmplacamiento(Global.partnerReference)); // Obtenemos el auto emplacado.
                    await Task.WhenAll(financesTask, debsTask, AsistTask, blockTask, TicketsTask, AgreementsTask, EmplacadoTask);
                    Global.finances = financesTask.Result;
                    Global.debts = debsTask.Result;
                    Global.blocks = blockTask.Result;
                    Global.Reportes = AsistTask.Result;
                    Global.tickets = TicketsTask.Result;
                    Global.tickets = TicketsTask.Result;
                    Global.agreements = AgreementsTask.Result;
                    Global.Emplacamiento = EmplacadoTask.Result;
                    response.globalUser = Global; // Asignamos al usuario localizado globalmente, Nosabemos si es operador o socio.
                }
                return Request.CreateResponse(
                    HttpStatusCode.OK, this.content<ScanQRresponse>(response),
                    Configuration.Formatters.JsonFormatter
                );
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private financesOperator getFinances(_PartnerGlobalDTO global)
        {
            try
            {
                // Obtener faltas
                if (global.Partner != null)
                {
                    return global.Partner.financesPartner();
                }
                else
                {
                    return global.Operator.financesOperator();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private List<caja_cargos_DTO> getDebs(string partnerReference)
        {
            try
            {
                Sale sale = new Sale(partnerReference);
                return sale.debs(partnerReference, partnerReference);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private List<MENSAJE> getBlocks(string partnerReference)
        {
            try
            {
                // Obtenemos los mensajes
                MENSAJE mENSAJE = new MENSAJE();
                return mENSAJE.getEco(partnerReference);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private List<HistAsistencia> getAsist(_PartnerGlobalDTO global)
        {
            try
            {
                // Obtener faltas
                if (global.Partner != null)
                {
                    return global.Partner.reportes();
                }
                else
                {
                    return global.Operator.reportes();
                }
            }
            catch (Exception ex)
            {
                return new List<HistAsistencia>();
            }
        }
        private List<Venta> getTickets(_PartnerGlobalDTO global)
        {
            try
            {
                // Obtener faltas
                if (global.Partner != null)
                {
                    return global.Partner.sales();
                }
                else
                {
                    return global.Operator.sales();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private List<Convenio> getAgreements(_PartnerGlobalDTO global)
        {
            try
            {
                // Obtener faltas
                if (global.Partner != null)
                {
                    return global.Partner.agreements();
                }
                else
                {
                    return global.Operator.agreements();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        private EmplacamientoDTO getEmplacamiento(string partnerReference)
        {
            try
            {
                EmplacamientoDTO _emplacamiento = new Emplacamiento().get(partnerReference);
                return _emplacamiento;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}