using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace radiotaxi.Model
{
    public class baseModel
    {

        public const string success = "success";
        public const string error = "error";

        public string Message { get; set; }

        public string StatusAction { get; set; }
        public string MessageAction { get; set; }

        [JsonIgnore]
        public radiotaxiEntities db { get; set; }

        [JsonIgnore]
        public Venta01Entities dbv { get; set; }

        [JsonIgnore]
        public BitacorasEntities dbb { get; set; }

        private Random random = new Random();
        private const string _salt = "PVI04893WCd98237Fqwh";
        public string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //Get the encrypted password. Used to compare the provided password with the one storaged in the bd.
        public string Hash(string valueToHash)
        {
            var computedHash = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(valueToHash + _salt));
            return Convert.ToBase64String(computedHash);
        }

        public bool HasColumn(DbDataReader Reader, string ColumnName)
        {
            try
            {
               if (Reader[ColumnName] != null) {
                Console.WriteLine("");
               }
               return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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

        public baseModel() {
            this.db = new radiotaxiEntities();
            this.db.Configuration.LazyLoadingEnabled = false;
            this.db.Configuration.ProxyCreationEnabled = false;

            this.dbb = new BitacorasEntities();
            this.dbb.Configuration.LazyLoadingEnabled = false;
            this.dbb.Configuration.ProxyCreationEnabled = false;

            this.dbv = new Venta01Entities();
            this.dbv.Configuration.LazyLoadingEnabled = false;
            this.dbv.Configuration.ProxyCreationEnabled = false;

        }
    }
}