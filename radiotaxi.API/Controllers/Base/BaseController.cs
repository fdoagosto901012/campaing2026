using Newtonsoft.Json;
using radiotaxi.Model;
using radiotaxi.Model.v2.finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace radiotaxi.API.Controllers.Base
{
    public class BaseController : ApiController
    {
        public BaseController() {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        protected object content<T>(Object a) {

            string json = JsonConvert.SerializeObject(a, Formatting.Indented,
                                new JsonSerializerSettings
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                });
            T b = JsonConvert.DeserializeObject<T>(json);
            return b;
        }

        protected string serialize(Object a)
        {
            try
            {
                return JsonConvert.SerializeObject(a, Formatting.Indented,
                                new JsonSerializerSettings
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected  JsonResult Json(object data, string contentType,
            Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        protected async Task<_PartnerGlobalDTO> getGlobal(string partnerreference) {
            try
            {
                // Declaracion de variables.
                _PartnerGlobalDTO Global = new _PartnerGlobalDTO();
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
                    Global.status = Global.Operator.STATUS;
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
                    Global.status = "A";
                }

                var financesTask = Task.Run(() => getFinances(Global)); // Obtenemos las finanzas del usuario.
                var debsTask = Task.Run(() => getDebs(Global.partnerReference)); // Obtenemos las deudas de un usuario.
                var AsistTask = Task.Run(() => getAsist(Global)); // Obtenemos la asistencia de un usuario.
                var blockTask = Task.Run(() => getBlocks(Global.partnerReference)); // Obtenemos la asistencia de un usuario.
                var TicketsTask = Task.Run(() => getTickets(Global)); // Obtenemos los tickets de un usuario.
                var AgreementsTask = Task.Run(() => getAgreements(Global)); // Obtenemos convenios.
                var EmplacadoTask = Task.Run(() => getEmplacamiento(Global.partnerReference)); // Obtenemos el auto emplacado.
                var LastReportTask = Task.Run(() => getLastReport(Global.partnerReference)); // Obtenemos el auto emplacado.
                var RtypReportTaks = Task.Run(() => getRtype(Global.partnerReference));
                await Task.WhenAll(financesTask, debsTask, EmplacadoTask, LastReportTask, RtypReportTaks
                  ,AsistTask, blockTask, TicketsTask, AgreementsTask
                );

                Global.finances = financesTask.Result;
                Global.debts = debsTask.Result;
                Global.Emplacamiento = EmplacadoTask.Result;
                Global.LastReportDate = LastReportTask.Result;
                Global.priceReport = RtypReportTaks.Result;
                Global.blocks = blockTask.Result;
                Global.Reportes = AsistTask.Result;
                Global.tickets = TicketsTask.Result;
                Global.agreements = AgreementsTask.Result;
                return Global;
            }
            catch (Exception ex)
            {
                return null;
            }
        
        }




        protected async Task<RTYPE> getRtype(string partnerReference)
        {
            try
            {
                RTYPE rTYPE = new RTYPE();
                return await rTYPE.getRtype(partnerReference);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }
        protected financesOperator getFinances(_PartnerGlobalDTO global)
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
        protected List<caja_cargos_DTO> getDebs(string partnerReference)
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
        protected List<MENSAJE> getBlocks(string partnerReference)
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
        protected List<HistAsistencia> getAsist(_PartnerGlobalDTO global)
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
        protected List<Venta> getTickets(_PartnerGlobalDTO global)
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

        protected async Task<_PartnerGlobalDTO> getTickets(string partnerreference)
        {
            try
            {
                // Declaracion de variables.
                _PartnerGlobalDTO Global = new _PartnerGlobalDTO();
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
                    Global.status = Global.Operator.STATUS;
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
                    Global.status = "A";
                }
                var TicketsTask = Task.Run(() => getTickets(Global)); // Obtenemos los tickets de un usuario.
                await Task.WhenAll(TicketsTask);
                Global.tickets = TicketsTask.Result;
                return Global;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        protected List<Convenio> getAgreements(_PartnerGlobalDTO global)
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
        protected EmplacamientoDTO getEmplacamiento(string partnerReference)
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
        protected DateTime getLastReport(string partnerReference)
        {
            try
            {
                HistAsistencia asis = new HistAsistencia();
                return asis.getlastreport(partnerReference);
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }
    }
}
