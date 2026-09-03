using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using radiotaxi.Model.v2;
using radiotaxi.Model.v2.finance;

namespace radiotaxi.Model
{
    public partial class C_permissions : baseModel
    {

        public int days { get; set; }
        public string motivo { get; set; }
        public string descripcion { get; set; }

        public DateTime start { get; set; }
        public DateTime end { get; set; }

        public C_permissions() { }

        // llama al mismo objeto.
        public bool save(){
            try
            {
                Inventario inventario = new Inventario()
                {
                    Id_Tienda = 1,
                    Id_Producto = "140." + this.RandomString(3) + "." + this.RandomString(3),
                    Descripcion = "PERMISO EXPRESS (" + start.ToString("dd/MM/yyyy") + " - " + end.ToString("dd/MM/yyyy") + ")",
                    Tipo = "PRO",
                    Id_Proveedor = this.gafet,
                    Id_Depto = "25",
                    Id_Familia = "140",
                    Id_Subfamilia = "1",
                    Id_Logo = "",
                    Consignado = false,
                    TipoComision = 0,
                    UnidadEnt = "PZA",
                    UnidadSal = "PZA",
                    Factor = 0,
                    Exist = 1,
                    TiempoSurtido = 0,
                    Minimo = 0,
                    Maximo = 0,
                    FechaOp = DateTime.Now,
                    TasaIva = 16,
                    TasaIvaCompra = 16,
                    CostoPromedio = 900,
                    CostoUltimo = 900,
                    Precio = 900,
                    PrecioConIva = 900,
                    Descuento = 0,
                    Id_Usuario = "10-1",
                    Color = "",
                    Status = "ACTIVO"
                };

                inventario = this.db.Inventarios.Add(inventario); // INSERTE EL COBRO EN EL INVENTARIO.
                this.db.SaveChanges();
                // Optenemos el ID OPERACION
                this.control = inventario.Control;// OBTENEMOS EL IDENTIFICADOR DE CONTROL DEL CARGO PARA BUSCARLO MAS RAPIDO.
                this.dateStart = start;
                this.dateEnd = end; // LOS PERMISOS SON SEMANALES.
                this.descripcion = "PERMISO EXPRESS (" + start.ToString("dd/MM/yyyy") + " - " + end.ToString("dd/MM/yyyy") + ")";
                this.authorizedBy = "AutoSis";
                this.db.C_permissions.Add(this);// Agregamos el objeto a la base de datos.
                this.db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // llama al mismo objeto desde el update.
        public bool update() {
            try
            {
                this.db.Entry(this).State = EntityState.Modified;
                this.db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool delete(int id) {
            try
            {
                C_permissions permissions = this.db.C_permissions.Find(id);
                permissions.active = false;
                this.db.Entry(permissions).State = EntityState.Modified;
                this.db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<C_permissionsDTO> get(string gafet) {
            try
            {
                using (var db = new radiotaxiEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    string query = @"select p.id, p.gafet, p.taxi, p.[control], p.dateStart, p.dateEnd, id_user, i.FechaOp, i.FechaOp, i.Exist, i.Descripcion from _permissions as p 
                                 left join inventario as i on p.[control] = i.[Control] Where p.gafet = '" + gafet + @"';";
                    List<C_permissionsDTO> _Permission = db.Database.SqlQuery<C_permissionsDTO>(query).ToList();
                    return _Permission;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public C_permissions get(int id)
        {
            try
            {
                return this.db.C_permissions
                    .Include(x => x.user)
                    .Where(x => x.id == id).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<C_permissions> getall()
        {
            try
            {
                return this.db.C_permissions.Where(x => x.active == true).Include(x => x.user).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void registraAsistencia(string gafete, string tipo, string taxi, string turno, DateTime fechareporte, string concepto, string ticket, DateTime fechaOp, Boolean uservarinicio, DateTime fechavarinicio)
        {
            try
            {
                string query = "AJR_REGISTRAASISTENCIA  gafete, tipo, taxi, turno, fechareporte, concepto, ticket, fechaOp, uservarinicio, fechavarinicio";
                var a = this.db.Database.ExecuteSqlCommand(query);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool calculate(){
            try
            {
                List<Chof_Detalle> express = new List<Chof_Detalle>();
                express = this.db.Chof_Detalle.Where(x => x.express == true).ToList();
                string query = @"
                                DECLARE @inicio Date;
                                DECLARE @final Date;

                                SET @final = CAST(DATEADD(DAY, 7, GETDATE()) as DATE);
                                SET @inicio = CAST(DATEADD(DAY, -40, GETDATE()) as DATE);

                                WITH UltimoReporte AS (
                                    select 
                                    Gafete,
                                    FechaReporte,
                                    u.id as userid,
                                    ROW_NUMBER() OVER (PARTITION BY Gafete ORDER BY FechaReporte DESC) AS rn
                                    from HistAsistencias
                                    left join Chof_Detalle as cd on cd.CHOFER = Gafete 
                                    left join [user] as u on u.partnerReference = Gafete
                                    where cd.express = 1
                                )
                                SELECT
                                    Gafete,
                                    FechaReporte,
                                    userid
                                FROM
                                    UltimoReporte as u
                                WHERE rn = 1 and userid is not null and
                                FechaReporte BETWEEN @inicio and @final 
                                order by FechaReporte desc;";
                List< OperatorLastReport > data = this.db.Database.SqlQuery<OperatorLastReport>(query).ToList();
                // Hacer el calculo de todos los operadores.
                foreach (var _operator in data){
                    try
                    {
                        // CREAMOS EL PERMISO EXPRESS PERO TENEMOS QUE ACTUALZAR AL FINAL PARA OBTENER EL ID_USER PARE RELACIONAR LOS PERMISOS CON EL OBJETO USER
                        C_permissions obj = new C_permissions()
                        {
                            active = true,
                            descripcion = "PERMISO EXPRESS",
                            days = 30,
                            gafet = _operator.Gafete.ToUpper(),
                            CreatedBy = "AutomaticoSistema",
                            dateCreated = DateTime.Now,
                            id_user = _operator.userid,
                            start = _operator.FechaReporte.AddDays(1),
                            end = _operator.FechaReporte.AddDays(30),
                        };
                        obj.save(); // Aqui adentro hacemos todo procesos.
                                    // Reportalo 30 dias que compete a los 900.
                        HistAsistencia histAsistencia = new HistAsistencia();
                        histAsistencia.reportar(_operator.Gafete, "OP", "000", "P", _operator.FechaReporte.AddDays(1), 1, "P" + obj.id.ToString(), _operator.FechaReporte.AddDays(1), 1, _operator.FechaReporte.AddDays(1));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool generate(string gafet) {
            try
            {
                List<Chof_Detalle> express = new List<Chof_Detalle>();
                express = this.db.Chof_Detalle.Where(x => x.express == true).ToList();
                string query = @"
                                WITH UltimoReporte AS (
	                                select 
	                                Gafete,
	                                FechaReporte,
	                                u.id as userid,
	                                ROW_NUMBER() OVER (PARTITION BY Gafete ORDER BY FechaReporte DESC) AS rn
	                                from HistAsistencias
	                                left join Chof_Detalle as cd on cd.CHOFER = Gafete 
	                                left join [user] as u on u.partnerReference = Gafete
	                                where cd.express = 1
                                )
                                SELECT
                                    Gafete,
                                    FechaReporte,
	                                userid
                                FROM
                                    UltimoReporte as u
                                WHERE rn = 1 and Gafete = '" + gafet + @"'
                                order by FechaReporte desc;";
                List<OperatorLastReport> data = this.db.Database.SqlQuery<OperatorLastReport>(query).ToList();
                // Hacer el calculo de todos los operadores.
                foreach (var _operator in data)
                {
                    // CREAMOS EL PERMISO EXPRESS PERO TENEMOS QUE ACTUALZAR AL FINAL PARA OBTENER EL ID_USER PARE RELACIONAR LOS PERMISOS CON EL OBJETO USER
                    C_permissions obj = new C_permissions()
                    {
                        active = true,
                        descripcion = "PERMISO EXPRESS",
                        days = 30,
                        gafet = _operator.Gafete.ToUpper(),
                        CreatedBy = "AutomaticoSistema",
                        dateCreated = DateTime.Now,
                        id_user = _operator.userid,
                        start = _operator.FechaReporte.AddDays(1),
                        end = _operator.FechaReporte.AddDays(30),
                    };
                    obj.save(); // Aqui adentro hacemos todo procesos.
                    // Reportalo 30 dias que compete a los 900.
                    HistAsistencia histAsistencia = new HistAsistencia();
                    histAsistencia.reportar(_operator.Gafete, "OP", "000", "P", _operator.FechaReporte.AddDays(1), 1, "P" + obj.id.ToString(), _operator.FechaReporte.AddDays(1), 1, _operator.FechaReporte.AddDays(1));
                    Console.WriteLine("...");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<C_permissionsDTO> vencidos() {
            try
            {
                string query = @"WITH UltimoReporte AS (
	                                select 
	                                Gafete,
	                                FechaReporte,
	                                u.id as userid,
	                                ticket,
	                                p.*,
	                                i.Exist,
	                                i.FechaOp,
	                                cd.CHOFER,
									u.firstName,
									u.lastNameF, 
									u.lastNameM,
	                                u.partnerReference,
									
	                                ROW_NUMBER() OVER (PARTITION BY Gafete ORDER BY FechaReporte DESC) AS rn
	                                from HistAsistencias as h
	                                left join Chof_Detalle as cd on cd.CHOFER = Gafete 
	                                left join [user] as u on u.partnerReference = Gafete
	                                left join _permissions as p on CONCAT('P',p.id) = h.Ticket
	                                left join Inventario as i on i.Control = p.[control]
	                                where cd.express = 1
                                )
                                select * from (
                                SELECT
                                    Gafete,
                                    CHOFER,
	                                partnerReference,
                                    FechaReporte,
	                                userid,
	                                ticket,
	                                [control],
	                                Exist,
	                                dateStart,
	                                dateEnd,
	                                taxi,
	                                CreatedBy,
	                                dateCreated,
	                                FechaOp,
									firstName,
									lastNameF, 
									lastNameM,
	                                rn
                                FROM
                                    UltimoReporte as u
                                WHERE
                                rn = 1) as c
                                where CAST(DATEADD(DAY, -5, dateEnd) as DATE) between CAST(DATEADD(DAY, -25, GETDATE()) as DATE) and CAST(DATEADD(DAY, 35, GETDATE()) as DATE)
                                order by FechaReporte desc;";
                List<C_permissionsDTO> data = this.db.Database.SqlQuery<C_permissionsDTO>(query).ToList();
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<C_permissionsDTO> vencidos(string chofer)
        {
            try
            {
                string query = @"WITH UltimoReporte AS (
	                                select 
	                                Gafete,
	                                FechaReporte,
	                                u.id as userid,
	                                ticket,
	                                p.*,
	                                i.Exist,
	                                i.FechaOp,
	                                cd.CHOFER,
									u.firstName,
									u.lastNameF, 
									u.lastNameM,
	                                u.partnerReference,
									
	                                ROW_NUMBER() OVER (PARTITION BY Gafete ORDER BY FechaReporte DESC) AS rn
	                                from HistAsistencias as h
	                                left join Chof_Detalle as cd on cd.CHOFER = Gafete 
	                                left join [user] as u on u.partnerReference = Gafete
	                                left join _permissions as p on CONCAT('P',p.id) = h.Ticket
	                                left join Inventario as i on i.Control = p.[control]
	                                where cd.express = 1
                                )
                                select * from (
                                SELECT
                                    Gafete,
                                    CHOFER,
	                                partnerReference,
                                    FechaReporte,
	                                userid,
	                                ticket,
	                                [control],
	                                Exist,
	                                dateStart,
	                                dateEnd,
	                                taxi,
	                                CreatedBy,
	                                dateCreated,
	                                FechaOp,
									firstName,
									lastNameF, 
									lastNameM,
	                                rn
                                FROM
                                    UltimoReporte as u
                                WHERE
                                rn = 1) as c
                                where CAST(DATEADD(DAY, -5, dateEnd) as DATE) between CAST(DATEADD(DAY, -25, GETDATE()) as DATE) and CAST(DATEADD(DAY, 35, GETDATE()) as DATE)
                                order by FechaReporte desc;";
                List<C_permissionsDTO> data = this.db.Database.SqlQuery<C_permissionsDTO>(query).ToList();
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


    }
}
