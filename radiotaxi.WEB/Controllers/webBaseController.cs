using Newtonsoft.Json;
using radiotaxi.Model;
using radiotaxi.Model.v2.Operator;
using radiotaxi.Model.v2.Partner;
using radiotaxi.WEB.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace radiotaxi.WEB.Controllers
{
    public class webBaseController : Controller
    {
        // GET: Client
        protected radiotaxiEntities db = new radiotaxiEntities();
        protected Venta01Entities dbventas = new Venta01Entities();
        private static String ToHex(System.Drawing.Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";
        public decimal maxdeb = 0;
        protected string FormatString(string myString, string format)
        {
            myString = Regex.Replace(myString, @"[^\d]", "");
            const char number = '#';
            const char character = '%';
            StringBuilder sb = new StringBuilder();
            if (myString.Length != 10) {
                myString = "0000000000";
            };
            int i = 0;
            foreach (char c in format)
            {
                switch (c)
                {
                    case number:
                        if (char.IsDigit(myString[i]))
                        {
                            sb.Append(myString[i]);
                            i++;
                        }
                        else
                        {
                            throw new Exception("Format string doesn't match input string");
                        }
                        break;
                    case character:
                        if (!char.IsDigit(myString[i]))
                        {
                            sb.Append(myString[i]);
                            i++;
                        }
                        else
                        {
                            throw new Exception("Format string doesn't match input string");
                        }
                        break;
                    default:
                        sb.Append(c);
                        break;
                }

            }
            return sb.ToString();
        }


        protected Random rnd = new Random();
        public const string success = "success";
        public const string error = "error";
        public Dictionary<string, string> months = new Dictionary<string, string>();
        public webBaseController()
        {
            months.Add("Dec", "12");
            months.Add("Nov", "11");
            months.Add("Oct", "10");
            months.Add("Sep", "09");
            months.Add("Aug", "08");
            months.Add("Jul", "07");
            months.Add("Jun", "06");
            months.Add("May", "05");
            months.Add("Apr", "04");
            months.Add("Mar", "03");
            months.Add("Feb", "02");
            months.Add("Jan", "01");
        }

        private Random rand;
        private int maxColorIndex;

       

        protected string getRandColor()
        {
            Random rnd = new Random();
            string hexOutput = String.Format("{0:X}", rnd.Next(0, 0xFFFFFF));
            while (hexOutput.Length < 6)
                hexOutput = "0" + hexOutput;
            return "#" + hexOutput;
        }

        public userlog User
        {
            get
            {
                if (Session["_USER"] != null)
                {
                    return (userlog)Session["_USER"];
                }
                return null;
            }
            set
            {
                Session["_USER"] = value;
            }
        }

        public string email
        {
            get
            {
                if (Session["_EMAIL"] != null)
                {
                    return (string)Session["_EMAIL"];
                }
                return null;
            }
            set
            {
                Session["_EMAIL"] = value;
            }
        }

        public string taxi
        {
            get
            {
                if (Session["_taxi"] != null)
                {
                    return (string)Session["_taxi"];
                }
                return null;
            }
            set
            {
                Session["_taxi"] = value;
            }
        }

        public string reference
        {
            get
            {
                if (Session["_reference"] != null)
                {
                    return (string)Session["_reference"];
                }
                return null;
            }
            set
            {
                Session["_reference"] = value;
            }
        }

        public List<caja_cargos_DTO> charges
        {
            get
            {
                if (Session["_charges"] != null)
                {
                    return (List<caja_cargos_DTO>)Session["_charges"];
                }
                return null;
            }
            set
            {
                Session["_charges"] = value;
            }
        }

        public List<Emplacamiento> vins
        {
            get
            {
                if (Session["_vins"] != null)
                {
                    return (List<Emplacamiento>)Session["_vins"];
                }
                return null;
            }
            set
            {
                Session["_vins"] = value;
            }
        }

        public string UserName
        {
            get
            {
                if (Session["_USERNAME"] != null)
                {
                    return (string)Session["_USERNAME"];
                }
                return null;
            }
            set
            {
                Session["_USERNAME"] = value;
            }
        }

        public string IP
        {
            get
            {
                if (Session["_IP"] != null)
                {
                    return (string)Session["_IP"];
                }
                return null;
            }
            set
            {
                Session["_IP"] = value;
            }
        }

        public string Name
        {
            get
            {
                if (Session["name"] != null)
                {
                    return (string)Session["name"];
                }
                return null;
            }
            set
            {
                Session["name"] = value;
            }
        }

        public string Role
        {
            get
            {
                if (Session["role"] != null)
                {
                    return (string)Session["role"];
                }
                return null;
            }
            set
            {
                Session["role"] = value;
            }
        }


        public MessageDTO Message
        {
            get
            {
                if (Session["MessageDTO"] != null)
                {
                    return (MessageDTO)Session["MessageDTO"];
                }
                return null;
            }
            set
            {
                Session["MessageDTO"] = value;
            }
        }

        protected ClientPagination client_pagination;
        // Seccion de clientes
        public ClientSearchParametersDTO ClientSearchParameters
        {
            get
            {
                if (Session["ClientSearchParameters"] != null)
                {
                    return (ClientSearchParametersDTO)Session["ClientSearchParameters"];
                }
                return null;
            }
            set
            {
                Session["ClientSearchParameters"] = value;
            }
        }

        public OperatorSearchParametersDTO OperatorSearchParameters
        {
            get
            {
                if (Session["OperatorSearchParameters"] != null)
                {
                    return (OperatorSearchParametersDTO)Session["OperatorSearchParameters"];
                }
                return null;
            }
            set
            {
                Session["OperatorSearchParameters"] = value;
            }
        }

        public PartnerSearchParametersDTO PartnerSearchParameters
        {
            get
            {
                if (Session["PartnerSearchParameters"] != null)
                {
                    return (PartnerSearchParametersDTO)Session["PartnerSearchParameters"];
                }
                return null;
            }
            set
            {
                Session["PartnerSearchParameters"] = value;
            }
        }

        public CardSearchParametersDTO CardSearchParameters
        {
            get
            {
                if (Session["CardSearchParameters"] != null)
                {
                    return (CardSearchParametersDTO)Session["CardSearchParameters"];
                }
                return null;
            }
            set
            {
                Session["CardSearchParameters"] = value;
            }
        }

        public UserLogSearchParametersDTO UserLogSearchParameters
        {
            get
            {
                if (Session["UserLogSearchParametersDTO"] != null)
                {
                    return (UserLogSearchParametersDTO)Session["UserLogSearchParametersDTO"];
                }
                return null;
            }
            set
            {
                Session["UserLogSearchParametersDTO"] = value;
            }
        }

        


        public List<DateTime> ParseDates(string dates) {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime date;
            string[] Services = dates.Split('=');
            List<String> DaysWeek = new List<String>();
            List<DateTime> time = new List<DateTime>();
            foreach (string service in Services)
            {
                string[] dateAll = service.Split(' ');
                if (service != "Invalid Date")
                {
                    string dateString = dateAll[3] + "-" + this.months[dateAll[1]] + "-" + dateAll[2] + " " + dateAll[4];
                    date = DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    time.Add(date);
                    if (!DaysWeek.Exists(x => x.Equals(dateAll[0] + " " + dateAll[4])))
                    {
                        DaysWeek.Add(dateAll[0] + " " + dateAll[4]);
                    }
                }
            }
            return time;
        }

        // Helpers methods
        public string getQRTarjeton(int id) {
            //Obtenemos el ID de la asignacion.
            return null;
        }

        protected string serialize(Object a) {
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


        protected string serializeFlat(Object a) {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new FirstLevelContractResolver(),
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(a, Formatting.Indented, settings);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool permissions(string roles)
        {
            roles = roles.Replace(" ", "");
            string[] _roles = roles.Split(',');
            foreach (var role in _roles)
            {
                if (Session["role"].ToString() == role)
                {
                    return true;
                }
            }
            return false;
        }

        protected override JsonResult Json(object data, string contentType,
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


        public string GenerarCadenaAleatoria(int longitud)
        {
            const string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder resultado = new StringBuilder(longitud);
            Random random = new Random();

            for (int i = 0; i < longitud; i++)
            {
                int indice = random.Next(caracteres.Length);
                resultado.Append(caracteres[indice]);
            }

            return resultado.ToString();
        }



    }
}