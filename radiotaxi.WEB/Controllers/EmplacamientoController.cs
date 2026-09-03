using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using radiotaxi.Model;
using radiotaxi.WEB.Helper;
using ZXing.Common;
using ZXing;
using radiotaxi.Model.v2.Partner;
using MigraDoc.DocumentObjectModel.Tables;

namespace radiotaxi.WEB.Controllers
{
    [RoutePrefix("Emplacamientosss")]
    public class EmplacamientoController : Controller
    {
        // GET: Emplacamiento
        public ActionResult Index()
        {
            return View();
        }

        [Route("{gafet}")]
        // GET: Emplacamiento/Details/5
        public ActionResult Details(string gafet)
        {

            //

            return View();
        }

        // GET: Emplacamiento/crear

        [Route("crear")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Emplacamiento/Create
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

        // GET: Emplacamiento/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Emplacamiento/Edit/5
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

        // GET: Emplacamiento/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Emplacamiento/Delete/5
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

        ///////////////////// PDF DE EMPLACAMIENTO ////////////////


        [Route("print/{gafet}")]
        public ActionResult PrintEmplaca(string gafet)
        {
            
            Partner _partner= new Partner();
            _partner = _partner.get(gafet);
            ViewBag.soc = _partner;
            
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
            paragraph3.AddText(("No. DE OFICIO: "   +"\n").ToUpper());
            paragraph3.AddText(("ASUNTO: " + "EMPLACADO").ToUpper());
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
            paragraph4.AddText(("Lic. Manuel Jesus Puerto Castillo \n").ToUpper());
            paragraph4.AddText(("Delegado Del Instituto de Movilidad B.J. \n").ToUpper());
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
            row.Cells[1].AddParagraph("NISSAN VERSA DRIVE");

            row = table.AddRow();
            row.Cells[0].AddParagraph("MODELO:");
            row.Cells[1].AddParagraph("2019");

            row = table.AddRow();
            row.Cells[0].AddParagraph("No. DE MOTOR:");
            row.Cells[1].AddParagraph("HR16366921U");

            row = table.AddRow();
            row.Cells[0].AddParagraph("No. DE SERIE:");
            row.Cells[1].AddParagraph("3N1CN7AD8KK450485");

            row = table.AddRow();
            row.Cells[0].AddParagraph("No. DE PLACAS:");
            row.Cells[1].AddParagraph("B-313-TMK");

            row = table.AddRow();
            row.Cells[0].AddParagraph("CAPACIDAD:");
            row.Cells[1].AddParagraph("4 PASAJEROS MÁS OPERADOR");

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
            paragraph6.AddFormattedText(( _partner.firstName + " " +  _partner.lastNameF + " " + _partner.lastNameM).ToUpper(), TextFormat.Bold);
            paragraph6.AddFormattedText((" socio propietario con numero economico: ").ToUpper());
            paragraph6.AddFormattedText((_partner.partnerReference).ToUpper(), TextFormat.Bold);
            paragraph6.AddFormattedText((" a reserva que esta se tramite en la secretaria de hacienda y credito publico el cambio de propietario. \n \n").ToUpper());
            paragraph6.AddFormattedText((" Pago de tenencia y T/C 2025 (REFRENDO)").ToUpper());
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
            paragraph8.AddFormattedText((" Los socios menores de edad seran representados por su padre o tutor que sean socios activos de este sindicato.").ToUpper());
            //paragraph8.AddFormattedText((" socio propietario con numero economico: ").ToUpper());
            //paragraph8.AddFormattedText(("5670").ToUpper(), TextFormat.Bold);
            //paragraph8.AddFormattedText((" a reserva que esta se tramite en la secretaria de hacienda y credito publico el cambio de propietario. \n \n").ToUpper());
            //paragraph8.AddFormattedText((" Pago de tenencia y T/C 2025 (REFRENDO)").ToUpper());
            paragraph8.Format.Font.Name = "Arial";
            paragraph8.Format.Font.Bold = false;
            paragraph8.Format.SpaceAfter = "0.2cm";
            paragraph8.Format.Font.Color = Colors.Black;









            /*
            // Fecha
            TextFrame ff = section.AddTextFrame();
            ff.Width = "138mm";
            ff.Height = "50mm";
            ff.RelativeVertical = RelativeVertical.Page;
            ff.RelativeHorizontal = RelativeHorizontal.Page;
            ff.Top = "25mm";
            ff.Left = "10mm";
            Paragraph paragraph2 = ff.AddParagraph();
            paragraph2.Format.Font.Size = new Unit(8);
            paragraph2.AddText(("Cancun Quintana Roo a " + DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"))).ToUpper());
            paragraph2.Format.Font.Name = "Impact";
            paragraph2.Format.Font.Bold = false;
            paragraph2.Format.SpaceAfter = "0.2cm";
            paragraph2.Format.Font.Color = Colors.Black;

            // Titulo
            TextFrame tf = section.AddTextFrame();
            tf.Width = "138mm";
            tf.Height = "50mm";
            tf.RelativeVertical = RelativeVertical.Page;
            tf.RelativeHorizontal = RelativeHorizontal.Page;
            tf.Top = "30mm";
            tf.Left = "10mm";
            Paragraph paragraph = tf.AddParagraph();
            paragraph.Format.Font.Size = new Unit(8);
            paragraph.AddText("Permiso".ToUpper());
            paragraph.Format.Font.Name = "Impact";
            paragraph.Format.Font.Bold = false;
            paragraph.Format.SpaceAfter = "0.2cm";
            paragraph.Format.Font.Color = Colors.Black;


            // Parrafo 1
            TextFrame tp = section.AddTextFrame();
            tp.Width = "120mm";
            tp.Height = "50mm";
            tp.RelativeVertical = RelativeVertical.Page;
            tp.RelativeHorizontal = RelativeHorizontal.Page;
            tp.Top = "35mm";
            tp.Left = "10mm";
            Paragraph paragraph3 = tp.AddParagraph();
            paragraph3.Format.Font.Size = new Unit(8);
            paragraph3.AddFormattedText("Autorizo un permiso de 30 días al ");
            //paragraph3.AddFormattedText("C. " + _operator.NOMBRE + " " + _operator.APELLIDOS, TextFormat.Bold);
            paragraph3.AddFormattedText(" teniendo su licencia vigente para prestar el servicio público en el ");
            //paragraph3.AddFormattedText("TAXI " + permissions.taxi, TextFormat.Bold | TextFormat.Underline);
            paragraph3.AddFormattedText(" quien se encuentra bajo el siguiente status y se compromete a respetar los reglamentos internos y a mantener buena conducta dentro y fuera del taxi respetando a todos sus compañeros y al cuerpo de delegados. \n\n");
            paragraph3.AddFormattedText("\t\t\tNO DADO DE ALTA (REGISTRO NUEVO ESQUEMA)\t\n\n", TextFormat.Bold);
            // paragraph3.AddFormattedText("NO DADO DE ALTA", TextFormat.Bold | TextFormat.Underline);
            //paragraph3.AddFormattedText("\t\tCON DEUDA ", TextFormat.Bold);
            paragraph3.Format.Font.Name = "Impact";
            paragraph3.Format.Font.Bold = false;
            paragraph3.Format.SpaceAfter = "0.2cm";
            paragraph3.Format.Font.Color = Colors.Black;

            /*
            // Parrafo 2
            TextFrame tp2 = section.AddTextFrame();
            tp2.Width = "120mm";
            tp2.Height = "50mm";
            tp2.RelativeVertical = RelativeVertical.Page;
            tp2.RelativeHorizontal = RelativeHorizontal.Page;
            tp2.Top = "2.5cm";
            tp2.Left = "1cm";
            Paragraph paragraph4 = tp.AddParagraph();
            paragraph4.Format.Font.Size = new Unit(8);
            paragraph4.AddText("Observación:\n Se acepta y se da por enterado que en caso de accidente, no procederá la reparación del taxi responsable, por la secretaria de auto seguro, firmando de conformidad.\n");
            paragraph4.Format.Font.Name = "Impact";
            paragraph4.Format.Font.Bold = false;
            paragraph4.Format.SpaceAfter = "0.2cm";
            paragraph4.Format.Font.Color = Colors.Black;
           

            // Parrafo 3
            TextFrame ts = section.AddTextFrame();
            ts.Width = "120mm";
            ts.Height = "50mm";
            ts.RelativeVertical = RelativeVertical.Page;
            ts.RelativeHorizontal = RelativeHorizontal.Page;
            ts.Top = "2.5cm";
            ts.Left = "1cm";
            Paragraph paragraph5 = tp.AddParagraph();
            paragraph5.Format.Font.Size = new Unit(8);
            paragraph5.AddFormattedText("Socio: ");
            paragraph5.AddFormattedText(permissions.taxi + "\n", TextFormat.Bold);
            paragraph5.AddFormattedText("C. RUBEN HERVE NOH EUAN\n");
            paragraph5.AddFormattedText("Vence: " + expDate.ToString(new System.Globalization.CultureInfo("es-ES").DateTimeFormat));
            paragraph5.Format.Font.Name = "Impact";
            paragraph5.Format.Font.Bold = false;
            paragraph5.Format.SpaceAfter = "0.2cm";
            paragraph5.Format.Font.Color = Colors.Black;
             */
            /*
            // Footer
            TextFrame tfo = section.AddTextFrame();
            tfo.Width = "40mm";
            tfo.Height = "20mm";
            tfo.RelativeVertical = RelativeVertical.Page;
            tfo.RelativeHorizontal = RelativeHorizontal.Page;
            tfo.Top = "70mm"; // Misma altura
            tfo.Left = "20mm"; // Posición horizontal

            Paragraph paragraph6 = tfo.AddParagraph(); // Se agrega al TextFrame correcto
            paragraph6.Format.Font.Size = new Unit(8);
            paragraph6.AddFormattedText("Prof. Marcos Rivero Sánchez\n", TextFormat.Bold);
            paragraph6.AddFormattedText("SECRETARIO DE AUTO SEGURO");
            paragraph6.Format.Font.Name = "Impact";
            paragraph6.Format.Font.Bold = false;
            paragraph6.Format.SpaceAfter = "0.2cm";
            paragraph6.Format.Font.Color = Colors.Black;

            // Footer 2
            TextFrame tfo2 = section.AddTextFrame();
            tfo2.Width = "40mm";
            tfo2.Height = "20mm";
            tfo2.RelativeVertical = RelativeVertical.Page;
            tfo2.RelativeHorizontal = RelativeHorizontal.Page;
            tfo2.Top = "70mm"; // Misma altura
            tfo2.Left = "80mm"; // Posición horizontal diferente

            Paragraph paragraph7 = tfo2.AddParagraph(); // Se agrega al TextFrame correcto
            paragraph7.Format.Font.Size = new Unit(8);
            paragraph7.AddFormattedText("C. Jesús Tamayo Lora\n", TextFormat.Bold);
            paragraph7.AddFormattedText("SECRETARIO DE TRABAJO");
            paragraph7.Format.Font.Name = "Impact";
            paragraph7.Format.Font.Bold = false;
            paragraph7.Format.SpaceAfter = "0.2cm";
            paragraph7.Format.Font.Color = Colors.Black;

            TextFrame tfo3 = section.AddTextFrame();
            tfo3.Width = "70mm";
            tfo3.Height = "20mm";
            tfo3.RelativeVertical = RelativeVertical.Page;
            tfo3.RelativeHorizontal = RelativeHorizontal.Page;
            tfo3.Top = "90mm"; // Misma altura
            tfo3.Left = "10mm"; // Posición horizontal diferente

            Paragraph paragraph8 = tfo3.AddParagraph(); // Se agrega al TextFrame correcto
            paragraph8.Format.Font.Size = new Unit(8);
            //paragraph8.AddFormattedText("PREGAFET: " + _operator.partnerReference, TextFormat.Bold);
            paragraph8.AddFormattedText("\nVENCE: " + DateTime.Now.AddDays(30).ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES")).ToUpper());
            paragraph8.Format.Font.Name = "Impact";
            paragraph8.Format.Font.Bold = false;
            paragraph8.Format.SpaceAfter = "0.2cm";
            paragraph8.Format.Font.Color = Colors.Black;

            */

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
