using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using radiotaxi.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Row = MigraDoc.DocumentObjectModel.Tables.Row;

namespace radiotaxi.WEB.Controllers
{
    public class fdiController : Controller
    {
        // GET: fdi
        public ActionResult Index()
        {
            return View();
        }

        // GET: fdi/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: fdi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: fdi/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: fdi/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: fdi/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: fdi/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: fdi/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        [Route("fdisoc/print/{eco}")]
        public ActionResult PrintFDIPolicySoc(string eco)
        {
            Partner _Partner = new Partner();
            _Partner = _Partner.get(eco);


            // GENERAMOS EL PDF con los datos.
            Document document = new Document();
            MigraDoc.DocumentObjectModel.Section section = document.AddSection();
            section.PageSetup.Orientation = Orientation.Portrait;
            section.PageSetup.PageHeight = "279.4mm";
            section.PageSetup.PageWidth = "215.9mm";

            // Cabecera
            Image myImage = section.Headers.Primary.AddImage(Server.MapPath("~/Content/images/encabezado.png"));
            myImage.RelativeVertical = RelativeVertical.Page;
            myImage.RelativeHorizontal = RelativeHorizontal.Page;
            myImage.Left = "20mm";
            myImage.Top = "0.15cm";
            myImage.Width = "179.4mm";
            myImage.WrapFormat.Style = WrapStyle.Through;

            // Cabecera
            TextFrame tt = section.AddTextFrame();
            tt.Width = "150mm";
            tt.Height = "20mm";
            tt.RelativeVertical = RelativeVertical.Page;
            tt.RelativeHorizontal = RelativeHorizontal.Page;
            tt.Top = "35mm"; // Misma altura
            tt.Left = "32mm"; // Posición horizontal

            // Titulo
            Paragraph paragraphc = tt.AddParagraph(); // Se agrega al TextFrame correcto
            paragraphc.Format.Font.Size = new Unit(16);
            paragraphc.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
            paragraphc.AddFormattedText("Poliza de fondo de defunción o invalidez\n".ToUpper());
            paragraphc.Format.Font.Name = "Roboto";
            paragraphc.Format.Font.Bold = true;
            paragraphc.Format.SpaceAfter = "0.1cm";
            paragraphc.Format.Font.Color = Colors.Black;

            // Info Socio
            TextFrame ts = section.AddTextFrame();
            ts.Width = "138mm";
            ts.Height = "50mm";
            ts.RelativeVertical = RelativeVertical.Page;
            ts.RelativeHorizontal = RelativeHorizontal.Page;
            ts.Top = "45mm";
            ts.Left = "10mm";
            Paragraph paragraph2 = ts.AddParagraph();
            paragraph2.Format.Font.Size = new Unit(10);
            paragraph2.AddText("Socio: ".ToUpper() + _Partner.Nombre + " " + _Partner.lastNameF + " " +  _Partner.lastNameM + "\n".ToUpper());
            paragraph2.AddText("Domicilio: ".ToUpper()  + "SM " + _Partner.userAddresses.FirstOrDefault().supermanzana + ", MZ " + _Partner.userAddresses.FirstOrDefault().manzana + ", LT " + _Partner.userAddresses.FirstOrDefault().lote + " " + _Partner.userAddresses.FirstOrDefault().street.ToUpper() + ", COLONIA: " + _Partner.userAddresses.FirstOrDefault().colony + ", CP " + _Partner.userAddresses.FirstOrDefault().postalCode.ToUpper());
            paragraph2.Format.Font.Name = "Roboto";
            paragraph2.Format.Font.Bold = false;
            paragraph2.Format.SpaceAfter = "0.2cm";
            paragraph2.Format.Font.Color = Colors.Black;

            // N°
            TextFrame tn = section.AddTextFrame();
            tn.Width = "138mm";
            tn.Height = "50mm";
            tn.RelativeVertical = RelativeVertical.Page;
            tn.RelativeHorizontal = RelativeHorizontal.Page;
            tn.Top = "45mm";
            tn.Left = "165mm";
            Paragraph paragraph3 = tn.AddParagraph();
            paragraph3.Format.Font.Size = new Unit(10);
            paragraph3.AddText("N°: folio".ToUpper());
            paragraph3.Format.Font.Name = "Roboto";
            paragraph3.Format.Font.Bold = false;
            paragraph3.Format.SpaceAfter = "0.2cm";
            paragraph3.Format.Font.Color = Colors.Black;

            // Crear un TextFrame que contenga la tabla
            TextFrame tfTable = section.AddTextFrame();
            tfTable.Width = "190mm"; // o el ancho que necesites
            tfTable.Height = "60mm"; // opcional, puedes ajustar si quieres limitar
            tfTable.Top = "60mm"; // posición vertical
            tfTable.Left = "12.95mm"; // posición horizontal
            tfTable.RelativeVertical = RelativeVertical.Page;
            tfTable.RelativeHorizontal = RelativeHorizontal.Page;

            // Crear tabla dentro del TextFrame
            Table table = new Table();
            table.Borders.Width = 0.75;
            table.Format.Font.Name = "Roboto";
            table.Format.Font.Size = 9;

            // Definir columnas
            table.AddColumn(Unit.FromMillimeter(40));  // Col 1
            table.AddColumn(Unit.FromMillimeter(55));  // Col 2
            table.AddColumn(Unit.FromMillimeter(55));  // Col 3
            table.AddColumn(Unit.FromMillimeter(40));  // Col 4

            void CenterRow(Row row)
            {
                foreach (Cell cell in row.Cells)
                {
                    cell.Format.Alignment = ParagraphAlignment.Center;
                    cell.VerticalAlignment = VerticalAlignment.Center;
                }
            }

            // --------- Fila 1: Encabezados ----------
            Row header1 = table.AddRow();
            header1.Height = Unit.FromMillimeter(12);
            header1.HeightRule = RowHeightRule.Exactly;
            header1.Cells[0].AddParagraph("NÚM. DE PÓLIZA");
            header1.Cells[1].AddParagraph("FECHA DE INICIO DE VIGENCIA");
            header1.Cells[2].AddParagraph("PLAZO DEL FONDO");
            header1.Cells[3].AddParagraph("FRECUENCIA DE PAGO DE CUOTAS");
            CenterRow(header1);

            // --------- Fila 2: Valores ----------
            Row row1 = table.AddRow();
            row1.Height = Unit.FromMillimeter(17);
            row1.HeightRule = RowHeightRule.Exactly;
            row1.Cells[0].AddParagraph("9569");
            row1.Cells[1].AddParagraph("1 DE OCTUBRE DE 1995");
            row1.Cells[2].AddParagraph("VIGENCIA SINDICAL");
            row1.Cells[3].AddParagraph("POR DEFUNCIÓN O INVALIDEZ\nDE SOCIO CONCESIONARIO");
            CenterRow(row1);

            // --------- Fila 3: Encabezados 2 ----------
            Row header2 = table.AddRow();
            header2.Height = Unit.FromMillimeter(12);
            header2.HeightRule = RowHeightRule.Exactly;
            header2.Cells[0].AddParagraph("APLICACIÓN");
            header2.Cells[1].AddParagraph("OPCIÓN DE INCREMENTO");
            header2.Cells[2].MergeRight = 1;
            header2.Cells[2].AddParagraph("PRIMA INICIAL");
            CenterRow(header2);

            // --------- Fila 4: Valores ----------
            Row row2 = table.AddRow();
            row2.Height = Unit.FromMillimeter(17);
            row2.HeightRule = RowHeightRule.Exactly;
            row2.Cells[0].AddParagraph("DIRECTA");
            row2.Cells[1].AddParagraph("ABIERTA");
            row2.Cells[2].AddParagraph("BÁSICA\nCOMPLEMENTARIA\nPRIMA TOTAL");
            row2.Cells[3].AddParagraph("300.00\n100.00\n");
            CenterRow(row2);

            // Agregar tabla al TextFrame
            tfTable.Elements.Add(table);


            // Info Asegurado
            TextFrame ta = section.AddTextFrame();
            ta.Width = "138mm";
            ta.Height = "50mm";
            ta.RelativeVertical = RelativeVertical.Page;
            ta.RelativeHorizontal = RelativeHorizontal.Page;
            ta.Top = "125mm";
            ta.Left = "10mm";
            Paragraph paragraph4 = ta.AddParagraph();
            paragraph4.Format.Font.Size = new Unit(10);
            paragraph4.AddText("Asegurado(a): ".ToUpper() + _Partner.Nombre + " " + _Partner.lastNameF + " " + _Partner.lastNameM + "\n".ToUpper());
            paragraph4.Format.Font.Name = "Roboto";
            paragraph4.Format.Font.Bold = false;
            paragraph4.Format.SpaceAfter = "0.2cm";
            paragraph4.Format.Font.Color = Colors.Black;

            TextFrame ta2 = section.AddTextFrame();
            ta2.Width = "138mm";
            ta2.Height = "50mm";
            ta2.RelativeVertical = RelativeVertical.Page;
            ta2.RelativeHorizontal = RelativeHorizontal.Page;
            ta2.Top = "130mm";
            ta2.Left = "10mm";
            Paragraph paragraph5 = ta2.AddParagraph();
            paragraph5.Format.Font.Size = new Unit(10);
            paragraph5.AddText("Acta y fecha de nacimiento: ".ToUpper() + _Partner.birthDate.ToString("dd/MM/yyyy"));
            paragraph5.Format.Font.Name = "Roboto";
            paragraph5.Format.Font.Bold = false;
            paragraph5.Format.SpaceAfter = "0.2cm";
            paragraph5.Format.Font.Color = Colors.Black;

            TextFrame ta3 = section.AddTextFrame();
            ta3.Width = "138mm";
            ta3.Height = "50mm";
            ta3.RelativeVertical = RelativeVertical.Page;
            ta3.RelativeHorizontal = RelativeHorizontal.Page;
            ta3.Top = "130mm";
            ta3.Left = "115mm";
            Paragraph paragraph6 = ta3.AddParagraph();
            paragraph6.Format.Font.Size = new Unit(10);
            paragraph6.AddText("Lugar de nacimiento: ".ToUpper() + _Partner.birthPlace.ToUpper());
            paragraph6.Format.Font.Name = "Roboto";
            paragraph6.Format.Font.Bold = false;
            paragraph6.Format.SpaceAfter = "0.2cm";
            paragraph6.Format.Font.Color = Colors.Black;

            TextFrame ta4 = section.AddTextFrame();
            ta4.Width = "138mm";
            ta4.Height = "50mm";
            ta4.RelativeVertical = RelativeVertical.Page;
            ta4.RelativeHorizontal = RelativeHorizontal.Page;
            ta4.Top = "138mm";
            ta4.Left = "10mm";
            Paragraph paragraph7 = ta4.AddParagraph();
            paragraph7.Format.Font.Size = new Unit(10);
            paragraph7.AddText("Cobertura inicial: $500,000.00 MXN".ToUpper());
            paragraph7.Format.Font.Name = "Roboto";
            paragraph7.Format.Font.Bold = false;
            paragraph7.Format.SpaceAfter = "0.2cm";
            paragraph7.Format.Font.Color = Colors.Black;

            TextFrame ta5 = section.AddTextFrame();
            ta5.Width = "138mm";
            ta5.Height = "50mm";
            ta5.RelativeVertical = RelativeVertical.Page;
            ta5.RelativeHorizontal = RelativeHorizontal.Page;
            ta5.Top = "138mm";
            ta5.Left = "90mm";
            Paragraph paragraph8 = ta5.AddParagraph();
            paragraph8.Format.Font.Size = new Unit(10);
            paragraph8.AddText("Con letra: quinientos mil pesos 00/100 m.n.: ".ToUpper());
            paragraph8.Format.Font.Name = "Roboto";
            paragraph8.Format.Font.Bold = false;
            paragraph8.Format.SpaceAfter = "0.2cm";
            paragraph8.Format.Font.Color = Colors.Black;


            // Tabla Beneficiarios

            // Crear un TextFrame que contenga la tabla
            TextFrame tfTable2 = section.AddTextFrame();
            tfTable2.Width = "190mm"; // o el ancho que necesites
            tfTable2.Height = "60mm"; // opcional, puedes ajustar si quieres limitar
            tfTable2.Top = "145mm"; // posición vertical
            tfTable2.Left = "25mm"; // posición horizontal
            tfTable2.RelativeVertical = RelativeVertical.Page;
            tfTable2.RelativeHorizontal = RelativeHorizontal.Page;


            // Crear la tabla de beneficiarios
            Table table2 = new Table();
            table2.Borders.Width = 0.75;
            table2.Format.Font.Name = "Roboto";
            table2.Format.Font.Size = 9;

            // Definir columnas (aproximadamente 70%, 15%, 15%)
            table2.AddColumn(Unit.FromMillimeter(110)); // Beneficiario
            table2.AddColumn(Unit.FromMillimeter(30));  // Parentesco
            table2.AddColumn(Unit.FromMillimeter(30));  // Porcentaje

            void CenterRow2(Row row)
            {
                foreach (Cell cell in row.Cells)
                {
                    cell.Format.Alignment = ParagraphAlignment.Center;
                    cell.VerticalAlignment = VerticalAlignment.Center;
                }
            }

            // Fila de encabezados
            Row header = table2.AddRow();
            header.Height = Unit.FromMillimeter(10);
            header.HeightRule = RowHeightRule.Exactly;
            header.Cells[0].AddParagraph("BENEFICIARIO(A)(S)");
            header.Cells[1].AddParagraph("PARENTESCO");
            header.Cells[2].AddParagraph("PORCENTAJE");
            CenterRow2(header);

            // Fila 1
            Row row5 = table2.AddRow();
            row5.Height = Unit.FromMillimeter(10);
            row5.HeightRule = RowHeightRule.Exactly;
            row5.Cells[0].AddParagraph("EDITH ROGIO CHAN CHAN");
            row5.Cells[1].AddParagraph("HIJA");
            row5.Cells[2].AddParagraph("34.00%");
            CenterRow2(row5);

            // Fila 2
            Row row6 = table2.AddRow();
            row6.Height = Unit.FromMillimeter(10);
            row6.HeightRule = RowHeightRule.Exactly;
            row6.Cells[0].AddParagraph("EDWIN SAUL CHAN CHAN");
            row6.Cells[1].AddParagraph("HIJO");
            row6.Cells[2].AddParagraph("33.00%");
            CenterRow2(row6);

            // Fila 3
            Row row7 = table2.AddRow();
            row7.Height = Unit.FromMillimeter(10);
            row7.HeightRule = RowHeightRule.Exactly;
            row7.Cells[0].AddParagraph("CESAR ISAI CHAN CHAN");
            row7.Cells[1].AddParagraph("HIJO");
            row7.Cells[2].AddParagraph("33.00%");
            CenterRow2(row7);

            // Agregar tabla al TextFrame
            tfTable2.Elements.Add(table2);

            // === 1. Crear el TextFrame para posicionar la tabla ===
            TextFrame tfFirmas = section.AddTextFrame();
            tfFirmas.Width = "205.9mm";               // Ancho de la tabla
            tfFirmas.Height = "80mm";               // Altura estimada
            tfFirmas.RelativeVertical = RelativeVertical.Page;
            tfFirmas.RelativeHorizontal = RelativeHorizontal.Page;
            tfFirmas.Top = "220mm";                 // Ajusta esta altura vertical
            tfFirmas.Left = "20mm";                 // Ajusta esta posición horizontal

            // === 2. Crear la tabla dentro del TextFrame ===
            Table firmasTable = new Table();
            firmasTable.Borders.Width = 0;
            firmasTable.Format.Font.Name = "Roboto";
            firmasTable.Format.Font.Size = 9;

            // 3 columnas
            for (int i = 0; i < 3; i++)
                firmasTable.AddColumn(Unit.FromMillimeter(63)); // 190mm / 3 ≈ 63mm

            // Función para agregar firma
            void AgregarFirma(Cell celda, string nombre, string cargo)
            {
                Paragraph p1 = celda.AddParagraph("_______________________________________");
                p1.Format.Alignment = ParagraphAlignment.Center;

                Paragraph p2 = celda.AddParagraph(nombre);
                p2.Format.Alignment = ParagraphAlignment.Center;

                Paragraph p3 = celda.AddParagraph(cargo);
                p3.Format.Alignment = ParagraphAlignment.Center;
                p3.Format.Font.Bold = true;
            }

            // Fila 1
            Row fila1 = firmasTable.AddRow();
            fila1.Height = Unit.FromMillimeter(20);
            AgregarFirma(fila1.Cells[1], "C. " + _Partner.Nombre + " " + _Partner.lastNameF + " " + _Partner.lastNameM, "SOCIO CONCESIONARIO");

            // Fila 2
            Row fila2 = firmasTable.AddRow();
            fila2.Height = Unit.FromMillimeter(20);
            AgregarFirma(fila2.Cells[0], "C. JUSTIANO KAUIL HERRERA", "PRESIDENTE DEL FDI");
            AgregarFirma(fila2.Cells[2], "C. RAUL ARMANDO LARA QUIJANO", "SECRETARIO DEL FDI");

            // Fila 3
            Row fila3 = firmasTable.AddRow();
            fila3.Height = Unit.FromMillimeter(20);
            AgregarFirma(fila3.Cells[0], "C. ROBERTO RODRIGUEZ CASTILLO", "TESORERO DEL FDI");
            AgregarFirma(fila3.Cells[2], "C. RUBEN ANTONIO CARRILLO BUENFIL", "SECRETARIO GENERAL");

            // === 3. Agregar tabla al TextFrame ===
            tfFirmas.Elements.Add(firmasTable);


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
        [Route("fdiop/print/{gafet}")]
        public ActionResult PrintFDIPolicyOP(string gafet)
        {
            Operator _Operator = new Operator();
            _Operator = _Operator.get(gafet);


            // GENERAMOS EL PDF con los datos.
            Document document = new Document();
            MigraDoc.DocumentObjectModel.Section section = document.AddSection();
            section.PageSetup.Orientation = Orientation.Portrait;
            section.PageSetup.PageHeight = "279.4mm";
            section.PageSetup.PageWidth = "215.9mm";

            // Cabecera
            Image myImage = section.Headers.Primary.AddImage(Server.MapPath("~/Content/images/encabezado.png"));
            myImage.RelativeVertical = RelativeVertical.Page;
            myImage.RelativeHorizontal = RelativeHorizontal.Page;
            myImage.Left = "20mm";
            myImage.Top = "0.15cm";
            myImage.Width = "179.4mm";
            myImage.WrapFormat.Style = WrapStyle.Through;

            // Cabecera
            TextFrame tt = section.AddTextFrame();
            tt.Width = "150mm";
            tt.Height = "20mm";
            tt.RelativeVertical = RelativeVertical.Page;
            tt.RelativeHorizontal = RelativeHorizontal.Page;
            tt.Top = "35mm"; // Misma altura
            tt.Left = "32mm"; // Posición horizontal

            // Titulo
            Paragraph paragraphc = tt.AddParagraph(); // Se agrega al TextFrame correcto
            paragraphc.Format.Font.Size = new Unit(16);
            paragraphc.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
            paragraphc.AddFormattedText("Poliza de fondo de defunción o invalidez\n".ToUpper());
            paragraphc.Format.Font.Name = "Roboto";
            paragraphc.Format.Font.Bold = true;
            paragraphc.Format.SpaceAfter = "0.1cm";
            paragraphc.Format.Font.Color = Colors.Black;

            // Info Socio
            TextFrame ts = section.AddTextFrame();
            ts.Width = "138mm";
            ts.Height = "50mm";
            ts.RelativeVertical = RelativeVertical.Page;
            ts.RelativeHorizontal = RelativeHorizontal.Page;
            ts.Top = "45mm";
            ts.Left = "10mm";
            Paragraph paragraph2 = ts.AddParagraph();
            paragraph2.Format.Font.Size = new Unit(10);
            paragraph2.AddText("Socio: ".ToUpper() + _Operator.NOMBRE + " " + _Operator.lastNameF + " " + _Operator.lastNameM + "\n".ToUpper());
            paragraph2.AddText("Domicilio: ".ToUpper() + "SM " + _Operator.userAddresses.FirstOrDefault().supermanzana + ", MZ " + _Operator.userAddresses.FirstOrDefault().manzana + ", LT " + _Operator.userAddresses.FirstOrDefault().lote + " " + _Operator.userAddresses.FirstOrDefault().street.ToUpper() + ", COLONIA: " + _Operator.userAddresses.FirstOrDefault().colony.ToUpper() + ", CP " + _Operator.userAddresses.FirstOrDefault().postalCode.ToUpper());
            paragraph2.Format.Font.Name = "Roboto";
            paragraph2.Format.Font.Bold = false;
            paragraph2.Format.SpaceAfter = "0.2cm";
            paragraph2.Format.Font.Color = Colors.Black;

            // N°
            TextFrame tn = section.AddTextFrame();
            tn.Width = "138mm";
            tn.Height = "50mm";
            tn.RelativeVertical = RelativeVertical.Page;
            tn.RelativeHorizontal = RelativeHorizontal.Page;
            tn.Top = "45mm";
            tn.Left = "165mm";
            Paragraph paragraph3 = tn.AddParagraph();
            paragraph3.Format.Font.Size = new Unit(10);
            paragraph3.AddText("N°: folio".ToUpper());
            paragraph3.Format.Font.Name = "Roboto";
            paragraph3.Format.Font.Bold = false;
            paragraph3.Format.SpaceAfter = "0.2cm";
            paragraph3.Format.Font.Color = Colors.Black;

            // Crear un TextFrame que contenga la tabla
            TextFrame tfTable = section.AddTextFrame();
            tfTable.Width = "190mm"; // o el ancho que necesites
            tfTable.Height = "60mm"; // opcional, puedes ajustar si quieres limitar
            tfTable.Top = "60mm"; // posición vertical
            tfTable.Left = "12.95mm"; // posición horizontal
            tfTable.RelativeVertical = RelativeVertical.Page;
            tfTable.RelativeHorizontal = RelativeHorizontal.Page;

            // Crear tabla dentro del TextFrame
            Table table = new Table();
            table.Borders.Width = 0.75;
            table.Format.Font.Name = "Roboto";
            table.Format.Font.Size = 9;

            // Definir columnas
            table.AddColumn(Unit.FromMillimeter(40));  // Col 1
            table.AddColumn(Unit.FromMillimeter(55));  // Col 2
            table.AddColumn(Unit.FromMillimeter(55));  // Col 3
            table.AddColumn(Unit.FromMillimeter(40));  // Col 4

            void CenterRow(Row row)
            {
                foreach (Cell cell in row.Cells)
                {
                    cell.Format.Alignment = ParagraphAlignment.Center;
                    cell.VerticalAlignment = VerticalAlignment.Center;
                }
            }

            // --------- Fila 1: Encabezados ----------
            Row header1 = table.AddRow();
            header1.Height = Unit.FromMillimeter(12);
            header1.HeightRule = RowHeightRule.Exactly;
            header1.Cells[0].AddParagraph("NÚM. DE PÓLIZA");
            header1.Cells[1].AddParagraph("FECHA DE INICIO DE VIGENCIA");
            header1.Cells[2].AddParagraph("PLAZO DEL FONDO");
            header1.Cells[3].AddParagraph("FRECUENCIA DE PAGO DE CUOTAS");
            CenterRow(header1);

            // --------- Fila 2: Valores ----------
            Row row1 = table.AddRow();
            row1.Height = Unit.FromMillimeter(17);
            row1.HeightRule = RowHeightRule.Exactly;
            row1.Cells[0].AddParagraph("9569");
            row1.Cells[1].AddParagraph("1 DE OCTUBRE DE 1995");
            row1.Cells[2].AddParagraph("VIGENCIA SINDICAL");
            row1.Cells[3].AddParagraph("POR DEFUNCIÓN O INVALIDEZ\nDE SOCIO CONCESIONARIO");
            CenterRow(row1);

            // --------- Fila 3: Encabezados 2 ----------
            Row header2 = table.AddRow();
            header2.Height = Unit.FromMillimeter(12);
            header2.HeightRule = RowHeightRule.Exactly;
            header2.Cells[0].AddParagraph("APLICACIÓN");
            header2.Cells[1].AddParagraph("OPCIÓN DE INCREMENTO");
            header2.Cells[2].MergeRight = 1;
            header2.Cells[2].AddParagraph("PRIMA INICIAL");
            CenterRow(header2);

            // --------- Fila 4: Valores ----------
            Row row2 = table.AddRow();
            row2.Height = Unit.FromMillimeter(17);
            row2.HeightRule = RowHeightRule.Exactly;
            row2.Cells[0].AddParagraph("DIRECTA");
            row2.Cells[1].AddParagraph("ABIERTA");
            row2.Cells[2].AddParagraph("BÁSICA\nCOMPLEMENTARIA\nPRIMA TOTAL");
            row2.Cells[3].AddParagraph("300.00\n100.00\n");
            CenterRow(row2);

            // Agregar tabla al TextFrame
            tfTable.Elements.Add(table);


            // Info Asegurado
            TextFrame ta = section.AddTextFrame();
            ta.Width = "138mm";
            ta.Height = "50mm";
            ta.RelativeVertical = RelativeVertical.Page;
            ta.RelativeHorizontal = RelativeHorizontal.Page;
            ta.Top = "125mm";
            ta.Left = "10mm";
            Paragraph paragraph4 = ta.AddParagraph();
            paragraph4.Format.Font.Size = new Unit(10);
            paragraph4.AddText("Asegurado(a): ".ToUpper() + _Operator.NOMBRE + " " + _Operator.lastNameF + " " + _Operator.lastNameM + "\n".ToUpper());
            paragraph4.Format.Font.Name = "Roboto";
            paragraph4.Format.Font.Bold = false;
            paragraph4.Format.SpaceAfter = "0.2cm";
            paragraph4.Format.Font.Color = Colors.Black;

            TextFrame ta2 = section.AddTextFrame();
            ta2.Width = "138mm";
            ta2.Height = "50mm";
            ta2.RelativeVertical = RelativeVertical.Page;
            ta2.RelativeHorizontal = RelativeHorizontal.Page;
            ta2.Top = "130mm";
            ta2.Left = "10mm";
            Paragraph paragraph5 = ta2.AddParagraph();
            paragraph5.Format.Font.Size = new Unit(10);
            paragraph5.AddText("Acta y fecha de nacimiento: ".ToUpper() + _Operator.birthDate?.ToString("dd/MM/yyyy"));
            paragraph5.Format.Font.Name = "Roboto";
            paragraph5.Format.Font.Bold = false;
            paragraph5.Format.SpaceAfter = "0.2cm";
            paragraph5.Format.Font.Color = Colors.Black;

            TextFrame ta3 = section.AddTextFrame();
            ta3.Width = "138mm";
            ta3.Height = "50mm";
            ta3.RelativeVertical = RelativeVertical.Page;
            ta3.RelativeHorizontal = RelativeHorizontal.Page;
            ta3.Top = "130mm";
            ta3.Left = "115mm";
            Paragraph paragraph6 = ta3.AddParagraph();
            paragraph6.Format.Font.Size = new Unit(10);
            paragraph6.AddText("Lugar de nacimiento: ".ToUpper() + _Operator.birthPlace.ToUpper());
            paragraph6.Format.Font.Name = "Roboto";
            paragraph6.Format.Font.Bold = false;
            paragraph6.Format.SpaceAfter = "0.2cm";
            paragraph6.Format.Font.Color = Colors.Black;

            TextFrame ta4 = section.AddTextFrame();
            ta4.Width = "138mm";
            ta4.Height = "50mm";
            ta4.RelativeVertical = RelativeVertical.Page;
            ta4.RelativeHorizontal = RelativeHorizontal.Page;
            ta4.Top = "138mm";
            ta4.Left = "10mm";
            Paragraph paragraph7 = ta4.AddParagraph();
            paragraph7.Format.Font.Size = new Unit(10);
            paragraph7.AddText("Cobertura inicial: $200,000.00 MXN".ToUpper());
            paragraph7.Format.Font.Name = "Roboto";
            paragraph7.Format.Font.Bold = false;
            paragraph7.Format.SpaceAfter = "0.2cm";
            paragraph7.Format.Font.Color = Colors.Black;

            TextFrame ta5 = section.AddTextFrame();
            ta5.Width = "138mm";
            ta5.Height = "50mm";
            ta5.RelativeVertical = RelativeVertical.Page;
            ta5.RelativeHorizontal = RelativeHorizontal.Page;
            ta5.Top = "138mm";
            ta5.Left = "90mm";
            Paragraph paragraph8 = ta5.AddParagraph();
            paragraph8.Format.Font.Size = new Unit(10);
            paragraph8.AddText("Con letra: doscientos mil pesos 00/100 m.n.: ".ToUpper());
            paragraph8.Format.Font.Name = "Roboto";
            paragraph8.Format.Font.Bold = false;
            paragraph8.Format.SpaceAfter = "0.2cm";
            paragraph8.Format.Font.Color = Colors.Black;


            // Tabla Beneficiarios

            // Crear un TextFrame que contenga la tabla
            TextFrame tfTable2 = section.AddTextFrame();
            tfTable2.Width = "190mm"; // o el ancho que necesites
            tfTable2.Height = "60mm"; // opcional, puedes ajustar si quieres limitar
            tfTable2.Top = "145mm"; // posición vertical
            tfTable2.Left = "25mm"; // posición horizontal
            tfTable2.RelativeVertical = RelativeVertical.Page;
            tfTable2.RelativeHorizontal = RelativeHorizontal.Page;


            // Crear la tabla de beneficiarios
            Table table2 = new Table();
            table2.Borders.Width = 0.75;
            table2.Format.Font.Name = "Roboto";
            table2.Format.Font.Size = 9;

            // Definir columnas (aproximadamente 70%, 15%, 15%)
            table2.AddColumn(Unit.FromMillimeter(110)); // Beneficiario
            table2.AddColumn(Unit.FromMillimeter(30));  // Parentesco
            table2.AddColumn(Unit.FromMillimeter(30));  // Porcentaje

            void CenterRow2(Row row)
            {
                foreach (Cell cell in row.Cells)
                {
                    cell.Format.Alignment = ParagraphAlignment.Center;
                    cell.VerticalAlignment = VerticalAlignment.Center;
                }
            }

            // Fila de encabezados
            Row header = table2.AddRow();
            header.Height = Unit.FromMillimeter(10);
            header.HeightRule = RowHeightRule.Exactly;
            header.Cells[0].AddParagraph("BENEFICIARIO(A)(S)");
            header.Cells[1].AddParagraph("PARENTESCO");
            header.Cells[2].AddParagraph("PORCENTAJE");
            CenterRow2(header);

            // Fila 1
            Row row5 = table2.AddRow();
            row5.Height = Unit.FromMillimeter(10);
            row5.HeightRule = RowHeightRule.Exactly;
            row5.Cells[0].AddParagraph("EDITH ROGIO CHAN CHAN");
            row5.Cells[1].AddParagraph("HIJA");
            row5.Cells[2].AddParagraph("34.00%");
            CenterRow2(row5);

            // Fila 2
            Row row6 = table2.AddRow();
            row6.Height = Unit.FromMillimeter(10);
            row6.HeightRule = RowHeightRule.Exactly;
            row6.Cells[0].AddParagraph("EDWIN SAUL CHAN CHAN");
            row6.Cells[1].AddParagraph("HIJO");
            row6.Cells[2].AddParagraph("33.00%");
            CenterRow2(row6);

            // Fila 3
            Row row7 = table2.AddRow();
            row7.Height = Unit.FromMillimeter(10);
            row7.HeightRule = RowHeightRule.Exactly;
            row7.Cells[0].AddParagraph("CESAR ISAI CHAN CHAN");
            row7.Cells[1].AddParagraph("HIJO");
            row7.Cells[2].AddParagraph("33.00%");
            CenterRow2(row7);

            // Agregar tabla al TextFrame
            tfTable2.Elements.Add(table2);

            // === 1. Crear el TextFrame para posicionar la tabla ===
            TextFrame tfFirmas = section.AddTextFrame();
            tfFirmas.Width = "205.9mm";               // Ancho de la tabla
            tfFirmas.Height = "80mm";               // Altura estimada
            tfFirmas.RelativeVertical = RelativeVertical.Page;
            tfFirmas.RelativeHorizontal = RelativeHorizontal.Page;
            tfFirmas.Top = "220mm";                 // Ajusta esta altura vertical
            tfFirmas.Left = "20mm";                 // Ajusta esta posición horizontal

            // === 2. Crear la tabla dentro del TextFrame ===
            Table firmasTable = new Table();
            firmasTable.Borders.Width = 0;
            firmasTable.Format.Font.Name = "Roboto";
            firmasTable.Format.Font.Size = 9;

            // 3 columnas
            for (int i = 0; i < 3; i++)
                firmasTable.AddColumn(Unit.FromMillimeter(63)); // 190mm / 3 ≈ 63mm

            // Función para agregar firma
            void AgregarFirma(Cell celda, string nombre, string cargo)
            {
                Paragraph p1 = celda.AddParagraph("_______________________________________");
                p1.Format.Alignment = ParagraphAlignment.Center;

                Paragraph p2 = celda.AddParagraph(nombre);
                p2.Format.Alignment = ParagraphAlignment.Center;

                Paragraph p3 = celda.AddParagraph(cargo);
                p3.Format.Alignment = ParagraphAlignment.Center;
                p3.Format.Font.Bold = true;
            }

            // Fila 1
            Row fila1 = firmasTable.AddRow();
            fila1.Height = Unit.FromMillimeter(20);
            AgregarFirma(fila1.Cells[1], "C. " + _Operator.NOMBRE + " " + _Operator.lastNameF+ " " + _Operator.lastNameM, "SOCIO OPERADOR");

            // Fila 2
            Row fila2 = firmasTable.AddRow();
            fila2.Height = Unit.FromMillimeter(20);
            AgregarFirma(fila2.Cells[0], "C. JUSTIANO KAUIL HERRERA", "PRESIDENTE DEL FDI");
            AgregarFirma(fila2.Cells[2], "C. RAUL ARMANDO LARA QUIJANO", "SECRETARIO DEL FDI");

            // Fila 3
            Row fila3 = firmasTable.AddRow();
            fila3.Height = Unit.FromMillimeter(20);
            AgregarFirma(fila3.Cells[0], "C. ROBERTO RODRIGUEZ CASTILLO", "TESORERO DEL FDI");
            AgregarFirma(fila3.Cells[2], "C. RUBEN ANTONIO CARRILLO BUENFIL", "SECRETARIO GENERAL");

            // === 3. Agregar tabla al TextFrame ===
            tfFirmas.Elements.Add(firmasTable);


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
    }
}
