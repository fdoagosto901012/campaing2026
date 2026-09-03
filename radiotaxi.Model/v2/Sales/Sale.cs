using radiotaxi.Model.v2.Sales;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model   
{
    public class Sale : baseModel
    {
        public string taxi { get; set; }
        public string reference { get; set; }

        public string chashcashier { get; set; }

        public List<caja_cargos_DTO> pays { get; set; }
        private HistAsistencia _HistAsistencia { get; set;} 

        public Sale(string _chashcashier) { 
            this.chashcashier = _chashcashier;
        }



        public List<caja_cargos_DTO> debs(string eco, string reference)
        {
            try
            {
                RTYPE rTYPE = new RTYPE();
                List<caja_cargos_DTO> Cargos = this.loadDebs(eco, reference);
                for (int i = 0; i < Cargos.Count; i++)
                {
                    Cargos[i].PAGA = Cargos[i].DEBE;
                    if (Cargos[i].CLAVE == "0")
                    {
                        Cargos[i].MULTIPLICADOR = 1;
                        // recalculamos el valor del precio.
                        Cargos[i].PRECIO = rTYPE.getdouble(Cargos[i].PRECIOFIJO);

                    }
                }
                return Cargos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<caja_cargos_DTO> loadDebs(string taxi, string reference)
        {
            try
            {
                string queryTaxi = "SELECT id_proveedor,proveedor,operador FROM proveedores WHERE Id_proveedor='" + taxi + "';"; // Obtenemos el nombre del proveedor. ( BUSCA INFORMACION DEL TAXI)
                caja_proveedor_DTO proveedoresSoc = db.Database.SqlQuery<caja_proveedor_DTO>(queryTaxi).ToList().FirstOrDefault();

                string queryOperador = "SELECT p.id_proveedor,p.proveedor,isnull(p.operador,'O') as operador,isnull(cd.status,'A') as status FROM proveedores p left join chof_Detalle cd on cd.chofer=p.id_proveedor and substring(p.id_proveedor,1,2)='OP' WHERE p.Id_proveedor='" + reference + "'";
                caja_proveedor_DTO proveedoresOp = db.Database.SqlQuery<caja_proveedor_DTO>(queryOperador).ToList().FirstOrDefault();

                string queryReporte = "SELECT MAX(FECHAREPORTE) AS FECHAREPORTE FROM HISTASISTENCIAS WHERE GAFETE='" + reference + "'";
                caja_reporte_DTO Reporte = db.Database.SqlQuery<caja_reporte_DTO>(queryReporte).ToList().FirstOrDefault();

                string queryReporteT = "SELECT MAX(TURNO) AS TURNO FROM HISTASISTENCIAS WHERE GAFETE = '" + reference + "' AND FECHAREPORTE = '" + Reporte.FECHAREPORTE.ToString("yyyyMMdd") + "'";
                Reporte.TURNO = db.Database.SqlQuery<caja_reporte_DTO>(queryReporteT).ToList().FirstOrDefault().TURNO;
                
                // CARGAR TODOS LOS CARGOS QUE SE ENCUENTRAN EN INVENTARIO QUE SE PUEDEN AGRUPAR!
                string queryCargos = @"
                        select 
                        --I.Id_Producto AS PRODUCTO,
                        I.ID_FAMILIA AS CLAVE,
                        F.FAMILIA AS CARGO,
                        I.ID_SUBFAMILIA AS IDA,
                        SF.SUBFAMILIA AS A,
                        SUM(I.EXIST) AS DEBE,
                        0 AS PAGA,
                        I.PrecioConIva as PRECIO,  
                        I.PrecioConIva as PRECIOFIJO, 
                        C.Id_Op as CONVENIO

                        from Inventario as I
                        INNER JOIN FAMILIA F ON F.ID_FAMILIA=I.ID_FAMILIA 
                        INNER JOIN SubFamilia SF ON SF.Id_Subfamilia = I.Id_Subfamilia
                        LEFT JOIN ConveniosDetalle CD ON CD.Id_Producto = I.Id_Producto
                        LEFT JOIN Convenios C ON C.Id_Op = CD.Id_Op
                        WHERE (I.ID_FAMILIA<>'36') AND (I.ID_PROVEEDOR='" + taxi + @"' AND I.EXIST > 0) OR (I.ID_PROVEEDOR='" + reference + @"' AND I.EXIST > 0  AND I.ID_FAMILIA NOT IN ('11'))
                        GROUP BY I.ID_FAMILIA,F.FAMILIA,I.ID_SUBFAMILIA, SF.SUBFAMILIA, I.PrecioConIva, C.Id_Op;
                ";
                List<caja_cargos_DTO> _Cargos = db.Database.SqlQuery<caja_cargos_DTO>(queryCargos).ToList();

                // CARGAR TODOS LOS CARGOS QUE SE ENCUENTRAN EN INVENTARIO QUE NO SE PUEDEN AGRUPAR!
                string _queryCargos = @"
                        select 
                        I.Id_Producto AS PRODUCTO,
                        I.ID_FAMILIA AS CLAVE,
                        F.FAMILIA AS CARGO,
                        I.ID_SUBFAMILIA AS IDA,
                        SF.SUBFAMILIA AS A,
                        I.EXIST AS DEBE,
                        0 AS PAGA,
                        I.PrecioConIva as PRECIO,  
                        I.PrecioConIva as PRECIOFIJO, 
                        C.Id_Op as CONVENIO
                        from Inventario as I
                        INNER JOIN FAMILIA F ON F.ID_FAMILIA=I.ID_FAMILIA 
                        INNER JOIN SubFamilia SF ON SF.Id_Subfamilia = I.Id_Subfamilia
                        LEFT JOIN ConveniosDetalle CD ON CD.Id_Producto = I.Id_Producto
                        LEFT JOIN Convenios C ON C.Id_Op = CD.Id_Op
                        WHERE (I.ID_FAMILIA<>'36') AND (I.ID_PROVEEDOR='" + taxi + @"' AND I.EXIST > 0) OR (I.ID_PROVEEDOR='" + reference + @"' AND I.EXIST > 0  AND I.ID_FAMILIA NOT IN ('11'));
                ";
                List<caja_cargos_DTO> __Cargos = db.Database.SqlQuery<caja_cargos_DTO>(_queryCargos).ToList();

                List<caja_cargos_DTO> Cargos = new List<caja_cargos_DTO>();
                foreach (var _cargo in _Cargos)
                {
                    if (_cargo.CONVENIO == null)
                    {
                        Cargos.Add(_cargo);
                    }
                    else if ((Cargos.Where(x => x.CONVENIO == _cargo.CONVENIO).FirstOrDefault() == null)) { // SI ES NULO SIGNIFICA QUE NO HEMOS GARDADO ESE CONVENIO AL FINAL.
                        foreach (var __cargo in __Cargos.Where(x => x.CONVENIO == _cargo.CONVENIO))
                        {
                            Cargos.Add(__cargo);
                        }
                    }
                }
                return Cargos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public caja_debs_reporte_DTO loadDebsProduct(string taxi, string reference)
        {
            try
            {
                string queryTaxi = "SELECT id_proveedor,proveedor,operador FROM proveedores WHERE Id_proveedor='" + taxi + "';"; // Obtenemos el nombre del proveedor. ( BUSCA INFORMACION DEL TAXI)
                caja_proveedor_DTO proveedoresSoc = db.Database.SqlQuery<caja_proveedor_DTO>(queryTaxi).ToList().FirstOrDefault();
                string queryOperador = "SELECT p.id_proveedor,p.proveedor,isnull(p.operador,'O') as operador,isnull(cd.status,'A') as status FROM proveedores p left join chof_Detalle cd on cd.chofer=p.id_proveedor and substring(p.id_proveedor,1,2)='OP' WHERE p.Id_proveedor='" + reference + "'";
                caja_proveedor_DTO proveedoresOp = db.Database.SqlQuery<caja_proveedor_DTO>(queryOperador).ToList().FirstOrDefault();
                string queryReporte = "SELECT MAX(FECHAREPORTE) AS FECHAREPORTE FROM HISTASISTENCIAS WHERE GAFETE='" + reference + "'";
                caja_reporte_DTO Reporte = db.Database.SqlQuery<caja_reporte_DTO>(queryReporte).ToList().FirstOrDefault();
                string queryReporteT = "SELECT MAX(TURNO) AS TURNO FROM HISTASISTENCIAS WHERE GAFETE = '" + reference + "' AND FECHAREPORTE = '" + Reporte.FECHAREPORTE.ToString("yyyyMMdd") + "'";
                Reporte.TURNO = db.Database.SqlQuery<caja_reporte_DTO>(queryReporteT).ToList().FirstOrDefault().TURNO;
                // CARGAR TODOS LOS CARGOS QUE SE ENCUENTRAN EN INVENTARIO
                string queryCargos = @"
                        SELECT I.Id_Producto AS PRODUCTO, I.Id_Producto, I.FechaOp, I.ID_FAMILIA AS CLAVE,F.FAMILIA AS CARGO,I.ID_SUBFAMILIA AS IDA, S.SUBFAMILIA AS A, SUM(I.EXIST)AS DEBE, 0 AS PAGA, I.PrecioConIva as PRECIO, C.Id_Op as CONVENIO
                        FROM INVENTARIO I 
                        INNER JOIN FAMILIA F ON F.ID_FAMILIA=I.ID_FAMILIA 
                        INNER JOIN SUBFAMILIA S ON S.ID_SUBFAMILIA=I.ID_SUBFAMILIA
                        LEFT JOIN ConveniosDetalle CD ON CD.Id_Producto = I.Id_Producto
                        LEFT JOIN Convenios C ON C.Id_Op = CD.Id_Op
                        WHERE (I.ID_FAMILIA<>'36') AND (I.ID_PROVEEDOR='" + taxi + @"' AND I.EXIST >0) OR (I.ID_PROVEEDOR='" + reference + @"' AND I.EXIST >0  AND I.ID_FAMILIA NOT IN ('11')) 
                        GROUP BY I.ID_FAMILIA,F.FAMILIA,I.ID_SUBFAMILIA,S.SUBFAMILIA, I.PrecioConIva, I.Id_Producto, I.FechaOp, C.Id_Op
                        ORDER BY I.ID_SUBFAMILIA,I.ID_FAMILIA";
                Debug.WriteLine(queryCargos);
                List<caja_cargos_DTO> Cargos = db.Database.SqlQuery<caja_cargos_DTO>(queryCargos).ToList();
                caja_debs_reporte_DTO data = new caja_debs_reporte_DTO();
                data.debs = Cargos;
                data.report = Reporte;
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Ticket_Detail_DTO pay(string username, string IP, string taxi, string reference, List<caja_cargos_DTO> pays, caja_debs_reporte_DTO data)
        {
            try
            {
                // Reporte OPERADOR
                string queryReporte = "SELECT MAX(FECHAREPORTE) AS FECHAREPORTE FROM HISTASISTENCIAS WHERE GAFETE='" + reference + "'";
                caja_reporte_DTO Reporte = db.Database.SqlQuery<caja_reporte_DTO>(queryReporte).ToList().FirstOrDefault();
                string queryReporteT = "SELECT MAX(TURNO) AS TURNO FROM HISTASISTENCIAS WHERE GAFETE = '" + reference + "' AND FECHAREPORTE = '" + Reporte.FECHAREPORTE.ToString("yyyyMMdd") + "'";
                Reporte.TURNO = db.Database.SqlQuery<caja_reporte_DTO>(queryReporteT).ToList().FirstOrDefault().TURNO;

                // Reporte SOC
                string queryReporteSOC = "SELECT MAX(FECHAREPORTE) AS FECHAREPORTE FROM HISTASISTENCIAS WHERE GAFETE='" + taxi + "'";
                caja_reporte_DTO ReporteSOC = db.Database.SqlQuery<caja_reporte_DTO>(queryReporteSOC).ToList().FirstOrDefault();
                string queryReporteTSOC = "SELECT MAX(TURNO) AS TURNO FROM HISTASISTENCIAS WHERE GAFETE = '" + taxi + "' AND FECHAREPORTE = '" + ReporteSOC.FECHAREPORTE.ToString("yyyyMMdd") + "'";
                Reporte.TURNO = db.Database.SqlQuery<caja_reporte_DTO>(queryReporteT).ToList().FirstOrDefault().TURNO;

                // Obtenemos el numero de ticket.
                string queryConsecutivo = @"SELECT DOCUMENTO, VALOR FROM  Venta01.dbo.CONSECUTIVOS WHERE DOCUMENTO='Venta'";
                Consecutivo_DTO consecutivo = db.Database.SqlQuery<Consecutivo_DTO>(queryConsecutivo).ToList().FirstOrDefault();
                Decimal NumTicekt = consecutivo.VALOR; // Obtenemos el numero de ticket
                Decimal NumConsec = NumTicekt + 1; // Actualizamos el siguiente numero de ticket.
                string query = @"exec sp_executesql N'UPDATE ""VENTA01""..""CONSECUTIVOS"" SET ""VALOR""=@P1 WHERE ""DOCUMENTO""=@P2 AND ""VALOR""=@P3',N'@P1 money,@P2 varchar(8000),@P3 money',$" + NumConsec.ToString() + @",'Venta',$" + consecutivo.VALOR.ToString();
                var a = this.db.Database.ExecuteSqlCommand(query);
                string date = DateTime.Now.ToString("yyyyMMdd");
                string time = DateTime.Now.ToString("HH:mm");
                string lastReportDate = data.report.FECHAREPORTE.ToString("yyyyMMdd");
                decimal totalAmount = 0;

                // Registramos cada uno de los conceptos
                foreach (var pay in pays.Where(x => x.PAGA > 0))
                {
                    List<caja_cargos_DTO> productSS = new List<caja_cargos_DTO>();
                    if (pay.CONVENIO != null && pay.PRODUCTO != null)
                    {
                        // Obtenemos las deudas que se van a pagar directamente desde la base de datos para que no tengan modificaciones en el Front
                        productSS = data.debs
                            .Where(x => x.CLAVE == pay.CLAVE && x.A == pay.A && x.PRODUCTO == pay.PRODUCTO)
                            .OrderBy(x => x.FechaOp)
                            .Take(pay.PAGA)
                            .ToList<caja_cargos_DTO>();
                    }
                    else {

                        // Obtenemos las deudas que se van a pagar directamente desde la base de datos para que no tengan modificaciones en el Front
                        productSS = data.debs
                            .Where(x => x.CLAVE == pay.CLAVE && x.A == pay.A)
                            .OrderBy(x => x.FechaOp)
                            .Take(pay.PAGA)
                            .ToList<caja_cargos_DTO>();
                    }

                    foreach (var product in productSS)
                    {
                        if (product.CLAVE == "0") // Significa que vamos a pagar faltas.
                        {
                            if (pay.RTYPE == "D") // D si sin dobles de lo contrario son simples.
                            {
                                // Obtenemos el tipo de pago.
                                RTYPE rmap = db.RTYPEs.Where(x => x.simple == product.PRECIO).FirstOrDefault();
                                decimal _rprice = rmap == null ? product.PRECIO * 2 : rmap.doble;
                                // Calcular cuanto va a pagar por los turnos dobles.
                                // Significa que es doble.
                                query = "EXECUTE CSP_REGISTRAVENTADET 'T-" + string.Format("{0:0}", NumTicekt) + @"','" + product.Id_Producto + "','" + (pay.A == "OPERADOR" ? reference : taxi) + "','2','" + product.CLAVE + "','" + product.IDA + "'," + pay.PAGA + ",'" + product.CARGO + " DOBLE" + "'," + _rprice + ",0,0," + _rprice + "," + (_rprice * pay.PAGA) + ",10," + ((_rprice) / (decimal)1.16).ToString() + ",0,0,172.41,0," + ((_rprice) / (decimal)1.16).ToString() + "," + ((_rprice) / (decimal)1.16).ToString() + "," + _rprice + "," + (_rprice * pay.PAGA) + ",0";
                                Debug.WriteLine(query);
                                a = this.db.Database.ExecuteSqlCommand(query);
                                query = "EXECUTE CSP_REGISTRAMOVDET '" + product.Id_Producto + "','T-" + string.Format("{0:0}", NumTicekt) + @"',0," + pay.PAGA + "," + _rprice + "," + (_rprice * (decimal)0.16).ToString() + "," + (_rprice * (decimal)0.16).ToString() + "";
                                Debug.WriteLine(query);
                                a = this.db.Database.ExecuteSqlCommand(query);
                                // Suma las faltas dobles al monto que se va a cobrar.
                                totalAmount += _rprice * pay.PAGA;
                                Console.WriteLine(totalAmount);
                            }
                            else // Faltas SIMPLES
                            {
                                query = "EXECUTE CSP_REGISTRAVENTADET 'T-" + string.Format("{0:0}", NumTicekt) + @"','" + product.Id_Producto + "','" + (pay.A == "OPERADOR" ? reference : taxi) + "','2','" + product.CLAVE + "','" + product.IDA + "'," + pay.PAGA + ",'" + product.CARGO + "'," + product.PRECIO + ",0,0," + product.PRECIO + "," + (product.PRECIO * pay.PAGA) + ",10," + (product.PRECIO / (decimal)1.16).ToString() + ",0,0,172.41,0," + (product.PRECIO / (decimal)1.16).ToString() + "," + (product.PRECIO / (decimal)1.16).ToString() + "," + product.PRECIO + "," + (product.PRECIO * pay.PAGA) + ",0";
                                Debug.WriteLine(query);
                                a = this.db.Database.ExecuteSqlCommand(query);
                                query = "EXECUTE CSP_REGISTRAMOVDET '" + product.Id_Producto + "','T-" + string.Format("{0:0}", NumTicekt) + @"',0," + pay.PAGA + "," + product.PRECIO + "," + (product.PRECIO * (decimal)0.16).ToString() + "," + (product.PRECIO * (decimal)0.16).ToString() + "";
                                Debug.WriteLine(query);
                                a = this.db.Database.ExecuteSqlCommand(query);
                                // Suma las faltas simples al monto que se va a cobrar.
                                totalAmount += (pay.PRECIO * pay.PAGA);
                                Console.WriteLine(totalAmount);

                            }
                            // Actualizamos invnetario.
                            query = "EXECUTE AJR_ACTUALIZAINVE '" + product.Id_Producto + "'," + pay.PAGA + ",2,'" + date + "','" + username + "','" + (pay.A == "OPERADOR" ? reference : taxi) + "'";
                            Debug.WriteLine(query);
                            a = this.db.Database.ExecuteSqlCommand(query);
                            // Actualizamos historico asistencia.
                            for (int i = 0; i < pay.PAGA; i++)
                            {
                                query = "EXECUTE AJR_REGISTRAASISTENCIA '" + (pay.A == "OPERADOR" ? reference : taxi) + "','" + (pay.A == "OPERADOR" ? "OP" : "E") + "','" + taxi + "','" + pay.RTYPE + "','" + date + "','1','T-" + string.Format("{0:0}", NumTicekt) + @"','" + date + "', 0,'18991230'";
                                Debug.WriteLine(i + " :: " + query);
                                a = this.db.Database.ExecuteSqlCommand(query);
                                Console.WriteLine("");
                            }
                        }
                        else
                        { 
                            // Es un elemento del inventario
                            query = "EXECUTE CSP_REGISTRAVENTADET 'T-" + string.Format("{0:0}", NumTicekt) + @"','" + product.Id_Producto + "','" + (pay.A == "OPERADOR" ? reference : taxi) + "','2','" + product.CLAVE + "','" + product.IDA + "',1,'" + product.CARGO + "'," + product.PRECIO + ",0,0," + product.PRECIO + "," + (product.PRECIO * pay.PAGA) + ",10," + (product.PRECIO / (decimal)1.16).ToString() + ",0,0,172.41,0," + (product.PRECIO / (decimal)1.16).ToString() + "," + (product.PRECIO / (decimal)1.16).ToString() + "," + product.PRECIO + ", " + (product.PRECIO * pay.PAGA) + ",0";
                            Debug.WriteLine(query);
                            a = this.db.Database.ExecuteSqlCommand(query);
                            query = "EXECUTE CSP_REGISTRAMOVDET '" + product.Id_Producto + "','T-" + string.Format("{0:0}", NumTicekt) + @"',0,1," + product.PRECIO + "," + (product.PRECIO * (decimal)0.16).ToString() + "," + (product.PRECIO * (decimal)0.16).ToString() + "";
                            Debug.WriteLine(query);
                            a = this.db.Database.ExecuteSqlCommand(query);
                            // Suma las faltas simples al monto que se va a cobrar.
                            Console.WriteLine(totalAmount);
                            query = "EXECUTE AJR_ACTUALIZAINVE '" + product.Id_Producto + "',1,2,'" + date + "','" + username + "','" + (pay.A == "OPERADOR" ? reference : taxi) + "'";
                            Debug.WriteLine(query);
                            a = this.db.Database.ExecuteSqlCommand(query);
                            // Actualizar inventario.
                            if (product.CONVENIO != null && product.PRODUCTO != null)
                            {
                                query = "AJR_REGISTRACOBROCONVENIOS  'X', '" + product.Id_Producto + "','T-" + string.Format("{0:0}", NumTicekt) + "','" + date + "','" + username + "'";
                                Debug.WriteLine(query);
                                a = this.db.Database.ExecuteSqlCommand(query);
                            }
                            totalAmount += (pay.PRECIO * pay.PAGA);
                        }
                    }
                }

                // Creamos el Ticket.
                query = @"EXECUTE CSP_REGISTRAVENTA '1','T-" + string.Format("{0:0}", NumTicekt) + @"','" + taxi + "','" + reference + "',17,'','Emitida'," + totalAmount + ",0,0," + totalAmount + "," + (totalAmount / (decimal)1.16).ToString() + ",0," + totalAmount + ",'" + username + "','" + date + "','10:10','" + time + "','" + username + "'," + totalAmount + ",17";
                Debug.WriteLine(query);
                a = this.db.Database.ExecuteSqlCommand(query);
                query = "EXECUTE CSP_REGISTRAMOV '1','T-" + string.Format("{0:0}", NumTicekt) + @"', 6,'Sal','T-" + string.Format("{0:0}", NumTicekt) + @"',''," + totalAmount + "," + (totalAmount / (decimal)1.16).ToString() + ",'" + date + "','" + username + "'";
                Debug.WriteLine(query);
                a = this.db.Database.ExecuteSqlCommand(query);

                Console.WriteLine("Actualizado");
                query = "EXECUTE CSP_REGISTRACOBRO 'T-" + string.Format("{0:0}", NumTicekt) + @"',17," + totalAmount + "," + totalAmount + ",0,'" + date + "','" + date + "',''";
                Debug.WriteLine(query);
                a = this.db.Database.ExecuteSqlCommand(query);
                query = "EXECUTE CSP_REGISTRACOBRODET 'T-" + string.Format("{0:0}", NumTicekt) + @"','1',''," + totalAmount + ",'Efectivo',0,1," + totalAmount;
                Debug.WriteLine(query);
                a = this.db.Database.ExecuteSqlCommand(query);
                // Regresamos el numero de ticket.

                //Obtenemos la información del ticket;
                Ticket_DTO ticketDTO = new Ticket_DTO();
                List<Ticket_DTO> tickets = ticketDTO.get("T-" + string.Format("{0:0}", NumTicekt)); // Obtenemos la informacion del ticket.
                decimal total = 0;

                for (int i = 0; i < tickets.Count(); i++)
                {
                    tickets[i].Total = tickets[i].Cant * tickets[i].PrecioConiva;
                    total += tickets[i].Total;
                }
                Ticket_Detail_DTO ticket_Detail_DTO = new Ticket_Detail_DTO();
                ticket_Detail_DTO.total = total;
                ticket_Detail_DTO.tickets = tickets;
                ticket_Detail_DTO.ticket = "T-" + string.Format("{0:0}", NumTicekt);
                return ticket_Detail_DTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Ticket_Detail_DTO paytwo(string username, string IP, string taxi, string reference, List<caja_cargos_DTO> pays, caja_debs_reporte_DTO data)
        {
            try
            {
                bool DoReport = false;
                bool isFault = false;
                int days = 0;
                string _taxi = "000";
                // Reporte OPERADOR
                string queryReporte = "SELECT MAX(FECHAREPORTE) AS FECHAREPORTE FROM HISTASISTENCIAS WHERE GAFETE='" + reference + "'";
                caja_reporte_DTO Reporte = db.Database.SqlQuery<caja_reporte_DTO>(queryReporte).ToList().FirstOrDefault();
                string queryReporteT = "SELECT MAX(TURNO) AS TURNO FROM HISTASISTENCIAS WHERE GAFETE = '" + reference + "' AND FECHAREPORTE = '" + Reporte.FECHAREPORTE.ToString("yyyyMMdd") + "'";
                Reporte.TURNO = db.Database.SqlQuery<caja_reporte_DTO>(queryReporteT).ToList().FirstOrDefault().TURNO;

                // Reporte SOC
                string queryReporteSOC = "SELECT MAX(FECHAREPORTE) AS FECHAREPORTE FROM HISTASISTENCIAS WHERE GAFETE='" + taxi + "'";
                caja_reporte_DTO ReporteSOC = db.Database.SqlQuery<caja_reporte_DTO>(queryReporteSOC).ToList().FirstOrDefault();
                string queryReporteTSOC = "SELECT MAX(TURNO) AS TURNO FROM HISTASISTENCIAS WHERE GAFETE = '" + taxi + "' AND FECHAREPORTE = '" + ReporteSOC.FECHAREPORTE.ToString("yyyyMMdd") + "'";
                Reporte.TURNO = db.Database.SqlQuery<caja_reporte_DTO>(queryReporteT).ToList().FirstOrDefault().TURNO;

                // Obtenemos el numero de ticket.
                string queryConsecutivo = @"SELECT DOCUMENTO, VALOR FROM  Venta01.dbo.CONSECUTIVOS WHERE DOCUMENTO='Venta'";
                Consecutivo_DTO consecutivo = db.Database.SqlQuery<Consecutivo_DTO>(queryConsecutivo).ToList().FirstOrDefault();
                Decimal NumTicekt = consecutivo.VALOR; // Obtenemos el numero de ticket
                Decimal NumConsec = NumTicekt + 1; // Actualizamos el siguiente numero de ticket.
                string query = @"exec sp_executesql N'UPDATE ""VENTA01""..""CONSECUTIVOS"" SET ""VALOR""=@P1 WHERE ""DOCUMENTO""=@P2 AND ""VALOR""=@P3',N'@P1 money,@P2 varchar(8000),@P3 money',$" + NumConsec.ToString() + @",'Venta',$" + consecutivo.VALOR.ToString();
                var a = this.db.Database.ExecuteSqlCommand(query);
                string date = DateTime.Now.ToString("yyyyMMdd");
                string time = DateTime.Now.ToString("HH:mm");
                string lastReportDate = data.report.FECHAREPORTE.ToString("yyyyMMdd");
                decimal totalAmount = 0;

                // Registramos cada uno de los conceptos
                foreach (var pay in pays.Where(x => x.PAGA > 0))
                {
                    List<caja_cargos_DTO> productSS = new List<caja_cargos_DTO>();
                    if (pay.CONVENIO != null && pay.PRODUCTO != null)
                    {
                        // Obtenemos las deudas que se van a pagar directamente desde la base de datos para que no tengan modificaciones en el Front
                        productSS = data.debs
                            .Where(x => x.CLAVE == pay.CLAVE && x.A == pay.A && x.PRODUCTO == pay.PRODUCTO)
                            .OrderBy(x => x.FechaOp)
                            .Take(pay.PAGA)
                            .ToList<caja_cargos_DTO>();
                    }
                    else if (pay.CLAVE == "36") // Adaptamos el objeto. 
                    {
                        productSS = new List<caja_cargos_DTO>();
                        productSS.Add(pay);
                    }
                    else
                    {
                        // Obtenemos las deudas que se van a pagar directamente desde la base de datos para que no tengan modificaciones en el Front
                        productSS = data.debs
                            .Where(x => x.CLAVE == pay.CLAVE && x.A == pay.A)
                            .OrderBy(x => x.FechaOp)
                            .Take(pay.PAGA)
                            .ToList<caja_cargos_DTO>();
                    }

                    // Una vez que tenemos los prodcutos por separados pasamos a registrarlos en ventas.
                    foreach (var product in productSS) // Recorremos cada uno de los productos e identificamos que es cada que.
                    {
                        // Verificamos si es falta.
                        if (product.CLAVE != "0" && product.CLAVE != "36")// si es distinto a 0 lo tratamos como cualquier otro proceso.
                        {
                            // Signifca que le pega a inventario
                            // Registramos el detalle de venta.
                            query = "EXECUTE CSP_REGISTRAVENTADET 'T-" + string.Format("{0:0}", NumTicekt) + @"','" + product.Id_Producto + "','" + (pay.A == "OPERADOR" ? reference : taxi) + "','2','" + product.CLAVE + "','" + product.IDA + "',1,'" + product.CARGO + "'," + product.PRECIO + ",0,0," + product.PRECIO + "," + (product.PRECIO * pay.PAGA) + ",10," + (product.PRECIO / (decimal)1.16).ToString() + ",0,0,172.41,0," + (product.PRECIO / (decimal)1.16).ToString() + "," + (product.PRECIO / (decimal)1.16).ToString() + "," + product.PRECIO + ", " + (product.PRECIO * pay.PAGA) + ",0";
                            Debug.WriteLine(query);
                            a = this.db.Database.ExecuteSqlCommand(query);

                            // Se registra el movimiento.
                            query = "EXECUTE CSP_REGISTRAMOVDET '" + product.Id_Producto + "','T-" + string.Format("{0:0}", NumTicekt) + @"',0,1," + product.PRECIO + "," + (product.PRECIO * (decimal)0.16).ToString() + "," + (product.PRECIO * (decimal)0.16).ToString() + "";
                            Debug.WriteLine(query);
                            a = this.db.Database.ExecuteSqlCommand(query);

                            query = "EXECUTE AJR_ACTUALIZAINVE '" + product.Id_Producto + "',1,2,'" + date + "','" + username + "','" + (pay.A == "OPERADOR" ? reference : taxi) + "'";
                            Debug.WriteLine(query);
                            a = this.db.Database.ExecuteSqlCommand(query);
                        }
                        else if (product.CLAVE == "0") // Registran faltas.
                        {
                            if (pay.RTYPE == "D") // D si sin dobles de lo contrario son simples.
                            {
                                // Obtenemos el tipo de pago.
                                RTYPE rmap = db.RTYPEs.Where(x => x.simple == product.PRECIO).FirstOrDefault();
                                decimal _rprice = rmap == null ? product.PRECIO * 2 : rmap.doble;
                                // Calcular cuanto va a pagar por los turnos dobles.
                                // Significa que es doble.
                                query = "EXECUTE CSP_REGISTRAVENTADET 'T-" + string.Format("{0:0}", NumTicekt) + @"','" + product.Id_Producto + "','" + (pay.A == "OPERADOR" ? reference : taxi) + "','2','" + product.CLAVE + "','" + product.IDA + "'," + pay.PAGA + ",'" + product.CARGO + " DOBLE" + "'," + _rprice + ",0,0," + _rprice + "," + (_rprice * pay.PAGA) + ",10," + ((_rprice) / (decimal)1.16).ToString() + ",0,0,172.41,0," + ((_rprice) / (decimal)1.16).ToString() + "," + ((_rprice) / (decimal)1.16).ToString() + "," + _rprice + "," + (_rprice * pay.PAGA) + ",0";
                                Debug.WriteLine(query);
                                a = this.db.Database.ExecuteSqlCommand(query);
                                query = "EXECUTE CSP_REGISTRAMOVDET '" + product.Id_Producto + "','T-" + string.Format("{0:0}", NumTicekt) + @"',0," + pay.PAGA + "," + _rprice + "," + (_rprice * (decimal)0.16).ToString() + "," + (_rprice * (decimal)0.16).ToString() + "";
                                Debug.WriteLine(query);
                                a = this.db.Database.ExecuteSqlCommand(query);
                                // Suma las faltas dobles al monto que se va a cobrar.
                                totalAmount += _rprice * pay.PAGA;
                                Console.WriteLine(totalAmount);
                            }
                            else // Faltas SIMPLES
                            {
                                query = "EXECUTE CSP_REGISTRAVENTADET 'T-" + string.Format("{0:0}", NumTicekt) + @"','" + product.Id_Producto + "','" + (pay.A == "OPERADOR" ? reference : taxi) + "','2','" + product.CLAVE + "','" + product.IDA + "'," + pay.PAGA + ",'" + product.CARGO + "'," + product.PRECIO + ",0,0," + product.PRECIO + "," + (product.PRECIO * pay.PAGA) + ",10," + (product.PRECIO / (decimal)1.16).ToString() + ",0,0,172.41,0," + (product.PRECIO / (decimal)1.16).ToString() + "," + (product.PRECIO / (decimal)1.16).ToString() + "," + product.PRECIO + "," + (product.PRECIO * pay.PAGA) + ",0";
                                Debug.WriteLine(query);
                                a = this.db.Database.ExecuteSqlCommand(query);
                                query = "EXECUTE CSP_REGISTRAMOVDET '" + product.Id_Producto + "','T-" + string.Format("{0:0}", NumTicekt) + @"',0," + pay.PAGA + "," + product.PRECIO + "," + (product.PRECIO * (decimal)0.16).ToString() + "," + (product.PRECIO * (decimal)0.16).ToString() + "";
                                Debug.WriteLine(query);
                                a = this.db.Database.ExecuteSqlCommand(query);
                                // Suma las faltas simples al monto que se va a cobrar.
                                totalAmount += (pay.PRECIO * pay.PAGA);
                                Console.WriteLine(totalAmount);

                            }
                            // Actualizamos invnetario.
                            query = "EXECUTE AJR_ACTUALIZAINVE '" + product.Id_Producto + "'," + pay.PAGA + ",2,'" + date + "','" + username + "','" + (pay.A == "OPERADOR" ? reference : taxi) + "'";
                            Debug.WriteLine(query);
                            a = this.db.Database.ExecuteSqlCommand(query);
                            // Actualizamos historico asistencia.
                            DoReport = true; // Seleccionamos si se tiene que actualizar el historico.
                            isFault = true;
                            days = pay.PAGA; // Se asigna cuantos paga.
                        }
                        else if (product.CLAVE == "36") // Se registran reportes.
                        {
                            // Setear taxi
                            string[] partes = product.CARGO.Replace(" ","").Split('-');
                            _taxi = partes[1];
                            if (pay.RTYPE == "D") // D si sin dobles de lo contrario son simples.
                            {
                                // Obtenemos el tipo de pago.
                                RTYPE rmap = db.RTYPEs.Where(x => x.simple == product.PRECIO).FirstOrDefault();
                                decimal _rprice = rmap == null ? product.PRECIO * 2 : rmap.doble;
                                // Calcular cuanto va a pagar por los turnos dobles.
                                // Significa que es doble.
                                query = "EXECUTE CSP_REGISTRAVENTADET 'T-" + string.Format("{0:0}", NumTicekt) + @"','" + product.Id_Producto + "','" + (pay.A == "OPERADOR" ? reference : taxi) + "','2','" + product.CLAVE + "','" + product.IDA + "'," + pay.PAGA + ",'" + product.CARGO + " DOBLE" + "'," + _rprice + ",0,0," + _rprice + "," + (_rprice * pay.PAGA) + ",10," + ((_rprice) / (decimal)1.16).ToString() + ",0,0,172.41,0," + ((_rprice) / (decimal)1.16).ToString() + "," + ((_rprice) / (decimal)1.16).ToString() + "," + _rprice + "," + (_rprice * pay.PAGA) + ",0";
                                Debug.WriteLine(query);
                                a = this.db.Database.ExecuteSqlCommand(query);
                                query = "EXECUTE CSP_REGISTRAMOVDET '" + product.Id_Producto + "','T-" + string.Format("{0:0}", NumTicekt) + @"',0," + pay.PAGA + "," + _rprice + "," + (_rprice * (decimal)0.16).ToString() + "," + (_rprice * (decimal)0.16).ToString() + "";
                                Debug.WriteLine(query);
                                a = this.db.Database.ExecuteSqlCommand(query);
                                // Suma las faltas dobles al monto que se va a cobrar.
                                totalAmount += _rprice * pay.PAGA;
                                Console.WriteLine(totalAmount);
                            }
                            else // Faltas SIMPLES
                            {
                                query = "EXECUTE CSP_REGISTRAVENTADET 'T-" + string.Format("{0:0}", NumTicekt) + @"','" + product.Id_Producto + "','" + (pay.A == "OPERADOR" ? reference : taxi) + "','2','" + product.CLAVE + "','" + product.IDA + "'," + pay.PAGA + ",'" + product.CARGO + "'," + product.PRECIO + ",0,0," + product.PRECIO + "," + (product.PRECIO * pay.PAGA) + ",10," + (product.PRECIO / (decimal)1.16).ToString() + ",0,0,172.41,0," + (product.PRECIO / (decimal)1.16).ToString() + "," + (product.PRECIO / (decimal)1.16).ToString() + "," + product.PRECIO + "," + (product.PRECIO * pay.PAGA) + ",0";
                                Debug.WriteLine(query);
                                a = this.db.Database.ExecuteSqlCommand(query);
                                query = "EXECUTE CSP_REGISTRAMOVDET '" + product.Id_Producto + "','T-" + string.Format("{0:0}", NumTicekt) + @"',0," + pay.PAGA + "," + product.PRECIO + "," + (product.PRECIO * (decimal)0.16).ToString() + "," + (product.PRECIO * (decimal)0.16).ToString() + "";
                                Debug.WriteLine(query);
                                a = this.db.Database.ExecuteSqlCommand(query);
                                // Suma las faltas simples al monto que se va a cobrar.
                                totalAmount += (pay.PRECIO * pay.PAGA);
                                Console.WriteLine(totalAmount);
                            }
                            // Actualizamos invnetario.
                            //query = "EXECUTE AJR_ACTUALIZAINVE '" + product.Id_Producto + "'," + pay.PAGA + ",2,'" + date + "','" + username + "','" + (pay.A == "OPERADOR" ? reference : taxi) + "'";
                            //Debug.WriteLine(query);
                            //a = this.db.Database.ExecuteSqlCommand(query);
                            // Actualizamos historico asistencia.
                            DoReport = true; // Seleccionamos si se tiene que actualizar el historico.
                            days = pay.PAGA; // Se asigna cuantos paga.
                            isFault = false;
                        }
                    }
                }

                // Creamos el Ticket.
                query = @"EXECUTE CSP_REGISTRAVENTA '1','T-" + string.Format("{0:0}", NumTicekt) + @"','" + taxi + "','" + reference + "',17,'','Emitida'," + totalAmount + ",0,0," + totalAmount + "," + (totalAmount / (decimal)1.16).ToString() + ",0," + totalAmount + ",'" + username + "','" + date + "','10:10','" + time + "','" + username + "'," + totalAmount + ",17";
                Debug.WriteLine(query);
                a = this.db.Database.ExecuteSqlCommand(query);
                query = "EXECUTE CSP_REGISTRAMOV '1','T-" + string.Format("{0:0}", NumTicekt) + @"', 6,'Sal','T-" + string.Format("{0:0}", NumTicekt) + @"',''," + totalAmount + "," + (totalAmount / (decimal)1.16).ToString() + ",'" + date + "','" + username + "'";
                Debug.WriteLine(query);
                a = this.db.Database.ExecuteSqlCommand(query);
                Console.WriteLine("Actualizado");
                query = "EXECUTE CSP_REGISTRACOBRO 'T-" + string.Format("{0:0}", NumTicekt) + @"',17," + totalAmount + "," + totalAmount + ",0,'" + date + "','" + date + "',''";
                Debug.WriteLine(query);
                a = this.db.Database.ExecuteSqlCommand(query);
                query = "EXECUTE CSP_REGISTRACOBRODET 'T-" + string.Format("{0:0}", NumTicekt) + @"','1',''," + totalAmount + ",'Efectivo',0,1," + totalAmount;
                Debug.WriteLine(query);
                a = this.db.Database.ExecuteSqlCommand(query);
                // Regresamos el numero de ticket.
                //Obtenemos la información del ticket;
                Ticket_DTO ticketDTO = new Ticket_DTO();
                List<Ticket_DTO> tickets = ticketDTO.get("T-" + string.Format("{0:0}", NumTicekt)); // Obtenemos la informacion del ticket.
                decimal total = 0;
                for (int i = 0; i < tickets.Count(); i++)
                {
                    tickets[i].Total = tickets[i].Cant * tickets[i].PrecioConiva;
                    total += tickets[i].Total;
                }

                Ticket_Detail_DTO ticket_Detail_DTO = new Ticket_Detail_DTO();
                ticket_Detail_DTO.total = total;
                ticket_Detail_DTO.tickets = tickets;
                ticket_Detail_DTO.ticket = "T-" + string.Format("{0:0}", NumTicekt);

                // Verificamos si se tiene que reportar a la persona.
                if (DoReport)
                {
                    // Obtener la ultima fecha de reporte.
                    _HistAsistencia = new HistAsistencia(); // Obtenemos el metodo para registrar las faltas y actualizar los reportes.
                    DateTime lastreport = _HistAsistencia.getlastreport(reference);
                    // Registramos
                    if (_HistAsistencia.reportedays(days, reference, "A", isFault ? 1 : 2, ticket_Detail_DTO.ticket, DateTime.Now, _taxi))
                    {
                        Console.WriteLine("Se hizo");
                    }
                    else { 
                        Console.WriteLine("No se hizo");
                    }
                }

                return ticket_Detail_DTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public List<caja_cargos_DTO> getdetails(string taxi, string reference, string family) {
            string query = @"SELECT I.ID_FAMILIA AS CLAVE, F.FAMILIA AS CARGO,I.ID_SUBFAMILIA AS IDA, 0 AS PAGA, I.PrecioConIva as PRECIO, I.Descripcion, I.FechaOp
                            from Inventario as I
                            INNER JOIN Familia as f on I.Id_Familia = f.Id_Familia
                            WHERE 
                            I.Id_Proveedor = '" + taxi + @"' and I.Id_Familia = '" + family + @"' or 
                            I.Id_Proveedor = '"  + reference + "' and I.Id_Familia = '" + family + @"';";
            List<caja_cargos_DTO> Cargos = db.Database.SqlQuery<caja_cargos_DTO>(query).ToList();
            return Cargos;
        }


        public Inventario GetInventario(string gafet, string familia)
        {
            try
            {
                return db.Inventarios.Where(x => x.Id_Proveedor == gafet && x.Id_Familia == familia).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Venta getventa(string ticket) {
            try
            {
                return this.db.Ventas.Where(X => X.Id_Op == ticket).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
