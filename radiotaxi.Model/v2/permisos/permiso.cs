using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class permiso : baseModel
    {
        public permiso() { }

        public string Gafete { get; set; }
        public DateTime FechaOp {  get; set; }
        public string Id_Usuario { get; set; }

        public string Nombre { get; set; }

        public int cantidad { get; set; }
        public string Ticket { get; set; }

        public List<permiso> getall() {
            try
            {
                string query = @"
                    select Gafete, ha.Ticket,ha.FechaOp, u.Id_Usuario, u.Nombre, count(-1) as cantidad from HistAsistencias as ha 
                    left join Venta01.dbo.Usuarios as u on ha.Ticket like CONCAT('%',u.Id_Usuario)   
                    where UPPER(Gafete) like 'OP-%' and ha.Ticket like 'PE-%' and ha.FechaOp >  DATEADD(year, -1, GETDATE())
                    group by Gafete, ha.FechaOp, Ticket,u.Id_Usuario, u.Nombre
                    order by ha.FechaOp desc;
                ";
                List<permiso> response = this.db.Database.SqlQuery<permiso>(query).ToList();

                query = @"
                    select Gafete, ha.Ticket, ha.FechaOp, u.Id_Usuario, u.Nombre, count(-1) as cantidad from HistAsistencias as ha 
                    left join Venta01.dbo.Usuarios as u on ha.Ticket like CONCAT('%',u.Id_Usuario)   
                    where UPPER(Gafete) like 'OP-%' and ha.Ticket like 'IN-%' and ha.FechaOp >  DATEADD(year, -1, GETDATE())
                    group by Gafete, ha.FechaOp, Ticket,u.Id_Usuario, u.Nombre
                    order by ha.FechaOp desc;
                ";
                List<permiso> response2 = this.db.Database.SqlQuery<permiso>(query).ToList();
                response.AddRange(response2);
                List<permiso> a = response.OrderByDescending(x => x.FechaOp).ToList();
                return a;
            }
            catch (Exception ex)
            {
                return new List<permiso>();
            }
        
        }
    }
}
