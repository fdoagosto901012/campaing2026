using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class HistAsistencia : baseModel
    {
        public HistAsistencia() { }
        public string color { get; set; }

        public DateTime getlastreport(string Gafete) {
            try
            {
                HistAsistencia hist = db.HistAsistencias.Where(x => 
                    x.Gafete == Gafete
                ).OrderByDescending(x => x.FechaReporte).FirstOrDefault();
                return hist.FechaReporte;
            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }
            finally 
            { 
            
            }
        }

        public bool reportar(string GAFETE, string TIPO ,string TAXI,string TURNO,DateTime FECHAREPORTE,int CONCEPTO,string TICKET, DateTime FECHAOP,int USARVARINICIO,DateTime FECHAVARINICIO) {
            try
            {
                DateTime dateReport = FECHAREPORTE;
                for (int i = 1; i <= 30; i++) // REPORTAR 30 dias 
                {
                    try
                    {
                        string query = "AJR_REGISTRAASISTENCIA '" + GAFETE + "','" + TIPO + "', '" + TAXI + "','" + TURNO + "', '" + dateReport.ToString("yyyyMMdd") + "', " + CONCEPTO.ToString() + ", '" + TICKET + "','" + FECHAOP.ToString("yyyyMMdd") + "', " + USARVARINICIO + ", '" + dateReport.ToString("yyyyMMdd") + "';";
                        var a = this.db.Database.ExecuteSqlCommand(query);
                        this.db.SaveChanges();
                        dateReport = dateReport.AddDays(1);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        throw;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public bool reportedays(int days, string reference, string turno, int concepto, string ticket , DateTime FechaOp, string taxi = "000") {
            try
            {
                HistAsistencia hist = new HistAsistencia();
                DateTime lastreport = this.getlastreport(reference);
                List<HistAsistencia> historico = new List<HistAsistencia>();
                if (lastreport != null)
                {
                    var dateReport = lastreport.AddDays(1);
                    for (int i = 0; i < days; i++)
                    {
                        string query = "AJR_REGISTRAASISTENCIA '" + reference + "','" + (reference.ToUpper().Contains("OP-") ? "OP" : "E") + "', '" + taxi + "','" + turno + "', '" + dateReport.ToString("yyyyMMdd") + "', " + concepto.ToString() + ", '" + ticket + "','" + FechaOp.ToString("yyyyMMdd") + "', " + 1 + ", '" + dateReport.ToString("yyyyMMdd") + "';";
                        var a = this.db.Database.ExecuteSqlCommand(query);
                        dateReport = dateReport.AddDays(1);
                    }
                }
                return true;
            }

            catch (ValidationException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally { }
        }
        
    }
}
