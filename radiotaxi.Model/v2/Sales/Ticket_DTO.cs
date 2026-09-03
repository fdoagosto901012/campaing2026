using radiotaxi.Model.v2.Openpay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class Ticket_DTO : baseModel
    {
        public string Id_Op { get; set; }
        public string Id_Cliente { get; set; }
        public string Id_Vendedor { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
        public string Id_Usuario { get; set; }
        public DateTime FechaOP { get; set; }
        public DateTime HoraOp { get; set; }

        /***************************/

        public string Id_Producto { get; set; }
        public string Id_Proveedor { get; set; }
        public int Cant { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioConiva { get; set; }
        public decimal Importe { get; set; }
        
        public List<Ticket_DTO> get(string ticket)
        {
            try
            {
                string query = @"
                DECLARE @TICKET AS varchar(15)
                SET @TICKET='" + ticket + @"'
                SELECT
				       v.Id_Vendedor
				       ,v.Id_Cliente
					  ,vd.[Id_Op]
                      ,[Id_Producto]
                      ,[Id_Proveedor]
                      ,[Cant]
                      ,[Descripcion]
                      ,[PrecioConiva]
	                  ,vd.[Importe]
                      ,v.FechaOp
					  ,v.HoraOp
                  FROM [dbo].[VentasDetalle]   as vd
				  left join Ventas as v on v.Id_Op = vd.Id_Op
				  WHERE vd.ID_OP = @TICKET";
                List<Ticket_DTO> _tickets = this.db.Database.SqlQuery<Ticket_DTO>(query).ToList();
                return _tickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Ticket_DTO>();
            }
        }

        public Venta getResume(string ticket)
        {
            try
            {
                string query = @"
                DECLARE @TICKET AS varchar(15)
                SET @TICKET='" + ticket + @"'
                SELECT
				       v.Id_Vendedor
				       ,v.Id_Cliente
					  ,vd.[Id_Op]
                      ,[Id_Producto]
                      ,[Id_Proveedor]
                      ,[Cant]
                      ,[Descripcion]
                      ,[PrecioConiva]
	                  ,vd.[Importe]
                      ,v.FechaOp
					  ,v.HoraOp
                  FROM [dbo].[VentasDetalle]   as vd
				  left join Ventas as v on v.Id_Op = vd.Id_Op
				  WHERE vd.ID_OP = @TICKET";
                return this.db.Ventas.Where(x => x.Id_Op == ticket).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
