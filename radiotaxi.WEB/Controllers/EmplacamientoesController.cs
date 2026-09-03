using Amazon.EC2.Model;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PagedList;
using radiotaxi.Model;
using radiotaxi.Model.v2.Partner;
using radiotaxi.WEB.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace radiotaxi.WEB.Controllers
{
    [SessionFilter("Admin, Practi-Emplacamiento, Practi-Plusvalia, Practi-BasicView")]
    [RoutePrefix("emplacamiento")]
    public class EmplacamientoesController : webBaseController
    {
        [Route("detalle/{gafet}")]
        public async Task<ActionResult> detail(string gafet)
        {
            try
            {   
                // Esto define al socio... su información basica.
                Partner _Partner = new Partner();
                _Partner = _Partner.get(gafet);
                var debsTask = Task.Run(() => getDebs(gafet));
                var SalesTask = Task.Run(() => getSales(_Partner));
                if (_Partner != null && _Partner.bucket != null && _Partner.key != null)
                {
                    _Partner.urlImage = AmazonHelper.getImageUser(_Partner.bucket, _Partner.key);
                }
                await Task.WhenAll(debsTask, SalesTask);

                // Catalogos.
                ViewBag.marcas = db.EmplaMarcas.ToList();
                ViewBag.modelos = db.EmplaModeloes.ToList();
                ViewBag.movimientos = db.EmplaMovimientos.ToList();
                ViewBag.aseguradoras = db.AseguradorasEmplas.ToList();

                List<caja_cargos_DTO> debs = debsTask.Result;
                decimal amountdebs = 0;
                foreach (caja_cargos_DTO deb in debs)
                {
                    if (deb.CLAVE != "0")
                    {
                        amountdebs += (deb.DEBE * deb.PRECIO);
                    }
                }
                ViewBag.debs = debs;
                ViewBag.amountdebs = amountdebs;
                ViewBag.sales = SalesTask.Result;
                //ViewBag.soc = _Partner;
                Emplacamiento emplacamiento = new Emplacamiento();
                List<Emplacamiento> emplacados = emplacamiento.getHistory(gafet);
                List<emplaBitacora> bitacoras = emplacamiento.getBitacora(gafet);
                foreach (Emplacamiento item in emplacados)
                {
                    EmplaMarca emplaMarca = db.EmplaMarcas.Where(x => x.Id_Marca == item.Id_Marca).FirstOrDefault<EmplaMarca>();
                    EmplaModelo emplaModelo = db.EmplaModeloes.Where(x => x.Id_Modelo == item.Id_Modelo).FirstOrDefault<EmplaModelo>();
                    item.Marca = emplaMarca.Marca;
                    item.Modelo = emplaModelo.Modelo;
                }
                Emplacamiento emplacadoAc = emplacados.Where(x => x.Operacion == "EMPLACADO").FirstOrDefault();
                ViewBag.emplaactual = emplacadoAc is null ? emplacados.FirstOrDefault() : emplacadoAc; 
                ViewBag.emplaHist = emplacados;
                ViewBag.movimientos = db.EmplaMovimientos.Where(x => x.Tipo_Mov == "M").ToList();
                ViewBag.bajaactual = emplacados.Where(x => x.Operacion != "EMPLACADO").OrderByDescending(x => x.FechaBaja).FirstOrDefault();
                ViewBag.bitacoras = bitacoras;
                return View(_Partner);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        private List<caja_cargos_DTO> getDebs(string gafet)
        {
            try
            {
                // DEUDAS
                Sale sale = new Sale(this.email);
                return sale.debs(gafet, gafet);
            }
            catch (Exception ex)
            {
                return new List<caja_cargos_DTO>();
            }
        }
        private List<Venta> getSales(Partner _Partner)
        {
            try
            {
                return _Partner.sales();
            }
            catch (Exception ex)
            {
                return new List<Venta>();
            }
        }

        // GET: Client
        [SessionFilter("Admin, Practi-Emplacamiento, Practi-BasicView")]
        [Route("")]
        public ActionResult Index(int? page)
        {
            if (this.PartnerSearchParameters == null) this.PartnerSearchParameters = new PartnerSearchParametersDTO();
            ViewBag.parameters = this.PartnerSearchParameters; // Objeto que contiene los parametros de busqueda, estos se guardan en session pero estransparante para la capa del controlador.
            Partner Partner = new Partner();
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            PartnerPagination PartnerPagination = Partner.getempla(pageNumber, pageSize, this.PartnerSearchParameters.gafet, this.PartnerSearchParameters.name, this.PartnerSearchParameters.lastName1, this.PartnerSearchParameters.lastName2, this.PartnerSearchParameters.serie, this.PartnerSearchParameters.placa, this.PartnerSearchParameters.EconoResp, this.PartnerSearchParameters.Responsable);
            // Obtener la imagen para N operadores.
            ViewBag.totalClients = PartnerPagination.TotalItems;
            return View(PartnerPagination);
        }

        [SessionFilter("Admin, Practi-Emplacamiento, Practi-BasicView")]
        [HttpPost]
        [Route("")]
        public ActionResult Index(int? page, string gafet, string name, string lastName1, string lastName2, string placa, string serie, string EconoResp, string Responsable)
        {
            try
            {
                this.PartnerSearchParameters = new PartnerSearchParametersDTO();
                this.PartnerSearchParameters.gafet = gafet; // Este es el gafet de operador
                this.PartnerSearchParameters.name = name;
                this.PartnerSearchParameters.lastName1 = lastName1;
                this.PartnerSearchParameters.lastName2 = lastName2;
                this.PartnerSearchParameters.serie = serie;
                this.PartnerSearchParameters.placa = placa;
                this.PartnerSearchParameters.placa = placa;
                this.PartnerSearchParameters.EconoResp = EconoResp;
                this.PartnerSearchParameters.Responsable = Responsable;

                ViewBag.parameters = this.PartnerSearchParameters;
                Partner Partner = new Partner();
                int statusConnection = 0;
                int pageSize = 100;
                int pageNumber = (page ?? 1);
                PartnerPagination clientPagination = Partner.getempla(pageNumber, pageSize, gafet, name, lastName1, lastName2, serie, placa, EconoResp, Responsable);
                ViewBag.totalClients = clientPagination.TotalItems;
                return View(clientPagination);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Emplacamientoes/Create
        [Route("crear/{gafet}")]
        public ActionResult Create( string gafet)
        {
            try
            {
                //ViewBag.soc = _Partner;
                Emplacamiento emplacamiento = new Emplacamiento();
                List<Emplacamiento> emplacados = emplacamiento.getHistory(gafet);
                List<emplaBitacora> bitacoras = emplacamiento.getBitacora(gafet);
                foreach (Emplacamiento item in emplacados)
                {
                    EmplaMarca emplaMarca = db.EmplaMarcas.Where(x => x.Id_Marca == item.Id_Marca).FirstOrDefault<EmplaMarca>();
                    EmplaModelo emplaModelo = db.EmplaModeloes.Where(x => x.Id_Modelo == item.Id_Modelo).FirstOrDefault<EmplaModelo>();
                    item.Marca = emplaMarca.Marca;
                    item.Modelo = emplaModelo.Modelo;
                }
                Emplacamiento emplacadoAc = emplacados.Where(x => x.Operacion == "EMPLACADO").FirstOrDefault();
                emplacadoAc = emplacadoAc is null ? emplacados.FirstOrDefault() : emplacadoAc;
                if (emplacadoAc.Protegido == true)
                {
                    return Redirect("/emplacamiento/protegido/");
                }
                // Info Socio
                Partner _Partner = new Partner();
                _Partner = _Partner.get(gafet);
                ViewBag.soc = _Partner;
                // Catalogos.
                ViewBag.marcas = db.EmplaMarcas.OrderBy(x => x.Marca).ToList();
                ViewBag.modelos = db.EmplaModeloes.OrderBy(x => x.Modelo).ToList();
                ViewBag.movimientos = db.EmplaMovimientos.OrderBy(x => x.Movimiento).ToList();
                ViewBag.aseguradoras = db.AseguradorasEmplas.OrderBy(x => x.aseguradora).ToList();
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        // POST: Emplacamientoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("enviar")]
        public ActionResult Create([Bind(Include = "No_Economico,Nombre,Paterno,Materno,Id_Marca,Id_Modelo,AutoAno,Motor,No_Serie,Placas,Capacidad,EconoResp,Responsable,Observaciones,Status,Bloqueado,Protegido,FechaOp,resp_telefono,resp_celular,poliza,tipopoliza,aseguradora,vencepoliza,Id_Mov")] Emplacamiento emplacamiento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string pattern = @"^[A-HJ-NPR-Z0-9]{17}$"; //Verificamos que el VIN Tenga el formato correcto.
                    emplacamiento.No_Serie = emplacamiento.No_Serie.ToUpper();
                    if (!Regex.IsMatch(emplacamiento.No_Serie, pattern))
                    {
                        ViewBag.message = "Error: Vin incorrecto.";
                        return View(emplacamiento);
                    }
                    // Verificamos si el VIN ESTA BLOQUEADO.
                    this.vins = emplacamiento.VINLock();
                    if (this.vins != null && this.vins.Count() > 0)
                    {
                        // Redireccionamos a el bloqueo de emplacamiento con el VIN.
                        this.vins = null; // Reseteamos VINs.
                        return Redirect("/emplacamiento/vinbloqueado/" + emplacamiento.No_Serie);
                    }

                    Secretaria emplacamientoSec = db.Secretarias.Where(x => x.Id == "18").FirstOrDefault();
                    Secretaria recaudadoraSec = db.Secretarias.Where(x => x.Id == "19").FirstOrDefault();
                    Secretaria encarcagadoMovSec = db.Secretarias.Where(x => x.Id == "20").FirstOrDefault();
                    //Consecutivo consecutivo = dbventas.Consecutivos.Where(x => x.Documento == "EMPLACAMIENTO").FirstOrDefault();

                    // Seleccionamos el tipo de movimiento segun el tipo de movimiento es la accion.
                    if (emplacamiento != null && emplacamiento.Id_Mov == "61")
                    { 
                        // SI ESTA EMPLACADO Y LO REEMPLACO LO DA DE BAJA AUTOMATICAMENTE.
                        Emplacamiento EmplaCar = db.Emplacamientoes.Where(x => x.No_Economico == emplacamiento.No_Economico && x.Status == "EMPLACADO" && x.Operacion == "EMPLACADO").FirstOrDefault();
                        Emplacamiento BajaCar = db.Emplacamientoes.Where(x => x.No_Economico == emplacamiento.No_Economico && x.Status == "BAJA" && x.Operacion == "BAJA").FirstOrDefault();
                        if (EmplaCar != null) // Si es una baja directa.
                        {
                            EmplaCar.Status = "HISTORICO";
                            EmplaCar.Operacion = "BAJA";
                            db.Entry<Emplacamiento>(EmplaCar);
                            db.SaveChanges();
                        }
                        if (BajaCar != null) // Si es una baja directa.
                        {
                            BajaCar.Status = "HISTORICO";
                            BajaCar.Operacion = "BAJA";
                            db.Entry<Emplacamiento>(BajaCar);
                            db.SaveChanges();
                        }
                        // Obtenemos el consecutivo de emplacamiento desde la base de datos Ventas01.
                        string query = @"select * from Consecutivos as c 
                                     where c.Documento = 'EMPLACAMIENTO';";
                        Consecutivo consecutivo = dbventas.Database.SqlQuery<Consecutivo>(query).FirstOrDefault();

                        // Asuganamos el valor consecutivo del emplacamiento.
                        emplacamiento.Id_Op = consecutivo.Valor + 1;
                        query = @"update Consecutivos set Valor = " + emplacamiento.Id_Op + @"
                                     where Documento = 'EMPLACAMIENTO';";
                        dbventas.Database.ExecuteSqlCommand(query);
                        dbventas.SaveChanges();

                        // Llenamos los datos del modelo.
                        emplacamiento.Propiedad = "Socio";
                        emplacamiento.Operacion = "EMPLACADO";
                        emplacamiento.TitularEmpla = emplacamientoSec.Titular;
                        emplacamiento.TitularRentas = recaudadoraSec.Titular;
                        emplacamiento.TitularCT = encarcagadoMovSec.Titular;
                        emplacamiento.TitularOtro = "";
                        emplacamiento.Id_Usuario = this.User.userId.ToString();
                        emplacamiento.Id_UsuarioEdit = this.User.userId.ToString();
                        emplacamiento.Nombre2 = "";
                        emplacamiento.Status = "EMPLACADO";
                        emplacamiento.FechaOp = DateTime.Now;
                        db.Emplacamientoes.Add(emplacamiento);
                        db.SaveChanges();

                        emplaBitacora emplaBitacora = new emplaBitacora();
                        emplaBitacora.id_emplacamiento = emplacamiento.Id_Op;
                        emplaBitacora.movBy = this.UserName;
                        emplaBitacora.movDate = DateTime.Now;
                        emplaBitacora.action = "ALTA";
                        emplaBitacora.objPos = this.serializeFlat(emplacamiento);
                        emplaBitacora.objPre = this.serialize(null);
                        db.emplaBitacoras.Add(emplaBitacora);
                        db.SaveChanges();

                        return Redirect("/emplacamiento/alta/correcta/" + emplacamiento.Id_Op.ToString().Replace(".0000", ""));
                    }
                    else if ( // BAJA

                         emplacamiento != null && emplacamiento.Id_Mov == "25" ||
                         emplacamiento != null && emplacamiento.Id_Mov == "10" ||
                         emplacamiento != null && emplacamiento.Id_Mov == "17" ||
                         emplacamiento != null && emplacamiento.Id_Mov == "4" ||
                         emplacamiento != null && emplacamiento.Id_Mov == "16" ||
                         emplacamiento != null && emplacamiento.Id_Mov == "33" ||
                         emplacamiento != null && emplacamiento.Id_Mov == "18" ||
                         emplacamiento != null && emplacamiento.Id_Mov == "45"
                     )
                    { // "ALTA POR PAGO DE DERECHO Y T/C 2025 (ALTA)" Esto significa que es un alta... 

                    }
                    else if ( // REFRENDOS
                        emplacamiento != null && emplacamiento.Id_Mov == "62" ||
                        emplacamiento != null && emplacamiento.Id_Mov == "15" 
                    )
                    {

                    } else {
                        // MOVIMIENTO NO IDENTIFICADO.
                        return Redirect("/emplacamiento/movimiento/invalido");
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                Console.WriteLine(e.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(emplacamiento);
        }

        [HttpGet]
        [Route("renderpdf/{id_operacion}")]
        public ActionResult renderpdf(decimal id_operacion)
        {
            // Obtenemos los datos de la baja.
            Emplacamiento emplacamiento = db.Emplacamientoes.Where(x => x.Id_Op == id_operacion).FirstOrDefault<Emplacamiento>();
            EmplaMarca emplaMarca = db.EmplaMarcas.Where(x => x.Id_Marca == emplacamiento.Id_Marca).FirstOrDefault<EmplaMarca>();
            EmplaModelo emplaModelo = db.EmplaModeloes.Where(x => x.Id_Modelo == emplacamiento.Id_Modelo).FirstOrDefault<EmplaModelo>();
            EmplaMovimiento emplaMovimiento = db.EmplaMovimientos.Where(x => x.Id_Mov == emplacamiento.Id_Mov).FirstOrDefault<EmplaMovimiento>();
            Partner _partner = (new Partner()).get(((int)emplacamiento.No_Economico).ToString());

            // GENERAMOS EL PDF con los datos.
            // Fecha vencimiento.
            string expDate = DateTime.Now.AddDays(30).ToShortDateString().ToString();
            Document document = new Document();
            MigraDoc.DocumentObjectModel.Section section = document.AddSection();
            section.PageSetup.Orientation = Orientation.Portrait;
            section.PageSetup.PageHeight = "279.4mm";
            section.PageSetup.PageWidth = "215.9mm";

            // Cabecera
            TextFrame tc = section.AddTextFrame();
            tc.Width = "150mm";
            tc.Height = "20mm";
            tc.RelativeVertical = RelativeVertical.Page;
            tc.RelativeHorizontal = RelativeHorizontal.Page;
            tc.Top = "17mm"; // Misma altura
            tc.Left = "32mm"; // Posición horizontal

            Paragraph paragraphc = tc.AddParagraph(); // Se agrega al TextFrame correcto
            paragraphc.Format.Font.Size = new Unit(12);
            paragraphc.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
            paragraphc.AddFormattedText("SINDICATO DE CHOFERES, TAXISTAS Y SIMILARES DEL CARIBE\n");
            paragraphc.AddFormattedText("ANDRES QUINTANA ROO\n");
            paragraphc.AddFormattedText("CONOCIDO");

            paragraphc.Format.Font.Name = "Arial";
            paragraphc.Format.Font.Bold = false;
            paragraphc.Format.SpaceAfter = "0.1cm";
            paragraphc.Format.Font.Color = Colors.Black;

            // V SECCION
            TextFrame tv = section.AddTextFrame();
            tv.Width = "150mm";
            tv.Height = "20mm";
            tv.RelativeVertical = RelativeVertical.Page;
            tv.RelativeHorizontal = RelativeHorizontal.Page;
            tv.Top = "35mm"; // Misma altura
            tv.Left = "10mm"; // Posición horizontal

            Paragraph paragraphv = tv.AddParagraph(); // Se agrega al TextFrame correcto
            paragraphv.Format.Font.Size = new Unit(12);
            //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
            paragraphv.AddFormattedText("V SECCION DEL F..U.T.V.");

            paragraphv.Format.Font.Name = "Arial";
            paragraphv.Format.Font.Bold = false;
            paragraphv.Format.SpaceAfter = "0.1cm";
            paragraphv.Format.Font.Color = Colors.Black;

            // REGISTRO
            TextFrame tr = section.AddTextFrame();
            tr.Width = "150mm";
            tr.Height = "20mm";
            tr.RelativeVertical = RelativeVertical.Page;
            tr.RelativeHorizontal = RelativeHorizontal.Page;
            tr.Top = "35mm"; // Misma altura
            tr.Left = "165mm"; // Posición horizontal

            Paragraph paragraphr = tr.AddParagraph(); // Se agrega al TextFrame correcto
            paragraphr.Format.Font.Size = new Unit(12);
            //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
            paragraphr.AddFormattedText("REGISTRO No. 48");

            paragraphr.Format.Font.Name = "Arial";
            paragraphr.Format.Font.Bold = false;
            paragraphr.Format.SpaceAfter = "0.1cm";

            // DIVISION
            Paragraph paragraph = section.AddParagraph();
            paragraph.Format.Borders.Bottom.Width = 1; // Grosor de la línea
            paragraph.Format.Borders.Bottom.Color = Colors.Black;
            paragraph.Format.Alignment = ParagraphAlignment.Center; // Centrar la línea horizontalmente
            paragraph.Format.SpaceBefore = "11mm"; // Espacio antes de la línea
            paragraph.Format.SpaceAfter = "1mm"; // Espacio después de la línea
            paragraph.AddText(" "); // Espacio para que la línea sea visible

            // Fecha
            TextFrame ff = section.AddTextFrame();
            ff.Width = "138mm";
            ff.Height = "50mm";
            ff.RelativeVertical = RelativeVertical.Page;
            ff.RelativeHorizontal = RelativeHorizontal.Page;
            ff.Top = "42mm";
            ff.Left = "10mm";
            Paragraph paragraph2 = ff.AddParagraph();
            paragraph2.Format.Font.Size = new Unit(10);
            paragraph2.AddText(("Cancun Q.Roo México a " + DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"))).ToUpper());
            paragraph2.Format.Font.Name = "Arial";
            paragraph2.Format.Font.Bold = false;
            paragraph2.Format.SpaceAfter = "0.2cm";
            paragraph2.Format.Font.Color = Colors.Black;

            // RENDER DE ASUNTO.
            String asunto = "N/A";
            if (emplaMovimiento.Tipo_Mov == "A") // ALTA
            {
                asunto = "ALTA";
            }
            else if (emplaMovimiento.Tipo_Mov == "B")
            {
                asunto = "BAJA";
            }
            if (emplaMovimiento.Tipo_Mov == "M" && emplacamiento.Operacion == "EMPLACADO")
            {
                asunto = "EMPLACADO";
            }

            // OFICIO / ASUNTO
            TextFrame oa = section.AddTextFrame();
            oa.Width = "138mm";
            oa.Height = "50mm";
            oa.RelativeVertical = RelativeVertical.Page;
            oa.RelativeHorizontal = RelativeHorizontal.Page;
            oa.Top = "42mm";
            oa.Left = "150mm";
            Paragraph paragraph3 = oa.AddParagraph();
            paragraph3.Format.Font.Size = new Unit(10);
            paragraph3.AddText(("No. DE OFICIO: " + "\n").ToUpper());
            paragraph3.AddText(("ASUNTO: " + asunto).ToUpper());
            if (emplaMovimiento.Id_Mov == "62") { paragraph3.AddText(("\nRefrendo").ToUpper()); }
            paragraph3.Format.Font.Name = "Arial";
            paragraph3.Format.Font.Bold = false;
            paragraph3.Format.SpaceAfter = "0.2cm";
            paragraph3.Format.Font.Color = Colors.Black;

            // PRESENTE
            TextFrame pr = section.AddTextFrame();
            pr.Width = "138mm";
            pr.Height = "50mm";
            pr.RelativeVertical = RelativeVertical.Page;
            pr.RelativeHorizontal = RelativeHorizontal.Page;
            pr.Top = "55mm";
            pr.Left = "10mm";
            Paragraph paragraph4 = pr.AddParagraph();
            paragraph4.Format.Font.Size = new Unit(12);
            paragraph4.AddText(("Lic. Valeria Uc Chavez \n").ToUpper());
            paragraph4.AddText(("Encargada del Descacho de la Dirección de REcaudacion de Benito Juárez del SATQ \n").ToUpper());
            paragraph4.AddText(("P R E S E N T E").ToUpper());
            paragraph4.Format.Font.Name = "Arial";
            paragraph4.Format.Font.Bold = false;
            paragraph4.Format.SpaceAfter = "0.2cm";
            paragraph4.Format.Font.Color = Colors.Black;

            // PARRAFO
            TextFrame pp = section.AddTextFrame();
            pp.Width = "200mm";
            pp.Height = "50mm";
            pp.RelativeVertical = RelativeVertical.Page;
            pp.RelativeHorizontal = RelativeHorizontal.Page;
            pp.Top = "80mm";
            pp.Left = "10mm";
            Paragraph paragraph5 = pp.AddParagraph();
            paragraph5.Format.Font.Size = new Unit(12);
            paragraph5.AddText(("El Sindicato de Choferes, Taxistas y Similares del Caribe \"Andres Quintana Roo\" hace constar que:").ToUpper());
            paragraph5.Format.Font.Name = "Arial";
            paragraph5.Format.Font.Bold = false;
            paragraph5.Format.SpaceAfter = "0.2cm";
            paragraph5.Format.Font.Color = Colors.Black;

            // Espacio entre el párrafo y la tabla
            Paragraph spacer = section.AddParagraph();
            spacer.Format.SpaceBefore = "50mm"; // Ajustar este valor según necesites

            // Tabla de datos del vehículo
            Table table = section.AddTable();
            table.Borders.Width = 0.75;
            table.Format.Alignment = ParagraphAlignment.Center; // Centrar la línea horizontalmente

            Column column1 = table.AddColumn("4cm");
            Column column2 = table.AddColumn("8cm");

            Row row;

            row = table.AddRow();
            row.Cells[0].AddParagraph("VEHÍCULO MARCA:");
            row.Cells[1].AddParagraph(emplaMarca.Marca + " " + emplaModelo.Modelo);

            row = table.AddRow();
            row.Cells[0].AddParagraph("MODELO:");
            row.Cells[1].AddParagraph(emplacamiento.AutoAno.ToString());

            row = table.AddRow();
            row.Cells[0].AddParagraph("No. DE MOTOR:");
            row.Cells[1].AddParagraph(emplacamiento.Motor.ToUpper());

            row = table.AddRow();
            row.Cells[0].AddParagraph("No. DE SERIE:");
            row.Cells[1].AddParagraph(emplacamiento.No_Serie.ToUpper());

            row = table.AddRow();
            row.Cells[0].AddParagraph("No. DE PLACAS:");
            row.Cells[1].AddParagraph(emplacamiento.Placas.ToUpper());

            row = table.AddRow();
            row.Cells[0].AddParagraph("CAPACIDAD:");
            row.Cells[1].AddParagraph(emplacamiento.Capacidad.ToString() + " Pasajeros más conductor".ToUpper());

            // Espacio
            section.AddParagraph("\n");

            // PARRAFO
            TextFrame pp2 = section.AddTextFrame();
            pp2.Width = "200mm";
            pp2.Height = "50mm";
            pp2.RelativeVertical = RelativeVertical.Page;
            pp2.RelativeHorizontal = RelativeHorizontal.Page;
            pp2.Top = "130mm";
            pp2.Left = "10mm";
            Paragraph paragraph6 = pp2.AddParagraph();
            paragraph6.Format.Font.Size = new Unit(12);
            paragraph6.AddFormattedText(("Es propiedad del Sr. ").ToUpper());
            
            paragraph6.AddFormattedText((_partner.firstName + " " + _partner.lastNameF + " " + _partner.lastNameM).ToUpper(), TextFormat.Bold);
            paragraph6.AddFormattedText((" socio propietario con numero economico: ").ToUpper());

            //
            int partnerRef;
            int.TryParse(_partner.partnerReference, out partnerRef);

            if (partnerRef <= 9999)
            {
                paragraph6.AddFormattedText((_partner.partnerReference).ToUpper(), TextFormat.Bold);

            }
            else if (partnerRef >= 10000 && partnerRef <= 15000)
            {
                partnerRef = partnerRef - 10000;
                paragraph6.AddFormattedText((partnerRef + " Colectivo").ToUpper(), TextFormat.Bold);
            }
            else if (partnerRef >= 80000 && partnerRef <= 81000)
            {
                partnerRef = partnerRef - 80000;
                paragraph6.AddFormattedText((partnerRef + " Servicio público especializado").ToUpper(), TextFormat.Bold);
            }
            //

            paragraph6.AddFormattedText((" a reserva que esta se tramite en la secretaria de hacienda y credito publico el cambio de propietario. \n \n").ToUpper());
            paragraph6.AddFormattedText((emplaMovimiento.Movimiento).ToUpper());
            paragraph6.Format.Font.Name = "Arial";
            paragraph6.Format.Font.Bold = false;
            paragraph6.Format.SpaceAfter = "0.2cm";
            paragraph6.Format.Font.Color = Colors.Black;

            // FIRMA
            TextFrame pf = section.AddTextFrame();
            pf.Width = "200mm";
            pf.Height = "50mm";
            pf.RelativeVertical = RelativeVertical.Page;
            pf.RelativeHorizontal = RelativeHorizontal.Page;
            pf.Top = "160mm";
            pf.Left = "10mm";
            Paragraph paragraph7 = pf.AddParagraph();
            paragraph7.Format.Font.Size = new Unit(12);
            paragraph7.Format.Alignment = ParagraphAlignment.Center;
            paragraph7.AddFormattedText(("\nATENTAMENTE").ToUpper());
            paragraph7.Format.Alignment = ParagraphAlignment.Center;
            paragraph7.AddFormattedText("\n\n\n\n\n\n\n");
            paragraph7.AddFormattedText("__________________________________________________\n");
            paragraph7.Format.Alignment = ParagraphAlignment.Center;
            paragraph7.AddFormattedText(("C. Hernan Herrera Och\n").ToUpper());
            paragraph7.AddFormattedText(("Jefe de Emplacamiento").ToUpper());
            paragraph7.Format.Font.Name = "Arial";
            paragraph7.Format.Font.Bold = false;
            paragraph7.Format.SpaceAfter = "0.2cm";
            paragraph7.Format.Font.Color = Colors.Black;

            string userName = this.UserName;
            string initials = string.Join("", userName.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries).Select(word => word[0]));

            // NOTA
            TextFrame pn = section.AddTextFrame();
            pn.Width = "200mm";
            pn.Height = "50mm";
            pn.RelativeVertical = RelativeVertical.Page;
            pn.RelativeHorizontal = RelativeHorizontal.Page;
            pn.Top = "240mm";
            pn.Left = "10mm";
            Paragraph paragraph8 = pn.AddParagraph();
            paragraph8.Format.Font.Size = new Unit(12);
            paragraph8.AddFormattedText(("NOTA: ").ToUpper(), TextFormat.Bold);
            paragraph8.AddFormattedText((" Los socios menores de edad seran representados por su padre o tutor que sean socios activos de este sindicato.\n\n").ToUpper());
            paragraph8.AddFormattedText(("Gestor:" + initials).ToUpper());
            paragraph8.Format.Font.Name = "Arial";
            paragraph8.Format.Font.Bold = false;
            paragraph8.Format.SpaceAfter = "0.2cm";
            paragraph8.Format.Font.Color = Colors.Black;


            //SEGUNDA HOJA
            MigraDoc.DocumentObjectModel.Section section2 = document.AddSection();
            section2.PageSetup.Orientation = Orientation.Portrait;
            section2.PageSetup.PageHeight = "279.4mm";
            section2.PageSetup.PageWidth = "215.9mm";

            // Cabecera
            TextFrame _tc = section2.AddTextFrame();
            _tc.Width = "150mm";
            _tc.Height = "20mm";
            _tc.RelativeVertical = RelativeVertical.Page;
            _tc.RelativeHorizontal = RelativeHorizontal.Page;
            _tc.Top = "17mm"; // Misma altura
            _tc.Left = "32mm"; // Posición horizontal

            Paragraph _paragraphc = _tc.AddParagraph(); // Se agrega al TextFrame correcto
            _paragraphc.Format.Font.Size = new Unit(12);
            _paragraphc.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
            _paragraphc.AddFormattedText("SINDICATO DE CHOFERES, TAXISTAS Y SIMILARES DEL CARIBE\n");
            _paragraphc.AddFormattedText("ANDRES QUINTANA ROO\n");
            _paragraphc.AddFormattedText("CONOCIDO");

            _paragraphc.Format.Font.Name = "Arial";
            _paragraphc.Format.Font.Bold = false;
            _paragraphc.Format.SpaceAfter = "0.1cm";
            _paragraphc.Format.Font.Color = Colors.Black;

            // V SECCION
            TextFrame _tv = section2.AddTextFrame();
            _tv.Width = "150mm";
            _tv.Height = "20mm";
            _tv.RelativeVertical = RelativeVertical.Page;
            _tv.RelativeHorizontal = RelativeHorizontal.Page;
            _tv.Top = "35mm"; // Misma altura
            _tv.Left = "10mm"; // Posición horizontal

            Paragraph _paragraphv = _tv.AddParagraph(); // Se agrega al TextFrame correcto
            _paragraphv.Format.Font.Size = new Unit(12);
            //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
            _paragraphv.AddFormattedText("V SECCION DEL F..U.T.V.");

            _paragraphv.Format.Font.Name = "Arial";
            _paragraphv.Format.Font.Bold = false;
            _paragraphv.Format.SpaceAfter = "0.1cm";
            _paragraphv.Format.Font.Color = Colors.Black;

            // REGISTRO
            TextFrame _tr = section2.AddTextFrame();
            _tr.Width = "150mm";
            _tr.Height = "20mm";
            _tr.RelativeVertical = RelativeVertical.Page;
            _tr.RelativeHorizontal = RelativeHorizontal.Page;
            _tr.Top = "35mm"; // Misma altura
            _tr.Left = "165mm"; // Posición horizontal

            Paragraph _paragraphr = _tr.AddParagraph(); // Se agrega al TextFrame correcto
            _paragraphr.Format.Font.Size = new Unit(12);
            //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
            _paragraphr.AddFormattedText("REGISTRO No. 48");

            paragraphr.Format.Font.Name = "Arial";
            paragraphr.Format.Font.Bold = false;
            paragraphr.Format.SpaceAfter = "0.1cm";

            // DIVISION
            Paragraph _paragraph = section2.AddParagraph();
            _paragraph.Format.Borders.Bottom.Width = 1; // Grosor de la línea
            _paragraph.Format.Borders.Bottom.Color = Colors.Black;
            _paragraph.Format.Alignment = ParagraphAlignment.Center; // Centrar la línea horizontalmente
            _paragraph.Format.SpaceBefore = "11mm"; // Espacio antes de la línea
            _paragraph.Format.SpaceAfter = "1mm"; // Espacio después de la línea
            _paragraph.AddText(" "); // Espacio para que la línea sea visible

            // Fecha
            TextFrame _ff = section2.AddTextFrame();
            _ff.Width = "138mm";
            _ff.Height = "50mm";
            _ff.RelativeVertical = RelativeVertical.Page;
            _ff.RelativeHorizontal = RelativeHorizontal.Page;
            _ff.Top = "42mm";
            _ff.Left = "10mm";
            Paragraph _paragraph2 = _ff.AddParagraph();
            _paragraph2.Format.Font.Size = new Unit(10);
            _paragraph2.AddText(("Cancun Q.Roo México a " + DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"))).ToUpper());
            _paragraph2.Format.Font.Name = "Arial";
            _paragraph2.Format.Font.Bold = false;
            _paragraph2.Format.SpaceAfter = "0.2cm";
            _paragraph2.Format.Font.Color = Colors.Black;

            // RENDER DE ASUNTO.
            //String asunto = "N/A";
            if (emplaMovimiento.Tipo_Mov == "A") // ALTA
            {
                asunto = "ALTA";
            }
            else if (emplaMovimiento.Tipo_Mov == "B")
            {
                asunto = "BAJA";
            }
            if (emplaMovimiento.Tipo_Mov == "M" && emplacamiento.Operacion == "EMPLACADO")
            {
                asunto = "EMPLACADO";
            }

            // OFICIO / ASUNTO
            TextFrame _oa = section2.AddTextFrame();
            _oa.Width = "138mm";
            _oa.Height = "50mm";
            _oa.RelativeVertical = RelativeVertical.Page;
            _oa.RelativeHorizontal = RelativeHorizontal.Page;
            _oa.Top = "42mm";
            _oa.Left = "150mm";
            Paragraph _paragraph3 = _oa.AddParagraph();
            _paragraph3.Format.Font.Size = new Unit(10);
            _paragraph3.AddText(("No. DE OFICIO: " + "\n").ToUpper());
            _paragraph3.AddText(("ASUNTO: " + asunto).ToUpper());
            if (emplaMovimiento.Id_Mov == "62") { _paragraph3.AddText(("\nRefrendo").ToUpper()); }
            _paragraph3.Format.Font.Name = "Arial";
            _paragraph3.Format.Font.Bold = false;
            _paragraph3.Format.SpaceAfter = "0.2cm";
            _paragraph3.Format.Font.Color = Colors.Black;

            // PRESENTE
            TextFrame _pr = section2.AddTextFrame();
            _pr.Width = "138mm";
            _pr.Height = "50mm";
            _pr.RelativeVertical = RelativeVertical.Page;
            _pr.RelativeHorizontal = RelativeHorizontal.Page;
            _pr.Top = "55mm";
            _pr.Left = "10mm";
            Paragraph _paragraph4 = _pr.AddParagraph();
            _paragraph4.Format.Font.Size = new Unit(12);
            _paragraph4.AddText(("C. Candelaria Hernández Peniche \n").ToUpper());
            _paragraph4.AddText(("Encargada de la unidad administrativa denominada delegación del instituto de movilidad en Benito Juárez \n").ToUpper());
            _paragraph4.AddText(("P R E S E N T E").ToUpper());
            _paragraph4.Format.Font.Name = "Arial";
            _paragraph4.Format.Font.Bold = false;
            _paragraph4.Format.SpaceAfter = "0.2cm";
            _paragraph4.Format.Font.Color = Colors.Black;

            // PARRAFO
            TextFrame _pp = section2.AddTextFrame();
            _pp.Width = "200mm";
            _pp.Height = "50mm";
            _pp.RelativeVertical = RelativeVertical.Page;
            _pp.RelativeHorizontal = RelativeHorizontal.Page;
            _pp.Top = "80mm";
            _pp.Left = "10mm";
            Paragraph _paragraph5 = _pp.AddParagraph();
            _paragraph5.Format.Font.Size = new Unit(12);
            _paragraph5.AddText(("El Sindicato de Choferes, Taxistas y Similares del Caribe \"Andres Quintana Roo\" hace constar que:").ToUpper());
            _paragraph5.Format.Font.Name = "Arial";
            _paragraph5.Format.Font.Bold = false;
            _paragraph5.Format.SpaceAfter = "0.2cm";
            _paragraph5.Format.Font.Color = Colors.Black;

            // Espacio entre el párrafo y la tabla
            Paragraph _spacer = section2.AddParagraph();
            _spacer.Format.SpaceBefore = "50mm"; // Ajustar este valor según necesites

            // Tabla de datos del vehículo
            Table _table = section2.AddTable();
            _table.Borders.Width = 0.75;
            _table.Format.Alignment = ParagraphAlignment.Center; // Centrar la línea horizontalmente

            Column _column1 = _table.AddColumn("4cm");
            Column _column2 = _table.AddColumn("8cm");

            Row _row;

            _row = _table.AddRow();
            _row.Cells[0].AddParagraph("VEHÍCULO MARCA:");
            _row.Cells[1].AddParagraph(emplaMarca.Marca + " " + emplaModelo.Modelo);

            _row = _table.AddRow();
            _row.Cells[0].AddParagraph("MODELO:");
            _row.Cells[1].AddParagraph(emplacamiento.AutoAno.ToString());

            _row = _table.AddRow();
            _row.Cells[0].AddParagraph("No. DE MOTOR:");
            _row.Cells[1].AddParagraph(emplacamiento.Motor.ToUpper());

            _row = _table.AddRow();
            _row.Cells[0].AddParagraph("No. DE SERIE:");
            _row.Cells[1].AddParagraph(emplacamiento.No_Serie.ToUpper());

            _row = _table.AddRow();
            _row.Cells[0].AddParagraph("No. DE PLACAS:");
            _row.Cells[1].AddParagraph(emplacamiento.Placas.ToUpper());

            _row = _table.AddRow();
            _row.Cells[0].AddParagraph("CAPACIDAD:");
            _row.Cells[1].AddParagraph(emplacamiento.Capacidad.ToString() + " Pasajeros más conductor".ToUpper());

            // Espacio
            section2.AddParagraph("\n");

            // PARRAFO
            TextFrame _pp2 = section2.AddTextFrame();
            _pp2.Width = "200mm";
            _pp2.Height = "50mm";
            _pp2.RelativeVertical = RelativeVertical.Page;
            _pp2.RelativeHorizontal = RelativeHorizontal.Page;
            _pp2.Top = "130mm";
            _pp2.Left = "10mm";
            Paragraph _paragraph6 = _pp2.AddParagraph();
            _paragraph6.Format.Font.Size = new Unit(12);
            _paragraph6.AddFormattedText(("Es propiedad del Sr. ").ToUpper());
            _paragraph6.AddFormattedText((_partner.firstName + " " + _partner.lastNameF + " " + _partner.lastNameM).ToUpper(), TextFormat.Bold);
            _paragraph6.AddFormattedText((" socio propietario con numero economico: ").ToUpper());

            //

            int.TryParse(_partner.partnerReference, out partnerRef);

            if (partnerRef <= 9999)
            {
                _paragraph6.AddFormattedText((_partner.partnerReference).ToUpper(), TextFormat.Bold);

            }
            else if (partnerRef >= 10000 && partnerRef <= 15000)
            {
                partnerRef = partnerRef - 10000;
                _paragraph6.AddFormattedText((partnerRef + " Colectivo").ToUpper(), TextFormat.Bold);
            }
            else if (partnerRef >= 80000 && partnerRef <= 81000)
            {
                partnerRef = partnerRef - 80000;
                _paragraph6.AddFormattedText((partnerRef + " Servicio público especializado").ToUpper(), TextFormat.Bold);
            }
            //

            _paragraph6.AddFormattedText((" a reserva que esta se tramite en la secretaria de hacienda y credito publico el cambio de propietario. \n \n").ToUpper());
            _paragraph6.AddFormattedText((emplaMovimiento.Movimiento).ToUpper());
            _paragraph6.Format.Font.Name = "Arial";
            _paragraph6.Format.Font.Bold = false;
            _paragraph6.Format.SpaceAfter = "0.2cm";
            _paragraph6.Format.Font.Color = Colors.Black;

            // FIRMA
            TextFrame _pf = section2.AddTextFrame();
            _pf.Width = "200mm";
            _pf.Height = "50mm";
            _pf.RelativeVertical = RelativeVertical.Page;
            _pf.RelativeHorizontal = RelativeHorizontal.Page;
            _pf.Top = "160mm";
            _pf.Left = "10mm";
            Paragraph _paragraph7 = _pf.AddParagraph();
            _paragraph7.Format.Font.Size = new Unit(12);
            _paragraph7.Format.Alignment = ParagraphAlignment.Center;
            _paragraph7.AddFormattedText(("\nATENTAMENTE").ToUpper());
            _paragraph7.Format.Alignment = ParagraphAlignment.Center;
            _paragraph7.AddFormattedText("\n\n\n\n\n\n\n");
            _paragraph7.AddFormattedText("__________________________________________________\n");
            _paragraph7.Format.Alignment = ParagraphAlignment.Center;
            _paragraph7.AddFormattedText(("C. Hernan Herrera Och\n").ToUpper());
            _paragraph7.AddFormattedText(("Jefe de Emplacamiento").ToUpper());
            _paragraph7.Format.Font.Name = "Arial";
            _paragraph7.Format.Font.Bold = false;
            _paragraph7.Format.SpaceAfter = "0.2cm";
            _paragraph7.Format.Font.Color = Colors.Black;


            // NOTA
            TextFrame _pn = section2.AddTextFrame();
            _pn.Width = "200mm";
            _pn.Height = "50mm";
            _pn.RelativeVertical = RelativeVertical.Page;
            _pn.RelativeHorizontal = RelativeHorizontal.Page;
            _pn.Top = "240mm";
            _pn.Left = "10mm";
            Paragraph _paragraph8 = _pn.AddParagraph();
            _paragraph8.Format.Font.Size = new Unit(12);
            _paragraph8.AddFormattedText(("NOTA: ").ToUpper(), TextFormat.Bold);
            _paragraph8.AddFormattedText((" Los socios menores de edad seran representados por su padre o tutor que sean socios activos de este sindicato.\n\n").ToUpper());
            _paragraph8.AddFormattedText(("Gestor:" + initials).ToUpper());
            _paragraph8.Format.Font.Name = "Arial";
            _paragraph8.Format.Font.Bold = false;
            _paragraph8.Format.SpaceAfter = "0.2cm";
            _paragraph8.Format.Font.Color = Colors.Black;

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;
            // Layout and render document to PDF
            pdfRenderer.RenderDocument();

            // Send PDF to browser                
            using (MemoryStream stream = new MemoryStream())
            {
                pdfRenderer.Save(stream, false);
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", stream.Length.ToString());
                Response.BinaryWrite(stream.ToArray());
                Response.Flush();
                stream.Position = 0;
                stream.Close();
                Response.End();
                return File(stream, "application/pdf", "PE:pdf");
            }
        }

        [HttpGet]
        [Route("renderpdf/{id_op}/{id_mov}")]
        public ActionResult renderpdf(decimal id_op, string id_mov)
        {
            // Obtenemos los datos de la baja.
            Emplacamiento emplacamiento = db.Emplacamientoes.Where(x => x.Id_Op == id_op).FirstOrDefault<Emplacamiento>();
            EmplaMarca emplaMarca = db.EmplaMarcas.Where(x => x.Id_Marca == emplacamiento.Id_Marca).FirstOrDefault<EmplaMarca>();
            EmplaModelo emplaModelo = db.EmplaModeloes.Where(x => x.Id_Modelo == emplacamiento.Id_Modelo).FirstOrDefault<EmplaModelo>();
            EmplaMovimiento emplaMovimiento = db.EmplaMovimientos.Where(x => x.Id_Mov == id_mov).FirstOrDefault<EmplaMovimiento>();
            Partner _partner = (new Partner()).get(((int)emplacamiento.No_Economico).ToString());
            

            // GENERAMOS EL PDF con los datos.
            // Fecha vencimiento.
            string expDate = DateTime.Now.AddDays(30).ToShortDateString().ToString();
            Document document = new Document();
            MigraDoc.DocumentObjectModel.Section section = document.AddSection();
            section.PageSetup.Orientation = Orientation.Portrait;
            section.PageSetup.PageHeight = "279.4mm";
            section.PageSetup.PageWidth = "215.9mm";

            // Cabecera
            TextFrame tc = section.AddTextFrame();
            tc.Width = "150mm";
            tc.Height = "20mm";
            tc.RelativeVertical = RelativeVertical.Page;
            tc.RelativeHorizontal = RelativeHorizontal.Page;
            tc.Top = "17mm"; // Misma altura
            tc.Left = "32mm"; // Posición horizontal

            Paragraph paragraphc = tc.AddParagraph(); // Se agrega al TextFrame correcto
            paragraphc.Format.Font.Size = new Unit(12);
            paragraphc.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
            paragraphc.AddFormattedText("SINDICATO DE CHOFERES, TAXISTAS Y SIMILARES DEL CARIBE\n");
            paragraphc.AddFormattedText("ANDRES QUINTANA ROO\n");
            paragraphc.AddFormattedText("CONOCIDO");

            paragraphc.Format.Font.Name = "Arial";
            paragraphc.Format.Font.Bold = false;
            paragraphc.Format.SpaceAfter = "0.1cm";
            paragraphc.Format.Font.Color = Colors.Black;

            // V SECCION
            TextFrame tv = section.AddTextFrame();
            tv.Width = "150mm";
            tv.Height = "20mm";
            tv.RelativeVertical = RelativeVertical.Page;
            tv.RelativeHorizontal = RelativeHorizontal.Page;
            tv.Top = "35mm"; // Misma altura
            tv.Left = "10mm"; // Posición horizontal

            Paragraph paragraphv = tv.AddParagraph(); // Se agrega al TextFrame correcto
            paragraphv.Format.Font.Size = new Unit(12);
            //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
            paragraphv.AddFormattedText("V SECCION DEL F..U.T.V.");

            paragraphv.Format.Font.Name = "Arial";
            paragraphv.Format.Font.Bold = false;
            paragraphv.Format.SpaceAfter = "0.1cm";
            paragraphv.Format.Font.Color = Colors.Black;

            // REGISTRO
            TextFrame tr = section.AddTextFrame();
            tr.Width = "150mm";
            tr.Height = "20mm";
            tr.RelativeVertical = RelativeVertical.Page;
            tr.RelativeHorizontal = RelativeHorizontal.Page;
            tr.Top = "35mm"; // Misma altura
            tr.Left = "165mm"; // Posición horizontal

            Paragraph paragraphr = tr.AddParagraph(); // Se agrega al TextFrame correcto
            paragraphr.Format.Font.Size = new Unit(12);
            //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
            paragraphr.AddFormattedText("REGISTRO No. 48");

            paragraphr.Format.Font.Name = "Arial";
            paragraphr.Format.Font.Bold = false;
            paragraphr.Format.SpaceAfter = "0.1cm";

            // DIVISION
            Paragraph paragraph = section.AddParagraph();
            paragraph.Format.Borders.Bottom.Width = 1; // Grosor de la línea
            paragraph.Format.Borders.Bottom.Color = Colors.Black;
            paragraph.Format.Alignment = ParagraphAlignment.Center; // Centrar la línea horizontalmente
            paragraph.Format.SpaceBefore = "11mm"; // Espacio antes de la línea
            paragraph.Format.SpaceAfter = "1mm"; // Espacio después de la línea
            paragraph.AddText(" "); // Espacio para que la línea sea visible

            
            // Fecha
            TextFrame ff = section.AddTextFrame();
            ff.Width = "138mm";
            ff.Height = "50mm";
            ff.RelativeVertical = RelativeVertical.Page;
            ff.RelativeHorizontal = RelativeHorizontal.Page;
            ff.Top = "42mm";
            ff.Left = "10mm";
            Paragraph paragraph2 = ff.AddParagraph();
            paragraph2.Format.Font.Size = new Unit(10);

            //paragraph2.AddText(("Cancun Q.Roo México a " + (emplacamiento.FechaOp != null ? emplacamiento.FechaOp.Value.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES")) : DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES")))).ToUpper());
            paragraph2.AddText(("Cancun Q.Roo México a " + DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"))).ToUpper());
            paragraph2.Format.Font.Name = "Arial";
            paragraph2.Format.Font.Bold = false;
            paragraph2.Format.SpaceAfter = "0.2cm";
            paragraph2.Format.Font.Color = Colors.Black;

            // RENDER DE ASUNTO.
            String asunto = "N/A";
            if (emplaMovimiento.Tipo_Mov == "A") // ALTA
            {
                asunto = "ALTA";
            }
            else if (emplaMovimiento.Tipo_Mov == "B")
            {
                asunto = "BAJA";
            }
            if (emplaMovimiento.Tipo_Mov == "M" && emplacamiento.Operacion == "EMPLACADO")
            {
                asunto = "EMPLACADO";
            }

            // OFICIO / ASUNTO
            TextFrame oa = section.AddTextFrame();
            oa.Width = "138mm";
            oa.Height = "50mm";
            oa.RelativeVertical = RelativeVertical.Page;
            oa.RelativeHorizontal = RelativeHorizontal.Page;
            oa.Top = "42mm";
            oa.Left = "150mm";
            Paragraph paragraph3 = oa.AddParagraph();
            paragraph3.Format.Font.Size = new Unit(10);
            paragraph3.AddText(("No. DE OFICIO: " + "\n").ToUpper());
            paragraph3.AddText(("ASUNTO: " + asunto).ToUpper());
            if(id_mov == "62") { paragraph3.AddText(("\nRefrendo").ToUpper()); }
            paragraph3.Format.Font.Name = "Arial";
            paragraph3.Format.Font.Bold = false;
            paragraph3.Format.SpaceAfter = "0.2cm";
            paragraph3.Format.Font.Color = Colors.Black;

            // PRESENTE
            TextFrame pr = section.AddTextFrame();
            pr.Width = "138mm";
            pr.Height = "50mm";
            pr.RelativeVertical = RelativeVertical.Page;
            pr.RelativeHorizontal = RelativeHorizontal.Page;
            pr.Top = "55mm";
            pr.Left = "10mm";
            Paragraph paragraph4 = pr.AddParagraph();
            paragraph4.Format.Font.Size = new Unit(12);
            paragraph4.AddText(("Lic. Valeria Uc Chavez \n").ToUpper());
            paragraph4.AddText(("Encargada del Despacho de la Dirección de Recaudación de Benito Juárez del SATQ \n").ToUpper());
            paragraph4.AddText(("P R E S E N T E").ToUpper());
            paragraph4.Format.Font.Name = "Arial";
            paragraph4.Format.Font.Bold = false;
            paragraph4.Format.SpaceAfter = "0.2cm";
            paragraph4.Format.Font.Color = Colors.Black;

            // PARRAFO
            TextFrame pp = section.AddTextFrame();
            pp.Width = "200mm";
            pp.Height = "50mm";
            pp.RelativeVertical = RelativeVertical.Page;
            pp.RelativeHorizontal = RelativeHorizontal.Page;
            pp.Top = "80mm";
            pp.Left = "10mm";
            Paragraph paragraph5 = pp.AddParagraph();
            paragraph5.Format.Font.Size = new Unit(12);
            paragraph5.AddText(("El Sindicato de Choferes, Taxistas y Similares del Caribe \"Andres Quintana Roo\" hace constar que:").ToUpper());
            paragraph5.Format.Font.Name = "Arial";
            paragraph5.Format.Font.Bold = false;
            paragraph5.Format.SpaceAfter = "0.2cm";
            paragraph5.Format.Font.Color = Colors.Black;

            // Espacio entre el párrafo y la tabla
            Paragraph spacer = section.AddParagraph();
            spacer.Format.SpaceBefore = "50mm"; // Ajustar este valor según necesites

            // Tabla de datos del vehículo
            Table table = section.AddTable();
            table.Borders.Width = 0.75;
            table.Format.Alignment = ParagraphAlignment.Center; // Centrar la línea horizontalmente

            Column column1 = table.AddColumn("4cm");
            Column column2 = table.AddColumn("8cm");

            Row row;

            row = table.AddRow();
            row.Cells[0].AddParagraph("VEHÍCULO MARCA:");
            row.Cells[1].AddParagraph(emplaMarca.Marca + " " + emplaModelo.Modelo);

            row = table.AddRow();
            row.Cells[0].AddParagraph("MODELO:");
            row.Cells[1].AddParagraph(emplacamiento.AutoAno.ToString());

            row = table.AddRow();
            row.Cells[0].AddParagraph("No. DE MOTOR:");
            row.Cells[1].AddParagraph(emplacamiento.Motor.ToUpper());

            row = table.AddRow();
            row.Cells[0].AddParagraph("No. DE SERIE:");
            row.Cells[1].AddParagraph(emplacamiento.No_Serie.ToUpper());

            row = table.AddRow();
            row.Cells[0].AddParagraph("No. DE PLACAS:");
            row.Cells[1].AddParagraph(emplacamiento.Placas.ToUpper());

            row = table.AddRow();
            row.Cells[0].AddParagraph("CAPACIDAD:");
            row.Cells[1].AddParagraph(emplacamiento.Capacidad.ToString() + " Pasajeros más conductor".ToUpper());

            // Espacio
            section.AddParagraph("\n");

            // PARRAFO
            TextFrame pp2 = section.AddTextFrame();
            pp2.Width = "200mm";
            pp2.Height = "50mm";
            pp2.RelativeVertical = RelativeVertical.Page;
            pp2.RelativeHorizontal = RelativeHorizontal.Page;
            pp2.Top = "130mm";
            pp2.Left = "10mm";
            Paragraph paragraph6 = pp2.AddParagraph();
            paragraph6.Format.Font.Size = new Unit(12);
            paragraph6.AddFormattedText(("Es propiedad del Sr. ").ToUpper());

            paragraph6.AddFormattedText((_partner.firstName + " " + _partner.lastNameF + " " + _partner.lastNameM).ToUpper(), TextFormat.Bold);
            paragraph6.AddFormattedText((" socio propietario con numero economico: ").ToUpper());

            // TEXTO ECO, ESPECIAL O FORANEO
            int partnerRef;
            int.TryParse(_partner.partnerReference, out partnerRef);

            if (partnerRef <= 9999)
            {
                paragraph6.AddFormattedText((_partner.partnerReference).ToUpper(), TextFormat.Bold);

            }
            else if (partnerRef >= 10000 && partnerRef <= 15000)
            {
                partnerRef = partnerRef - 10000;
                paragraph6.AddFormattedText((partnerRef + " Colectivo").ToUpper(), TextFormat.Bold);
            }
            else if (partnerRef >= 80000 && partnerRef <= 81000)
            {
                partnerRef = partnerRef - 80000;
                paragraph6.AddFormattedText((partnerRef + " Servicio público especializado").ToUpper(), TextFormat.Bold);
            }
            //

            paragraph6.AddFormattedText((" a reserva que esta se tramite en la secretaria de hacienda y credito publico el cambio de propietario. \n \n").ToUpper());
            paragraph6.AddFormattedText((emplaMovimiento.Movimiento).ToUpper());
            paragraph6.Format.Font.Name = "Arial";
            paragraph6.Format.Font.Bold = false;
            paragraph6.Format.SpaceAfter = "0.2cm";
            paragraph6.Format.Font.Color = Colors.Black;

            string userName = this.UserName;
            string initials = string.Join("", userName.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries).Select(word => word[0]));

            // FIRMA
            TextFrame pf = section.AddTextFrame();
            pf.Width = "200mm";
            pf.Height = "50mm";
            pf.RelativeVertical = RelativeVertical.Page;
            pf.RelativeHorizontal = RelativeHorizontal.Page;
            pf.Top = "160mm";
            pf.Left = "10mm";
            Paragraph paragraph7 = pf.AddParagraph();
            paragraph7.Format.Font.Size = new Unit(12);
            paragraph7.Format.Alignment = ParagraphAlignment.Center;
            paragraph7.AddFormattedText(("\nATENTAMENTE").ToUpper());
            paragraph7.Format.Alignment = ParagraphAlignment.Center;
            paragraph7.AddFormattedText("\n\n\n\n\n\n\n");
            paragraph7.AddFormattedText("__________________________________________________\n");
            paragraph7.Format.Alignment = ParagraphAlignment.Center;
            paragraph7.AddFormattedText(("C. Hernan Herrera Och\n").ToUpper());
            paragraph7.AddFormattedText(("Jefe de Emplacamiento").ToUpper());
            paragraph7.Format.Font.Name = "Arial";
            paragraph7.Format.Font.Bold = false;
            paragraph7.Format.SpaceAfter = "0.2cm";
            paragraph7.Format.Font.Color = Colors.Black;


            // NOTA
            TextFrame pn = section.AddTextFrame();
            pn.Width = "200mm";
            pn.Height = "50mm";
            pn.RelativeVertical = RelativeVertical.Page;
            pn.RelativeHorizontal = RelativeHorizontal.Page;
            pn.Top = "240mm";
            pn.Left = "10mm";
            Paragraph paragraph8 = pn.AddParagraph();
            paragraph8.Format.Font.Size = new Unit(12);
            paragraph8.AddFormattedText(("NOTA: ").ToUpper(), TextFormat.Bold);
            paragraph8.AddFormattedText((" Los socios menores de edad seran representados por su padre o tutor que sean socios activos de este sindicato.\n\n").ToUpper());
            paragraph8.AddFormattedText(("Gestor: " + initials).ToUpper());

            paragraph8.Format.Font.Name = "Arial";
            paragraph8.Format.Font.Bold = false;
            paragraph8.Format.SpaceAfter = "0.2cm";
            paragraph8.Format.Font.Color = Colors.Black;

            // HOJA 2

            MigraDoc.DocumentObjectModel.Section section2 = document.AddSection();
            section2.PageSetup.Orientation = Orientation.Portrait;
            section2.PageSetup.PageHeight = "279.4mm";
            section2.PageSetup.PageWidth = "215.9mm";

            // Cabecera
            TextFrame _tc = section2.AddTextFrame();
            _tc.Width = "150mm";
            _tc.Height = "20mm";
            _tc.RelativeVertical = RelativeVertical.Page;
            _tc.RelativeHorizontal = RelativeHorizontal.Page;
            _tc.Top = "17mm"; // Misma altura
            _tc.Left = "32mm"; // Posición horizontal

            Paragraph _paragraphc = _tc.AddParagraph(); // Se agrega al TextFrame correcto
            _paragraphc.Format.Font.Size = new Unit(12);
            _paragraphc.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
            _paragraphc.AddFormattedText("SINDICATO DE CHOFERES, TAXISTAS Y SIMILARES DEL CARIBE\n");
            _paragraphc.AddFormattedText("ANDRES QUINTANA ROO\n");
            _paragraphc.AddFormattedText("CONOCIDO");

            _paragraphc.Format.Font.Name = "Arial";
            _paragraphc.Format.Font.Bold = false;
            _paragraphc.Format.SpaceAfter = "0.1cm";
            _paragraphc.Format.Font.Color = Colors.Black;

            // V SECCION
            TextFrame _tv = section2.AddTextFrame();
            _tv.Width = "150mm";
            _tv.Height = "20mm";
            _tv.RelativeVertical = RelativeVertical.Page;
            _tv.RelativeHorizontal = RelativeHorizontal.Page;
            _tv.Top = "35mm"; // Misma altura
            _tv.Left = "10mm"; // Posición horizontal

            Paragraph _paragraphv = _tv.AddParagraph(); // Se agrega al TextFrame correcto
            _paragraphv.Format.Font.Size = new Unit(12);
            //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
            _paragraphv.AddFormattedText("V SECCION DEL F..U.T.V.");

            _paragraphv.Format.Font.Name = "Arial";
            _paragraphv.Format.Font.Bold = false;
            _paragraphv.Format.SpaceAfter = "0.1cm";
            _paragraphv.Format.Font.Color = Colors.Black;

            // REGISTRO
            TextFrame _tr = section2.AddTextFrame();
            _tr.Width = "150mm";
            _tr.Height = "20mm";
            _tr.RelativeVertical = RelativeVertical.Page;
            _tr.RelativeHorizontal = RelativeHorizontal.Page;
            _tr.Top = "35mm"; // Misma altura
            _tr.Left = "165mm"; // Posición horizontal

            Paragraph _paragraphr = _tr.AddParagraph(); // Se agrega al TextFrame correcto
            _paragraphr.Format.Font.Size = new Unit(12);
            //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
            _paragraphr.AddFormattedText("REGISTRO No. 48");

            _paragraphr.Format.Font.Name = "Arial";
            _paragraphr.Format.Font.Bold = false;
            _paragraphr.Format.SpaceAfter = "0.1cm";

            // DIVISION
            Paragraph _paragraph = section2.AddParagraph();
            _paragraph.Format.Borders.Bottom.Width = 1; // Grosor de la línea
            _paragraph.Format.Borders.Bottom.Color = Colors.Black;
            _paragraph.Format.Alignment = ParagraphAlignment.Center; // Centrar la línea horizontalmente
            _paragraph.Format.SpaceBefore = "11mm"; // Espacio antes de la línea
            _paragraph.Format.SpaceAfter = "1mm"; // Espacio después de la línea
            _paragraph.AddText(" "); // Espacio para que la línea sea visible

            // Fecha
            TextFrame _ff = section2.AddTextFrame();
            _ff.Width = "138mm";
            _ff.Height = "50mm";
            _ff.RelativeVertical = RelativeVertical.Page;
            _ff.RelativeHorizontal = RelativeHorizontal.Page;
            _ff.Top = "42mm";
            _ff.Left = "10mm";
            Paragraph _paragraph2 = _ff.AddParagraph();
            _paragraph2.Format.Font.Size = new Unit(10);
            //_paragraph2.AddText(("Cancun Q.Roo México a " + (emplacamiento.FechaOp != null ? emplacamiento.FechaOp.Value.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES")) : DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES")))).ToUpper());
            _paragraph2.AddText(("Cancun Q.Roo México a " + DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"))).ToUpper());
            _paragraph2.Format.Font.Name = "Arial";
            _paragraph2.Format.Font.Bold = false;
            _paragraph2.Format.SpaceAfter = "0.2cm";
            _paragraph2.Format.Font.Color = Colors.Black;

            // RENDER DE ASUNTO.
            //String asunto = "N/A";
            if (emplaMovimiento.Tipo_Mov == "A") // ALTA
            {
                asunto = "ALTA";
            }
            else if (emplaMovimiento.Tipo_Mov == "B")
            {
                asunto = "BAJA";
            }
            if (emplaMovimiento.Tipo_Mov == "M" && emplacamiento.Operacion == "EMPLACADO")
            {
                asunto = "EMPLACADO";
            }

            // OFICIO / ASUNTO
            TextFrame _oa = section2.AddTextFrame();
            _oa.Width = "138mm";
            _oa.Height = "50mm";
            _oa.RelativeVertical = RelativeVertical.Page;
            _oa.RelativeHorizontal = RelativeHorizontal.Page;
            _oa.Top = "42mm";
            _oa.Left = "150mm";
            Paragraph _paragraph3 = _oa.AddParagraph();
            _paragraph3.Format.Font.Size = new Unit(10);
            _paragraph3.AddText(("No. DE OFICIO: " + "\n").ToUpper());
            _paragraph3.AddText(("ASUNTO: " + asunto).ToUpper());
            if (id_mov == "62") { _paragraph3.AddText(("\nRefrendo").ToUpper()); }
            _paragraph3.Format.Font.Name = "Arial";
            _paragraph3.Format.Font.Bold = false;
            _paragraph3.Format.SpaceAfter = "0.2cm";
            _paragraph3.Format.Font.Color = Colors.Black;

            // PRESENTE
            TextFrame _pr = section2.AddTextFrame();
            _pr.Width = "138mm";
            _pr.Height = "50mm";
            _pr.RelativeVertical = RelativeVertical.Page;
            _pr.RelativeHorizontal = RelativeHorizontal.Page;
            _pr.Top = "55mm";
            _pr.Left = "10mm";
            Paragraph _paragraph4 = _pr.AddParagraph();
            _paragraph4.Format.Font.Size = new Unit(12);

            _paragraph4.AddText(("C. Candelaria Hernández Peniche \n").ToUpper());
            _paragraph4.AddText(("Encargada de la unidad administrativa denominada delegación del instituto de movilidad en Benito Juárez \n").ToUpper());
            _paragraph4.AddText(("P R E S E N T E").ToUpper());
            _paragraph4.Format.Font.Name = "Arial";
            _paragraph4.Format.Font.Bold = false;
            _paragraph4.Format.SpaceAfter = "0.2cm";
            _paragraph4.Format.Font.Color = Colors.Black;

            // PARRAFO
            TextFrame _pp = section2.AddTextFrame();
            _pp.Width = "200mm";
            _pp.Height = "50mm";
            _pp.RelativeVertical = RelativeVertical.Page;
            _pp.RelativeHorizontal = RelativeHorizontal.Page;
            _pp.Top = "80mm";
            _pp.Left = "10mm";
            Paragraph _paragraph5 = _pp.AddParagraph();
            _paragraph5.Format.Font.Size = new Unit(12);
            _paragraph5.AddText(("El Sindicato de Choferes, Taxistas y Similares del Caribe \"Andres Quintana Roo\" hace constar que:").ToUpper());
            _paragraph5.Format.Font.Name = "Arial";
            _paragraph5.Format.Font.Bold = false;
            _paragraph5.Format.SpaceAfter = "0.2cm";
            _paragraph5.Format.Font.Color = Colors.Black;

            // Espacio entre el párrafo y la tabla
            Paragraph _spacer = section2.AddParagraph();
            _spacer.Format.SpaceBefore = "50mm"; // Ajustar este valor según necesites

            // Tabla de datos del vehículo
            Table _table = section2.AddTable();
            _table.Borders.Width = 0.75;
            _table.Format.Alignment = ParagraphAlignment.Center; // Centrar la línea horizontalmente
            Column _column1 = _table.AddColumn("4cm");
            Column _column2 = _table.AddColumn("8cm");

            Row _row;

            _row = _table.AddRow();
            _row.Cells[0].AddParagraph("VEHÍCULO MARCA:");
            _row.Cells[1].AddParagraph(emplaMarca.Marca + " " + emplaModelo.Modelo);

            _row = _table.AddRow();
            _row.Cells[0].AddParagraph("MODELO:");
            _row.Cells[1].AddParagraph(emplacamiento.AutoAno.ToString());

            _row = _table.AddRow();
            _row.Cells[0].AddParagraph("No. DE MOTOR:");
            _row.Cells[1].AddParagraph(emplacamiento.Motor);

            _row = _table.AddRow();
            _row.Cells[0].AddParagraph("No. DE SERIE:");
            _row.Cells[1].AddParagraph(emplacamiento.No_Serie);

            _row = _table.AddRow();
            _row.Cells[0].AddParagraph("No. DE PLACAS:");
            _row.Cells[1].AddParagraph(emplacamiento.Placas.ToUpper());

            _row = _table.AddRow();
            _row.Cells[0].AddParagraph("CAPACIDAD:");
            _row.Cells[1].AddParagraph(emplacamiento.Capacidad.ToString() + " Pasajeros más conductor".ToUpper());

            // Espacio
            section2.AddParagraph("\n");

            // PARRAFO
            TextFrame _pp2 = section2.AddTextFrame();
            _pp2.Width = "200mm";
            _pp2.Height = "50mm";
            _pp2.RelativeVertical = RelativeVertical.Page;
            _pp2.RelativeHorizontal = RelativeHorizontal.Page;
            _pp2.Top = "130mm";
            _pp2.Left = "10mm";
            Paragraph _paragraph6 = _pp2.AddParagraph();
            _paragraph6.Format.Font.Size = new Unit(12);
            _paragraph6.AddFormattedText(("Es propiedad del Sr. ").ToUpper());

            _paragraph6.AddFormattedText((_partner.firstName + " " + _partner.lastNameF + " " + _partner.lastNameM).ToUpper(), TextFormat.Bold);
            _paragraph6.AddFormattedText((" socio propietario con numero economico: ").ToUpper());

            // TEXTO ECO, ESPECIAL O FORANEO
            int.TryParse(_partner.partnerReference, out partnerRef);

            if (partnerRef <= 9999)
            {
                _paragraph6.AddFormattedText((_partner.partnerReference).ToUpper(), TextFormat.Bold);

            }else if (partnerRef >= 10000 && partnerRef <= 15000)
            {
                partnerRef = partnerRef - 10000;
                _paragraph6.AddFormattedText((partnerRef + " Colectivo").ToUpper(), TextFormat.Bold);
            }
            else if (partnerRef >= 80000 && partnerRef <= 81000)
            {
                partnerRef = partnerRef - 80000;
                _paragraph6.AddFormattedText((partnerRef + " Servicio público especializado").ToUpper(), TextFormat.Bold);
            }
            //

            _paragraph6.AddFormattedText((" a reserva que esta se tramite en la secretaria de hacienda y credito publico el cambio de propietario. \n \n").ToUpper());
            _paragraph6.AddFormattedText((emplaMovimiento.Movimiento).ToUpper());
            _paragraph6.Format.Font.Name = "Arial";
            _paragraph6.Format.Font.Bold = false;
            _paragraph6.Format.SpaceAfter = "0.2cm";
            _paragraph6.Format.Font.Color = Colors.Black;

            // FIRMA
            TextFrame _pf = section2.AddTextFrame();
            _pf.Width = "200mm";
            _pf.Height = "50mm";
            _pf.RelativeVertical = RelativeVertical.Page;
            _pf.RelativeHorizontal = RelativeHorizontal.Page;
            _pf.Top = "160mm";
            _pf.Left = "10mm";
            Paragraph _paragraph7 = _pf.AddParagraph();
            _paragraph7.Format.Font.Size = new Unit(12);
            _paragraph7.Format.Alignment = ParagraphAlignment.Center;
            _paragraph7.AddFormattedText(("\nATENTAMENTE").ToUpper());
            _paragraph7.Format.Alignment = ParagraphAlignment.Center;
            _paragraph7.AddFormattedText("\n\n\n\n\n\n\n");
            _paragraph7.AddFormattedText("__________________________________________________\n");
            _paragraph7.Format.Alignment = ParagraphAlignment.Center;
            _paragraph7.AddFormattedText(("C. Hernan Herrera Och\n").ToUpper());
            _paragraph7.AddFormattedText(("Jefe de Emplacamiento").ToUpper());
            _paragraph7.Format.Font.Name = "Arial";
            _paragraph7.Format.Font.Bold = false;
            _paragraph7.Format.SpaceAfter = "0.2cm";
            _paragraph7.Format.Font.Color = Colors.Black;


            // NOTA
            TextFrame _pn = section2.AddTextFrame();
            _pn.Width = "200mm";
            _pn.Height = "50mm";
            _pn.RelativeVertical = RelativeVertical.Page;
            _pn.RelativeHorizontal = RelativeHorizontal.Page;
            _pn.Top = "240mm";
            _pn.Left = "10mm";
            Paragraph _paragraph8 = _pn.AddParagraph();
            _paragraph8.Format.Font.Size = new Unit(12);
            _paragraph8.AddFormattedText(("NOTA: ").ToUpper(), TextFormat.Bold);
            _paragraph8.AddFormattedText((" Los socios menores de edad seran representados por su padre o tutor que sean socios activos de este sindicato.\n\n").ToUpper());
            _paragraph8.AddFormattedText(("Gestor: " + initials).ToUpper());

            _paragraph8.Format.Font.Name = "Arial";
            _paragraph8.Format.Font.Bold = false;
            _paragraph8.Format.SpaceAfter = "0.2cm";
            _paragraph8.Format.Font.Color = Colors.Black;

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;
            // Layout and render document to PDF
            pdfRenderer.RenderDocument();

            // Send PDF to browser                
            using (MemoryStream stream = new MemoryStream())
            {
                pdfRenderer.Save(stream, false);
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", stream.Length.ToString());
                Response.BinaryWrite(stream.ToArray());
                Response.Flush();
                stream.Position = 0;
                stream.Close();
                Response.End();
                return File(stream, "application/pdf", "PE:pdf");
            }
        }

        [HttpGet]
        [Route("alta/correcta/{id}")]
        public ActionResult AltaCorrecta(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emplacamiento emplacamiento = db.Emplacamientoes.Find(id);
            if (emplacamiento == null)
            {
                return HttpNotFound();
            }
            ViewBag.emplaMarca = db.EmplaMarcas.Where(x => x.Id_Marca == emplacamiento.Id_Marca).FirstOrDefault<EmplaMarca>();
            ViewBag.emplaModelo = db.EmplaModeloes.Where(x => x.Id_Modelo == emplacamiento.Id_Modelo).FirstOrDefault<EmplaModelo>();
            ViewBag.emplaMovimiento = db.EmplaMovimientos.Where(x => x.Id_Mov == emplacamiento.Id_Mov).FirstOrDefault<EmplaMovimiento>();
            ViewBag.Partner = (new Partner()).get(((int)emplacamiento.No_Economico).ToString());
            return View(emplacamiento);
        }

        [HttpGet]
        [Route("movimiento/{id}/{Id_Mov}")]
        public ActionResult movimiento(decimal id,string Id_Mov)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emplacamiento emplacamiento = db.Emplacamientoes.Find(id);
            if (emplacamiento == null)
            {
                return HttpNotFound();
            }

            // Registrar movimiento. 
            emplaBitacora emplaBitacora = new emplaBitacora(); // Aqui se guardan todos los cambios cuando, quien y que.
            emplaBitacora.objPre = this.serializeFlat(emplacamiento);
            // Moficiacion del movimiento.
            
            ViewBag.emplaMarca = db.EmplaMarcas.Where(x => x.Id_Marca == emplacamiento.Id_Marca).FirstOrDefault<EmplaMarca>();
            ViewBag.emplaModelo = db.EmplaModeloes.Where(x => x.Id_Modelo == emplacamiento.Id_Modelo).FirstOrDefault<EmplaModelo>();
            // ESTE ES EL MOVIMIENTO QUE TIENE EL OBJETO.
            ViewBag.emplaMovimiento = db.EmplaMovimientos.Where(x => x.Id_Mov == emplacamiento.Id_Mov).FirstOrDefault<EmplaMovimiento>();
            ViewBag.Partner = (new Partner()).get(((int)emplacamiento.No_Economico).ToString());
            // ESTE ES EL MOVIMIENTO QUE VOY A HACER.
            ViewBag.movimientos = db.EmplaMovimientos.Where(x => x.Id_Mov == Id_Mov).FirstOrDefault<EmplaMovimiento>();
            string movimiento = "";
            if (Id_Mov == "10") // ESTOY HACIENDO UNA REMPRESION DE BAJA.
            {
                try
                {
                    movimiento += "REIMPRESION : " + ((EmplaMovimiento)ViewBag.emplaMovimiento).Movimiento;
                }
                catch (Exception ex)
                {
                    // SIGNIFICA QUE EL MOVIMIENTO FUE BORRADO
                    movimiento += "REIMPRESION : " + ((EmplaMovimiento)ViewBag.movimientos).Movimiento;
                    emplacamiento.Id_Mov = Id_Mov;
                }

            }
            else if (Id_Mov == "61") // ESTOY REIMPRIMIENDO UN ALTA.
            {
                emplacamiento.Id_Mov = Id_Mov;
                movimiento += "REIMPRESION : " + ((EmplaMovimiento)ViewBag.movimientos).Movimiento;
            }
            else
            {
                emplacamiento.Id_Mov = Id_Mov;
                movimiento += ((EmplaMovimiento)ViewBag.movimientos).Movimiento;
            }

            emplaBitacora.id_emplacamiento = emplacamiento.Id_Op;
            emplaBitacora.movBy = this.UserName;
            emplaBitacora.movDate = DateTime.Now;
            emplaBitacora.action = movimiento;
            emplaBitacora.objPos = this.serializeFlat(emplacamiento);
            db.emplaBitacoras.Add(emplaBitacora); // Agregamos a bitacora el movimiento. 
            db.SaveChanges();
            return View(emplacamiento);
        }

        // GET: Emplacamientoes/Edit/5
        [HttpGet]
        [Route("Editar/{id}")]
        public ActionResult Edit(decimal id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emplacamiento emplacamiento = db.Emplacamientoes.Find(id);
            if (emplacamiento == null)
            {
                return HttpNotFound();
            }

            if (emplacamiento.Protegido == true && !Permissions.allow("Admin"))
            {
                return Redirect("/emplacamiento/protegido/");
            }

            emplacamiento.No_Serie = emplacamiento.No_Serie.ToUpper();


            // Catalogos.
            ViewBag.marcas = db.EmplaMarcas.OrderByDescending(x => x.Marca).OrderByDescending(x => x.Marca).ToList();
            ViewBag.modelos = db.EmplaModeloes.OrderByDescending(x => x.Modelo).OrderByDescending(x => x.Modelo).ToList();
            ViewBag.movimientos = db.EmplaMovimientos.OrderByDescending(x => x.Movimiento).OrderByDescending(x => x.Movimiento).ToList();
            ViewBag.aseguradoras = db.AseguradorasEmplas.OrderByDescending(x => x.aseguradora).ToList();

            ViewBag.emplaMarca = db.EmplaMarcas.Where(x => x.Id_Marca == emplacamiento.Id_Marca).FirstOrDefault<EmplaMarca>();
            ViewBag.emplaModelo = db.EmplaModeloes.Where(x => x.Id_Modelo == emplacamiento.Id_Modelo).FirstOrDefault<EmplaModelo>();
            ViewBag.emplaMovimiento = db.EmplaMovimientos.Where(x => x.Id_Mov == emplacamiento.Id_Mov).FirstOrDefault<EmplaMovimiento>();
            ViewBag.Partner = (new Partner()).get(((int)emplacamiento.No_Economico).ToString());
            emplacamiento.resp_celular =  this.FormatString(emplacamiento.resp_celular, "(###) ### ####");
            emplacamiento.resp_telefono =  this.FormatString(emplacamiento.resp_telefono, "(###) ### ####");
            return View(emplacamiento);
        }

        // POST: Emplacamientoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Edit/")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Op,No_Economico,Nombre,Nombre2,Paterno,Materno,Propiedad,Operacion,Id_Mov,Id_Marca,Id_Modelo,AutoAno,Motor,No_Serie,Placas,Capacidad,EconoResp,Responsable,TitularEmpla,TitularRentas,TitularCT,TitularOtro,Observaciones,Status,Bloqueado,Protegido,FechaOp,FechaBaja,Id_Usuario,Id_UsuarioEdit,resp_telefono,resp_celular,poliza,tipopoliza,aseguradora,vencepoliza,Modelo,Marca,Message,StatusAction,MessageAction,db,dbv,dbb")] Emplacamiento emplacamiento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Emplacamiento emplacadoActual = db.Emplacamientoes.Find(emplacamiento.Id_Op);
                    if (emplacadoActual.Protegido == true)
                    {
                        return Redirect("/emplacamiento/vinbloqueado/" + emplacamiento.No_Serie);
                    }
                    string pattern = @"^[A-HJ-NPR-Z0-9]{17}$"; //Verificamos que el VIN Tenga el formato correcto.
                    if (!Regex.IsMatch(emplacamiento.No_Serie, pattern))
                    {
                        ViewBag.message = "Error: Vin incorrecto.";
                        return View(emplacamiento);
                    }
                    // Verificamos si el VIN ESTA BLOQUEADO.
                    this.vins = emplacamiento.VINLock();
                    if (this.vins != null && this.vins.Count() > 0 && emplacadoActual.No_Serie != emplacamiento.No_Serie) // Si son distintos no te deje editar.
                    {
                        // Redireccionamos a el bloqueo de emplacamiento con el VIN.
                        this.vins = null; // Reseteamos VINs.
                        return Redirect("/emplacamiento/vinbloqueado/" + emplacamiento.No_Serie);
                    }

                    emplaBitacora emplaBitacora = new emplaBitacora(); // Aqui se guardan todos los cambios cuando, quien y que.
                    // En realidad obtenemos todos los campos editables y los insertamos en el actual...
                    emplaBitacora.id_emplacamiento = emplacadoActual.Id_Op;
                    emplaBitacora.movBy = this.UserName;
                    emplaBitacora.movDate = DateTime.Now;
                    emplaBitacora.action = "EDIT";
                    emplaBitacora.objPre = this.serializeFlat(emplacadoActual);
                    emplacadoActual.Id_Marca = emplacamiento.Id_Marca;
                    emplacadoActual.Id_Modelo = emplacamiento.Id_Modelo;
                    emplacadoActual.AutoAno = emplacamiento.AutoAno;
                    emplacadoActual.Nombre = emplacamiento.Nombre;
                    emplacadoActual.Nombre2 = emplacamiento.Nombre2;
                    emplacadoActual.Paterno = emplacamiento.Paterno;
                    emplacadoActual.Materno = emplacamiento.Materno;
                    emplacadoActual.Motor = emplacamiento.Motor;
                    emplacadoActual.No_Serie = emplacamiento.No_Serie;
                    emplacadoActual.Placas = emplacamiento.Placas;
                    emplacadoActual.Capacidad = emplacamiento.Capacidad;
                    emplacadoActual.EconoResp = emplacamiento.EconoResp;
                    emplacadoActual.Responsable = emplacamiento.Responsable;
                    emplacadoActual.resp_telefono = emplacamiento.resp_telefono;
                    emplacadoActual.resp_celular = emplacamiento.resp_celular;
                    emplacadoActual.poliza = emplacamiento.poliza;
                    emplacadoActual.tipopoliza = emplacamiento.tipopoliza;
                    emplacadoActual.aseguradora = emplacamiento.aseguradora;
                    emplacadoActual.vencepoliza = emplacamiento.vencepoliza;
                    emplacadoActual.Observaciones = emplacamiento.Observaciones;
                    emplacadoActual.Bloqueado = emplacamiento.Bloqueado;
                    emplacadoActual.Protegido = emplacamiento.Protegido;
                    db.Entry(emplacadoActual).State = EntityState.Modified;
                    emplaBitacora.objPos = this.serializeFlat(emplacadoActual);
                    db.emplaBitacoras.Add(emplaBitacora);
                    db.SaveChanges();
                    return Redirect("/emplacamiento/detalle/" + ((int)emplacadoActual.No_Economico));
                }
                return View(emplacamiento);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                Console.WriteLine(e.Message);
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET: Emplacamientoes/Edit/5
        [HttpGet]
        [Route("baja/{id}")]
        public ActionResult Baja(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emplacamiento emplacamiento = db.Emplacamientoes.Find(id);
            if (emplacamiento == null)
            {
                return HttpNotFound();
            }

            // Catalogos.
            ViewBag.marcas = db.EmplaMarcas.OrderByDescending(x => x.Marca).OrderByDescending(x => x.Marca).ToList();
            ViewBag.modelos = db.EmplaModeloes.OrderByDescending(x => x.Modelo).OrderByDescending(x => x.Modelo).ToList();
            ViewBag.movimientos = db.EmplaMovimientos.OrderByDescending(x => x.Movimiento).OrderByDescending(x => x.Movimiento).ToList();
            ViewBag.aseguradoras = db.AseguradorasEmplas.OrderByDescending(x => x.aseguradora).ToList();

            ViewBag.emplaMarca = db.EmplaMarcas.Where(x => x.Id_Marca == emplacamiento.Id_Marca).FirstOrDefault<EmplaMarca>();
            ViewBag.emplaModelo = db.EmplaModeloes.Where(x => x.Id_Modelo == emplacamiento.Id_Modelo).FirstOrDefault<EmplaModelo>();
            ViewBag.emplaMovimiento = db.EmplaMovimientos.Where(x => x.Id_Mov == emplacamiento.Id_Mov).FirstOrDefault<EmplaMovimiento>();
            ViewBag.Partner = (new Partner()).get(((int)emplacamiento.No_Economico).ToString());
            return View(emplacamiento);
        }

        // POST: Emplacamientoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("baja/")]
        [ValidateAntiForgeryToken]
        public ActionResult Baja([Bind(Include = "Id_Op,No_Economico,Nombre,Nombre2,Paterno,Materno,Propiedad,Operacion,Id_Mov,Id_Marca,Id_Modelo,AutoAno,Motor,No_Serie,Placas,Capacidad,EconoResp,Responsable,TitularEmpla,TitularRentas,TitularCT,TitularOtro,Observaciones,Status,Bloqueado,Protegido,FechaOp,FechaBaja,Id_Usuario,Id_UsuarioEdit,resp_telefono,resp_celular,poliza,tipopoliza,aseguradora,vencepoliza,Modelo,Marca,Message,StatusAction,MessageAction,db,dbv,dbb")] Emplacamiento emplacamiento)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    emplaBitacora emplaBitacora = new emplaBitacora(); // Aqui se guardan todos los cambios cuando, quien y que.
                    // En realidad obtenemos todos los campos editables y los insertamos en el actual...
                    Emplacamiento emplacadoActual = db.Emplacamientoes.Find(emplacamiento.Id_Op);
                    emplaBitacora.id_emplacamiento = emplacadoActual.Id_Op;
                    emplaBitacora.movBy = this.UserName;
                    emplaBitacora.movDate = DateTime.Now;
                    emplaBitacora.action = "BAJA";
                    emplaBitacora.objPre = this.serializeFlat(emplacadoActual);
                    emplacadoActual.Id_Mov = emplacamiento.Id_Mov;
                    emplacadoActual.Status = "BAJA";
                    emplacadoActual.Operacion = "BAJA";
                    emplacadoActual.Observaciones = emplacamiento.Observaciones; // Guardamos las observaciones.
                    db.Entry(emplacadoActual).State = EntityState.Modified;
                    emplaBitacora.objPos = this.serializeFlat(emplacadoActual);
                    db.emplaBitacoras.Add(emplaBitacora);
                    db.SaveChanges();
                    return Redirect("/emplacamiento/baja/correcta/" + emplacamiento.Id_Op.ToString().Replace(".0000", ""));
                }
                return View(emplacamiento);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                Console.WriteLine(e.Message);
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("baja/correcta/{id}")]
        public ActionResult BajaCorrecta(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emplacamiento emplacamiento = db.Emplacamientoes.Find(id);
            if (emplacamiento == null)
            {
                return HttpNotFound();
            }
            ViewBag.emplaMarca = db.EmplaMarcas.Where(x => x.Id_Marca == emplacamiento.Id_Marca).FirstOrDefault<EmplaMarca>();
            ViewBag.emplaModelo = db.EmplaModeloes.Where(x => x.Id_Modelo == emplacamiento.Id_Modelo).FirstOrDefault<EmplaModelo>();
            ViewBag.emplaMovimiento = db.EmplaMovimientos.Where(x => x.Id_Mov == emplacamiento.Id_Mov).FirstOrDefault<EmplaMovimiento>();
            ViewBag.Partner = (new Partner()).get(((int)emplacamiento.No_Economico).ToString());
            return View(emplacamiento);
        }

        // GET: Emplacamientoes/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emplacamiento emplacamiento = db.Emplacamientoes.Find(id);
            if (emplacamiento == null)
            {
                return HttpNotFound();
            }
            return View(emplacamiento);
        }

        // POST: Emplacamientoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Emplacamiento emplacamiento = db.Emplacamientoes.Find(id);
            db.Emplacamientoes.Remove(emplacamiento);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult Alta(Emplacamiento emplacamiento)
        {
            MENSAJE message = new MENSAJE();
            Operator _operator = new Operator();
            try
            {

                return Content(this.serialize(""), "application/json");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult propietario(string gafet)
        {
            MENSAJE message = new MENSAJE();
            Operator _operator = new Operator();
            try
            {
                return Content(this.serialize(""), "application/json");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult refrendo(string gafet)
        {

            MENSAJE message = new MENSAJE();
            Operator _operator = new Operator();
            try
            {
                return Content(this.serialize(""), "application/json");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("movimiento/invalido")]
        public ActionResult errormov()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("vinbloqueado/{vin}")]
        public ActionResult vinlock(string vin)
        {
            try
            {
                Emplacamiento empla = new Emplacamiento();
                return View(empla.FindVin(vin));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("protegido/")]
        public ActionResult protegido()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult getModels(string Id_Marca)
        {
            List<EmplaModelo> modelos = db.EmplaModeloes.Where(x => x.Id_Marca == Id_Marca).OrderBy(x => x.Modelo).ToList(); 
            MENSAJE message = new MENSAJE();
            Operator _operator = new Operator();
            try
            {
                return Content(this.serialize(modelos), "application/json");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [SessionFilter("Admin, Practi-Emplacamiento, Practi-Plusvalia")]

        [Route("filter/")]
        public async Task<ActionResult> Filter()
        {
            List<EmplacamientoCarReport> objects = new List<EmplacamientoCarReport> ();
            string query = @"SELECT 
                          [No_Economico]
                          ,[Marca]
	                      ,[Modelo],[Capacidad]
                          ,[AutoAno]
	                      ,Emplacamiento.FechaOp
	                      ,[status]
                      FROM [SindQR].[dbo].[Emplacamiento]
                        left join [dbo].[EmplaMarca] on [dbo].[Emplacamiento].Id_Marca = [dbo].[EmplaMarca].Id_Marca
	                    left join [dbo].[EmplaModelo] on [dbo].[Emplacamiento].Id_Modelo = [dbo].[EmplaModelo].Id_Modelo
	                    where (No_Economico between 1 and 11500 or No_Economico between 80000 and 81500)";
            objects = await db.Database.SqlQuery<EmplacamientoCarReport>(query).ToListAsync();
            ViewBag.cars = objects;
            return View(objects);
        }
    }
}