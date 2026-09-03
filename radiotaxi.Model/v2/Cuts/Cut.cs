using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace radiotaxi.Model
{
    public  class Cut : baseModel
    {
        public decimal? CORTE_CAJA { get; set; } 
        public DateTime? FechaOp { get; set; }
        public string Id_Familia { get; set; }

        public DateTime date_id { get; set; }
        public short date_year { get; set; }
        public byte date_month { get; set; }
        public byte date_day { get; set; }
        public byte weekday_id { get; set; }
        public string weekday_nm { get; set; }
        public string month_nm { get; set; }
        public short day_of_year { get; set; }
        public byte quarter_id { get; set; }
        public DateTime first_day_of_month { get; set; }
        public DateTime last_day_of_month { get; set; }
        public DateTime start_dts { get; set; }
        public DateTime end_dts { get; set; }

        public CutResults get(DateTime start, DateTime end) {
            try
            {
                string query = "fdo_cuts_family '" + start.ToString("yyyyMMdd") + "', '" + end.ToString("yyyyMMdd") + "'";
                //List<CutResults> operators = this.db.Database.SqlQuery<CutResults>(query);

                CutResults domainEntity = new CutResults();
                var command = this.db.Database.Connection.CreateCommand();
                command.CommandText = "[dbo].[fdo_cuts_family]";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter("@start_dt_s", start.ToString("yyyyMMdd"));
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                command.Parameters.Add(param);

                SqlParameter param2 = new SqlParameter("@end_dt_s", end.ToString("yyyyMMdd"));
                param2.Direction = ParameterDirection.Input;
                param2.DbType = DbType.String;
                command.Parameters.Add(param2);

                try
                {
                    this.db.Database.Connection.Open();
                    var reader = command.ExecuteReader();

                    domainEntity.Cuts =
                    ((IObjectContextAdapter)this.db).ObjectContext.Translate<Cut>
                    (reader).ToList();
                    reader.NextResult();
                    domainEntity.days =
                    ((IObjectContextAdapter)this.db).ObjectContext.Translate<day>
                    (reader).ToList();
                    reader.NextResult();
                    domainEntity.Familias =
                    ((IObjectContextAdapter)this.db).ObjectContext.Translate<Familia>
                    (reader).ToList();

                    return domainEntity;
                }
                finally
                {
                    this.db.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                CutResults domainEntity = new CutResults();
                domainEntity.days = new List<day>();
                domainEntity.Cuts = new List<Cut>();
                domainEntity.Familias = new List<Familia>();
                return domainEntity;
            }
        }

        public void get(string start, string end)
        {

        }
    }
}
