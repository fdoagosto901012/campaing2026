using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class MENSAJE : baseModel
    {

        public List<MENSAJE> bloqueos = new List<MENSAJE>();
        public List<desbloqueo> desbloqueos = new List<desbloqueo>();

        public MENSAJE() { }


        public List<MENSAJE> getGafet(string gafet)
        {
            try
            {
                using (var db = new radiotaxiEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    return db.MENSAJES.Where(x => x.CHOFER == gafet).ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<MENSAJE>();
            }
        }

        public List<desbloqueo> Desbloqueos(string chofer) {
            try
            {
                using (var db = new BitacorasEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    return db.desbloqueos.Where(X => X.chofer == chofer).ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<desbloqueo>();
            }
        }

        public void bloquearTaxi(string reference) { 
        
        }

        
        public List<SecretariasforMensaje> Secretarias() {
            try
            {
                return db.SecretariasforMensajes.ToList();
            }
            catch (Exception)
            {

                return new List<SecretariasforMensaje>();
            }
        }

        public List<MENSAJE> getEco(string eco)
        {
            try
            {
                using (var db = new radiotaxiEntities())
                {
                    return db.MENSAJES.Where(x => x.TAXI == eco || x.CHOFER == eco).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<desbloqueo> getDesbloqueosEco(string eco)
        {
            return this.dbb.desbloqueos.Where(x => x.taxi == eco).ToList();
        }

        public void bloquearSoc(string reference, string message, string username, string role, string detener, string id_secretaria)
        {
            try
            {
                Partner @operator = new Partner().get(reference); // Obtenemos al operador
                string query = @"SELECT CHOFER AS NUMERO,NOMBRE+ ' ' + ISNULL(APELLIDOS,'') AS NOMBRE,STATUS FROM CHOF_DETALLE WHERE CHOFER='" + reference + @"'\r\n"; // ESTO ES PARA FINTEAR JAJAJA Y TE CONFUNDA MAS, JAJAJA ATTE: FERNANDO A.C
                // Obtenemos el consecutivo.
                query = "SELECT DOCUMENTO, VALOR FROM CONSECUTIVOS WHERE DOCUMENTO='FM1000'"; // Obtenemos el consecutivo.
                Consecutivo consecutivo = db.Database.SqlQuery<Consecutivo>(query).FirstOrDefault();
                decimal actual = consecutivo.Valor;
                int siguiente = (int)(consecutivo.Valor + 1);
                query = "exec sp_executesql N'UPDATE \"SINDQR\"..\"CONSECUTIVOS\" SET \"VALOR\"=@P1 WHERE \"DOCUMENTO\"=@P2 AND \"VALOR\"=@P3',N'@P1 money,@P2 varchar(8000),@P3 money',$" + siguiente + @",'FM1000',$" + actual;
                this.db.Database.ExecuteSqlCommand(query);
                query = "EXECUTE AJR_REGISTRAMENSAJES 1,'0','" + reference + "','E','" + message + "','" + (DateTime.Now).ToString("yyyyMMdd") + "'," + detener + ",'" + id_secretaria + "','" + role + "','FM1000-" + siguiente + "','" + username + "','" + role + "'";
                this.db.Database.ExecuteSqlCommand(query); // Insertar el mensje 
                Console.WriteLine("Se agrego el mensaje.");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void bloquearTaxi(string reference, string message, string username, string role, string detener, string id_secretaria)
        {
            try
            {
                Partner @operator = new Partner().get(reference); // Obtenemos al operador
                string query = @"SELECT CHOFER AS NUMERO,NOMBRE+ ' ' + ISNULL(APELLIDOS,'') AS NOMBRE,STATUS FROM CHOF_DETALLE WHERE CHOFER='" + reference + @"'\r\n"; // ESTO ES PARA FINTEAR JAJAJA Y TE CONFUNDA MAS, JAJAJA ATTE: FERNANDO A.C
                // Obtenemos el consecutivo.
                query = "SELECT DOCUMENTO, VALOR FROM CONSECUTIVOS WHERE DOCUMENTO='FM1000'"; // Obtenemos el consecutivo.
                Consecutivo consecutivo = db.Database.SqlQuery<Consecutivo>(query).FirstOrDefault();
                decimal actual = consecutivo.Valor;
                int siguiente = (int)(consecutivo.Valor + 1);
                query = "exec sp_executesql N'UPDATE \"SINDQR\"..\"CONSECUTIVOS\" SET \"VALOR\"=@P1 WHERE \"DOCUMENTO\"=@P2 AND \"VALOR\"=@P3',N'@P1 money,@P2 varchar(8000),@P3 money',$" + siguiente + @",'FM1000',$" + actual;
                this.db.Database.ExecuteSqlCommand(query);
                query = "EXECUTE AJR_REGISTRAMENSAJES 1,'" + reference + "','0','E','" + message + "','" + (DateTime.Now).ToString("yyyyMMdd") + "'," + detener + ",'" + id_secretaria + "','" + role + "','FM1000-" + siguiente + "','" + username + "','" + role + "'";
                this.db.Database.ExecuteSqlCommand(query); // Insertar el mensje 
                Console.WriteLine("Se agrego el mensaje.");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void bloquearOperador(string reference, string message, string username, string role, string detener, string id_secretaria)
        {
            try
            {
                Operator @operator = new Operator().get(reference); // Obtenemos al operador
                string query = @"SELECT CHOFER AS NUMERO,NOMBRE+ ' ' + ISNULL(APELLIDOS,'') AS NOMBRE,STATUS FROM CHOF_DETALLE WHERE CHOFER='" + reference + @"'\r\n";
                // Obtenemos el consecutivo.
                query = "SELECT DOCUMENTO, VALOR FROM CONSECUTIVOS WHERE DOCUMENTO='FM1000'"; // Obtenemos el consecutivo.
                Consecutivo consecutivo = db.Database.SqlQuery<Consecutivo>(query).FirstOrDefault();
                decimal actual = consecutivo.Valor;
                int siguiente = (int)(consecutivo.Valor + 1);
                query = "exec sp_executesql N'UPDATE \"SINDQR\"..\"CONSECUTIVOS\" SET \"VALOR\"=@P1 WHERE \"DOCUMENTO\"=@P2 AND \"VALOR\"=@P3',N'@P1 money,@P2 varchar(8000),@P3 money',$" + siguiente + @",'FM1000',$" + actual;
                this.db.Database.ExecuteSqlCommand(query);
                query = "EXECUTE AJR_REGISTRAMENSAJES 1,'0','" + reference + "','OP','" + message + "','" + (DateTime.Now).ToString("yyyyMMdd") + "'," + detener + ",'" + id_secretaria + "','" + role + "','FM1000-" + siguiente + "','" + username + "','" + role + "'";
                this.db.Database.ExecuteSqlCommand(query); // Insertar el mensje 
                Console.WriteLine("Se agrego el mensaje.");
            }
            catch (Exception)
            {
                throw;
            }
        }





        public bool DesbloquearOP(string folio, string reference, string username, string secretaria)
        {
            try
            {
                MENSAJE message = db.MENSAJES.Where(x => x.FOLIO == folio).FirstOrDefault();
                if (message != null && message.SECRETARIA.ToUpper() == secretaria.ToUpper())
                {
                    string query = @"EXECUTE AJR_DESBLOQUEO '0','" + reference + "','OP','" + folio + "','2',1,'" + username + "','" + secretaria + "'";
                    this.db.Database.ExecuteSqlCommand(query); // Insertar el mensje
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
