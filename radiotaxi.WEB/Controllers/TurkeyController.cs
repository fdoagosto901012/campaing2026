using radiotaxi.Model;
using radiotaxi.Model.v2.Operator;
using radiotaxi.WEB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// Librerias para generar el PDF
using ZXing.Common;
using ZXing;
using ZXing.QrCode;
using PdfSharp.Drawing;
using System.Data.Entity.Core.Metadata.Edm;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using System.IO;
using MigraDoc.Rendering;
using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI.WebControls;
using Orientation = MigraDoc.DocumentObjectModel.Orientation;
using Image = MigraDoc.DocumentObjectModel.Shapes.Image;
using Unit = MigraDoc.DocumentObjectModel.Unit;
using System.Globalization;
using System.Drawing.Imaging;
using Amazon.CognitoIdentity.Model;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;

namespace radiotaxi.WEB.Controllers
{

    [RoutePrefix("Turkey")]
    public class TurkeyController : webBaseController
    {
        // GET: Turkey
        public ActionResult Index()
        {
            return View();
        }

        // GET: Turkey/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Turkey/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Turkey/Create
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

        // GET: Turkey/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Turkey/Edit/5
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

        // GET: Turkey/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Turkey/Delete/5
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

        // POST: Turkey/Delete/5
        [HttpGet]
        [Route("print/{reference}")]
        public ActionResult print(string reference)
        {
            string cajera = this.User.firstName + " " + this.User.lastName;

            // OBTENEMOS LA INFORMACION DEL OPERADOR / SOCIO.
            int userId = 0;
            string firstName = "";
            string lastNameF = "";
            string lastNameM = "";
            string partnerReference = "";
            string status = "B";

            if (reference.ToUpper().Contains("OP-"))
            {
                Operator _operator = new Operator();
                _operator = _operator.get(reference);
                partnerReference = _operator.partnerReference;
                userId = _operator.userId;
                firstName = _operator.firstName;
                lastNameF = _operator.lastNameF;
                lastNameM = _operator.lastNameM;
                status = _operator.STATUS;
            }
            else {
                Partner _Partner = new Partner();
                _Partner = _Partner.get(reference);
                partnerReference = _Partner.partnerReference;
                userId = _Partner.userId;
                firstName = _Partner.firstName;
                lastNameF = _Partner.lastNameF;
                lastNameM = _Partner.lastNameM;
                status = _Partner.Status;
            }

            if (status == "B")
            {
                return View("error");
            }

            // DEUDAS
            Sale sale = new Sale(this.email);
            List<caja_cargos_DTO> cargos = sale.debs(reference, reference);
            ViewBag.debs = cargos == null ? new List<caja_cargos_DTO>() : cargos;
            decimal amountdebs = 0;
            foreach (caja_cargos_DTO deb in cargos ?? new List<caja_cargos_DTO>())
            {
                if (!partnerReference.ToUpper().Contains("OP") && deb.CLAVE != "0")
                {
                    amountdebs += (deb.DEBE * deb.PRECIO);
                }
                else if (partnerReference.ToUpper().Contains("OP"))
                {
                    amountdebs += (deb.DEBE * deb.PRECIO);
                }
            }
            ViewBag.amountdebs = amountdebs;

            if (amountdebs > maxdeb && userId != 0)
            {
                return View();
            }

            Christmasgift gift = new Christmasgift();
            gift = gift.get(userId);

            if (gift == null)
            {
                gift = new Christmasgift();
                gift.userid = userId;
                gift.membership_type = "";
                gift.note = "";
                gift.ChristmasgiftPrinteds.Add(
                    new ChristmasgiftPrinted()
                    {
                        PrintedBy = this.User.firstName + " " + this.User.lastName,
                        PrintedDate = DateTime.Now,
                        membership_type = "",
                        note = ""
                    }
                );
                gift.save();
            }
            else {
                gift.ChristmasgiftPrinteds.Add(
                        new ChristmasgiftPrinted()
                        {
                            PrintedBy = this.User.firstName + " " + this.User.lastName,
                            PrintedDate = DateTime.Now,
                            membership_type = "",
                            note = ""
                        }
                );
                gift.update();
            }

            string folio = gift.id.ToString();
            DateTime date = DateTime.Now;
            // QR
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 270,
                    Width = 270,
                    Margin = 1
                }
            };
            // Codigo de barras.
            BarcodeWriter writer2 = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    PureBarcode = true,
                    Height = 100,
                    Width = 270,
                    Margin = 1,

                }
            };
            var result = writer.Write(folio);
            var result2 = writer2.Write(folio);
            System.Drawing.Bitmap qrCodeImage = new System.Drawing.Bitmap(result); // CODIGO QR
            System.Drawing.Bitmap BarCodeImage = new System.Drawing.Bitmap(result2); // CODIGO DE BARRAS
            // Obtenida toda la informacion procedemos a renderizar el tarjeton...
            Document document = new Document();
            MigraDoc.DocumentObjectModel.Section section = document.AddSection();
            section.PageSetup.Orientation = Orientation.Landscape;
            section.PageSetup.PageHeight = "8cm";
            section.PageSetup.PageWidth = "16cm";
            //Footer QR code
            Image objqrCodeImage = section.AddImage(AmazonHelper.MigraDocFilenameFromByteArray(AmazonHelper.ToByteArray(qrCodeImage)));
            objqrCodeImage.Width = "8cm";
            objqrCodeImage.Height = "8cm";
            objqrCodeImage.Top = "7cm";
            objqrCodeImage.Left = "0cm";
            objqrCodeImage.WrapFormat.Style = WrapStyle.Through;
            objqrCodeImage.RelativeHorizontal = RelativeHorizontal.Page;
            objqrCodeImage.RelativeVertical = RelativeVertical.Page;

            
            // FECHA DE IMPRESION
            TextFrame T1 = section.AddTextFrame();
            T1.Width = "8cm";
            T1.Height = "2cm";
            T1.RelativeVertical = RelativeVertical.Page;
            T1.RelativeHorizontal = RelativeHorizontal.Page;
            T1.Top = "14.8cm";
            T1.Left = "0.3cm";
            Paragraph paragraph1 = T1.AddParagraph();
            paragraph1.Format.Font.Size = new Unit(10);
            paragraph1.AddText(date.ToString());
            paragraph1.Format.Font.Name = "Impact";
            paragraph1.Format.Font.Bold = false;
            paragraph1.Format.SpaceAfter = "0.2cm";
            paragraph1.Format.Font.Color = Colors.Black;

            // FOLIO : 
            TextFrame T2 = section.AddTextFrame();
            T2.Width = "7cm";
            T2.Height = "2cm";
            T2.RelativeVertical = RelativeVertical.Page;
            T2.RelativeHorizontal = RelativeHorizontal.Page;
            T2.Top = "6.4cm";
            T2.Left = ".8cm";
            Paragraph paragraph2 = T2.AddParagraph();
            paragraph2.Format.Font.Size = new Unit(20);
            paragraph2.AddText("Cupón de Pavo #" + folio);
            paragraph2.Format.Font.Name = "Impact";
            paragraph2.Format.Font.Bold = false;
            paragraph2.Format.SpaceAfter = "0.2cm";
            paragraph2.Format.Font.Color = Colors.Black;

            // GAFET : 
            TextFrame T3 = section.AddTextFrame();
            T3.Width = "8cm";
            T3.Height = "2cm";
            T3.RelativeVertical = RelativeVertical.Page;
            T3.RelativeHorizontal = RelativeHorizontal.Page;
            T3.Top = "2cm";
            T3.Left = "0.3cm";
            Paragraph paragraph3 = T3.AddParagraph();
            paragraph3.Format.Font.Size = new Unit(20);
            paragraph3.AddText(partnerReference);
            paragraph3.Format.Font.Name = "Impact";
            paragraph3.Format.Font.Bold = false;
            paragraph3.Format.SpaceAfter = "0.2cm";
            paragraph3.Format.Font.Color = Colors.Black;

            // NOMBRE : 
            TextFrame T4 = section.AddTextFrame();
            T4.Width = "7cm";
            T4.Height = "2cm";
            T4.RelativeVertical = RelativeVertical.Page;
            T4.RelativeHorizontal = RelativeHorizontal.Page;
            T4.Top = "2.8cm";
            T4.Left = "0.3cm";
            Paragraph paragraph4 = T4.AddParagraph();
            paragraph4.Format.Font.Size = new Unit(14);
            paragraph4.AddText(firstName + " " + lastNameF + " " + lastNameM);
            paragraph4.Format.Font.Name = "Impact";
            paragraph4.Format.Font.Bold = false;
            paragraph4.Format.SpaceAfter = "0.2cm";
            paragraph4.Format.Font.Color = Colors.Black;

            // MENSAJE : 
            TextFrame T5 = section.AddTextFrame();
            T5.Width = "7cm";
            T5.Height = "2cm";
            T5.RelativeVertical = RelativeVertical.Page;
            T5.RelativeHorizontal = RelativeHorizontal.Page;
            T5.Top = "4cm";
            T5.Left = "0.3cm";
            string name = firstName.ToLowerInvariant();
            TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
            name = textInfo.ToTitleCase(name);
            Paragraph paragraph5 = T5.AddParagraph();
            paragraph5.Format.Font.Size = new Unit(12);
            paragraph5.AddText(name + " agradecemos sinceramente por tu apoyo, esfuerzo y dedicación este año. ¡Te deseamos una Feliz Navidad y un próspero Año Nuevo!");
            paragraph5.Format.Font.Name = "Impact";
            paragraph5.Format.Font.Bold = false;
            paragraph5.Format.SpaceAfter = "0.2cm";
            paragraph5.Format.Font.Color = Colors.Black;

            // CAJERA : 
            TextFrame T9 = section.AddTextFrame();
            T9.Width = "7cm";
            T9.Height = "2cm";
            T9.RelativeVertical = RelativeVertical.Page;
            T9.RelativeHorizontal = RelativeHorizontal.Page;
            T9.Top = "15.2cm";
            T9.Left = "0.3cm";
            Paragraph paragraph9 = T9.AddParagraph();
            paragraph9.Format.Font.Size = new Unit(10);
            paragraph9.AddText("Cajera: " + cajera);
            paragraph9.Format.Font.Name = "Impact";
            paragraph9.Format.Font.Bold = false;
            paragraph9.Format.SpaceAfter = "0.2cm";
            paragraph9.Format.Font.Color = Colors.Black;


            //CABECERA

            // IMAGENES

            //ADMINISTRACION
            Image myImage = section.Headers.Primary.AddImage(Server.MapPath("~/Content/images/AdministracionVerde.png"));
            myImage.Height = "2cm";
            myImage.Width = "2cm";
            myImage.RelativeVertical = RelativeVertical.Page;
            myImage.RelativeHorizontal = RelativeHorizontal.Page;
            myImage.WrapFormat.Style = WrapStyle.Through;
            myImage.Top = "0cm";
            myImage.Left = "0cm";

            //TC
            Image myImageTc = section.Headers.Primary.AddImage(Server.MapPath("~/Content/images/TC_LOGO.png"));
            myImageTc.Height = "1.5cm";
            myImageTc.Width = "1.5cm";
            myImageTc.Top = "0.1cm";
            myImageTc.Left = "6.5cm";
            myImageTc.RelativeVertical = RelativeVertical.Page;
            myImageTc.RelativeHorizontal = RelativeHorizontal.Page;
            myImageTc.WrapFormat.Style = WrapStyle.Through;

            // TEXTO CABECERA
            TextFrame T6 = section.AddTextFrame();
            T6.Width = "6cm";
            T6.Height = "2cm";
            T6.RelativeVertical = RelativeVertical.Page;
            T6.RelativeHorizontal = RelativeHorizontal.Page;
            T6.Top = ".5cm";
            T6.Left = "1.8cm";
            Paragraph paragraph6 = T6.AddParagraph();
            paragraph6.Format.Font.Size = new Unit(6);
            paragraph6.AddText("Sindicato de Choferes, Taxistas y Similares del Caribe");
            paragraph6.Format.Font.Name = "Impact";
            paragraph6.Format.Font.Bold = false;
            paragraph6.Format.SpaceAfter = "0.2cm";
            paragraph6.Format.Font.Color = Colors.Black;

            TextFrame T7 = section.AddTextFrame();
            T7.Width = "6cm";
            T7.Height = "2cm";
            T7.RelativeVertical = RelativeVertical.Page;
            T7.RelativeHorizontal = RelativeHorizontal.Page;
            T7.Top = "0.7cm";
            T7.Left = "2cm";
            Paragraph paragraph7 = T7.AddParagraph();
            paragraph7.Format.Font.Size = new Unit(12);
            paragraph7.AddText("\"ANDRÉS QUINTANA ROO\"");
            paragraph7.Format.Font.Name = "Impact";
            paragraph7.Format.Font.Bold = false;
            paragraph7.Format.SpaceAfter = "0.2cm";
            paragraph7.Format.Font.Color = Colors.Black;

            TextFrame T8 = section.AddTextFrame();
            T8.Width = "6cm";
            T8.Height = "2cm";
            T8.RelativeVertical = RelativeVertical.Page;
            T8.RelativeHorizontal = RelativeHorizontal.Page;
            T8.Top = "1.2cm";
            T8.Left = "2.2cm";
            Paragraph paragraph8 = T8.AddParagraph();
            paragraph8.Format.Font.Size = new Unit(6);
            paragraph8.AddText("JUNTOS POR LA TRANSFORMACIÓN         2024-2026");
            paragraph8.Format.Font.Name = "Impact";
            paragraph8.Format.Font.Bold = false;
            paragraph8.Format.SpaceAfter = "0.2cm";
            paragraph8.Format.Font.Color = Colors.Black;


            
            // PDF GENERATOR 
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
                return File(stream, "application/pdf", "T_109090.pdf");
            }
        }

        [HttpPost]
        [Route("printWEB/{reference}/{type}")]
        [SessionFilter("Admin,Practi-Cajas,Practi-AdminTurkey")]
        public ActionResult printWEB(string note, string reference , string type = "G")
        {
            string cajera = this.User.firstName + " " + this.User.lastName;
            // OBTENEMOS LA INFORMACION DEL OPERADOR / SOCIO.
            int userId = 0;
            string firstName = "";
            string lastNameF = "";
            string lastNameM = "";
            string partnerReference = "";
            string status = "";
            ViewBag.reprint = 0;
            string membertype = "soc";
            if (reference.ToUpper().Contains("OP-"))
            {
                Operator _operator = new Operator();
                _operator = _operator.get(reference);
                partnerReference = _operator.partnerReference;
                userId = _operator.userId;
                firstName = _operator.firstName;
                lastNameF = _operator.lastNameF;
                lastNameM = _operator.lastNameM;
                status = _operator.STATUS;
                membertype = "op";

            }
            else
            {
                Partner _Partner = new Partner();
                _Partner = _Partner.get(reference);
                partnerReference = _Partner.partnerReference;
                userId = _Partner.userId;
                firstName = _Partner.firstName;
                lastNameF = _Partner.lastNameF;
                lastNameM = _Partner.lastNameM;
                status = _Partner.Status;
            }

            // DEUDAS
            Sale sale = new Sale(this.email);
            List<caja_cargos_DTO> cargos = sale.debs(reference, reference);
            ViewBag.debs = cargos == null ? new List<caja_cargos_DTO>() : cargos;
            decimal amountdebs = 0;
            foreach (caja_cargos_DTO deb in cargos ?? new List<caja_cargos_DTO>())
            {
                if (!partnerReference.ToUpper().Contains("OP") && deb.CLAVE != "0")
                {
                    amountdebs += (deb.DEBE * deb.PRECIO);
                }
                else if (partnerReference.ToUpper().Contains("OP"))
                {
                    amountdebs += (deb.DEBE * deb.PRECIO);
                }
            }
            ViewBag.amountdebs = amountdebs;

            Christmasgift gift = new Christmasgift();
            gift = gift.get(userId);
            if (
                amountdebs > this.maxdeb && !(Permissions.allow("Admin, Practi-AdminTurkey"))
                || status == "B" && !(Permissions.allow("Admin, Practi-AdminTurkey")) || 
                gift != null && 
                gift.ChristmasgiftPrinteds != null && 
                gift.ChristmasgiftPrinteds.Where(x => x.active == true).Count() > 0 &&
                !(Permissions.allow("Admin, Practi-AdminTurkey"))
                )
            {
                return View("error");
            }

            if (gift == null)
            {
                gift = new Christmasgift();
                gift.userid = userId;
                gift.membership_type = membertype;
                gift.note = note;
                if (this.User.Role == "Practi-Cajas" && type == "D" ||  this.User.Role == "Practi-AdminTurkey" && type == "D" || this.User.Role == "Admin" && type == "D")
                {
                    gift.DeliveryBy = this.User.firstName + " " + this.User.lastName;
                    gift.DeliveryDate = DateTime.Now;
                }

                // Obtener el numero de rempresion...
                gift.ChristmasgiftPrinteds.Add(
                    new ChristmasgiftPrinted()
                    {
                        PrintedBy = this.User.firstName + " " + this.User.lastName,
                        PrintedDate = DateTime.Now,
                        membership_type = "",
                        note = note
                    }
                );
                gift.save();
            }
            else
            {
                gift.ChristmasgiftPrinteds.Add(
                        new ChristmasgiftPrinted()
                        {
                            Christmasgiftid = gift.id,
                            PrintedBy = this.User.firstName + " " + this.User.lastName,
                            PrintedDate = DateTime.Now,
                            membership_type = "",
                            note = note
                        }
                );
                gift.update();
            }
            ViewBag.reprint = gift.ChristmasgiftPrinteds.Count();
            string folio = gift.id.ToString();
            DateTime date = DateTime.Now;
            // QR
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 270,
                    Width = 270,
                    Margin = 1
                }
            };
            // Codigo de barras.
            BarcodeWriter writer2 = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    PureBarcode = true,
                    Height = 100,
                    Width = 270,
                    Margin = 1,

                }
            };
            var result = writer.Write("PV" + folio);
            var result2 = writer2.Write("PV" + folio);
            System.Drawing.Bitmap qrCodeImage = new System.Drawing.Bitmap(result); // CODIGO QR
            System.Drawing.Bitmap BarCodeImage = new System.Drawing.Bitmap(result2); // CODIGO DE BARRAS
            System.IO.MemoryStream ms = new MemoryStream();
            qrCodeImage.Save(ms, ImageFormat.Jpeg);
            byte[] QRbyteImage = ms.ToArray();
            string QR = Convert.ToBase64String(QRbyteImage); // Get Base64;
            ViewBag.qr = QR;
            qrCodeImage.Save(ms, ImageFormat.Jpeg);
            byte[] BARbyteImage = ms.ToArray();
            string BAR = Convert.ToBase64String(BARbyteImage); // Get Base64;
            ViewBag.bar = BAR;
            ViewBag.folio = folio;
            ViewBag.date = date;
            ViewBag.cajera = cajera;
            ViewBag.partnerReference = partnerReference;
            ViewBag.firstName = firstName;
            ViewBag.lastNameF = lastNameF;
            ViewBag.lastNameM = lastNameM;
            return View();
        }

        // POST: Turkey/Delete/5
        [HttpPost]
        [Route("printExternal/{reference}/{type}")]
        [SessionFilter("Admin,Practi-Cajas,Practi-AdminTurkey")]
        public ActionResult printWEBex(FormCollection collection)
        {
            try
            {
                if (this.User.Role != "Admin")
                {
                    return View("Error");
                }
                string cajera = this.User.firstName + " " + this.User.lastName;
                int userId = 0;
                string lastNameM = "";
                string partnerReference = "";
                string firstName = collection["name"];
                string lastNameF = collection["lastname"];
                string note = collection["coments"];
                Christmasgift gift = new Christmasgift();
                gift.membership_type = "";
                gift.note = note;
                gift.membership_type = "EXTERNO";
                gift.ChristmasgiftPrinteds.Add(
                    new ChristmasgiftPrinted()
                    {
                        PrintedBy = this.User.firstName + " " + this.User.lastName,
                        PrintedDate = DateTime.Now,
                        membership_type = "",
                        note = ""
                    }
                );
                gift.save(); // Guardamos.
                ViewBag.reprint = gift.ChristmasgiftPrinteds.Count();
                string folio = gift.id.ToString();
                DateTime date = DateTime.Now;
                BarcodeWriter writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Height = 270,
                        Width = 270,
                        Margin = 1
                    }
                };
                // Codigo de barras.
                BarcodeWriter writer2 = new BarcodeWriter
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new EncodingOptions
                    {
                        PureBarcode = true,
                        Height = 100,
                        Width = 270,
                        Margin = 1,

                    }
                };
                var result = writer.Write("PV" + folio);
                var result2 = writer2.Write("PV" + folio);
                System.Drawing.Bitmap qrCodeImage = new System.Drawing.Bitmap(result); // CODIGO QR
                System.Drawing.Bitmap BarCodeImage = new System.Drawing.Bitmap(result2); // CODIGO DE BARRAS
                System.IO.MemoryStream ms = new MemoryStream();
                qrCodeImage.Save(ms, ImageFormat.Jpeg);
                byte[] QRbyteImage = ms.ToArray();
                string QR = Convert.ToBase64String(QRbyteImage); // Get Base64;
                ViewBag.qr = QR;
                qrCodeImage.Save(ms, ImageFormat.Jpeg);
                byte[] BARbyteImage = ms.ToArray();
                string BAR = Convert.ToBase64String(BARbyteImage); // Get Base64;
                ViewBag.bar = BAR;
                ViewBag.folio = folio;
                ViewBag.date = date;
                ViewBag.cajera = cajera;
                ViewBag.partnerReference = partnerReference;
                ViewBag.firstName = firstName;
                ViewBag.lastNameF = lastNameF;
                ViewBag.lastNameM = lastNameM;
                return View();
            }
            catch (Exception ex)
            {
                return View("Error");
            }

        }


        // CUPON EXTERNO
        public ActionResult ExternalCoupon()
        {
            return View();
        }

        // PAVOS POR CAJERA
        public ActionResult CashierTurkeys()
        {
            return View();
        }

        // POST: Operator/GetCities
        [HttpGet]
        public JsonResult getbydatedelivered()
        {
            try
            {
                // Entregas por dia.
                Christmasgift christmasgift = new Christmasgift();
                List<Pavday> entregas = christmasgift.getByDayDeliver();
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(entregas, settings);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (JsonReaderException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonSerializationException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonWriterException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonSchemaException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // POST: Operator/GetCities
        [HttpGet]
        public JsonResult getbydatedeliveredop()
        {
            try
            {
                // Entregas por dia.
                Christmasgift christmasgift = new Christmasgift();
                List<Pavday> entregas = christmasgift.getByDayDeliverOP();
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(entregas, settings);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (JsonReaderException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonSerializationException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonWriterException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonSchemaException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // POST: Operator/GetCities
        [HttpGet]
        public JsonResult getTicketsVsDeliver()
        {
            try
            {
                // Entregas por dia.
                Christmasgift christmasgift = new Christmasgift();
                List<List<Pavtype>> entregas = christmasgift.getByDayDeliverVsTIckets();
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(entregas, settings);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (JsonReaderException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonSerializationException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonWriterException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonSchemaException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        // POST: Operator/GetCities
        [HttpGet]
        public JsonResult getTicketsVsDeliverop()
        {
            try
            {
                // Entregas por dia.
                Christmasgift christmasgift = new Christmasgift();
                List<List<Pavtype>> entregas = christmasgift.getByDayDeliverVsTIcketsop();
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(entregas, settings);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (JsonReaderException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonSerializationException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonWriterException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonSchemaException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        // POST: Operator/GetCities
        [HttpGet]
        public JsonResult getbycashier()
        {
            try
            {
                // Entregas por dia.
                Christmasgift christmasgift = new Christmasgift();
                List<Pavtcashier> entregas = christmasgift.getCachies();
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(entregas, settings);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (JsonReaderException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonSerializationException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonWriterException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonSchemaException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        // POST: Operator/GetCities
        [HttpGet]
        public JsonResult gettotal()
        {
            try
            {
                // Entregas por dia.
                Christmasgift christmasgift = new Christmasgift();
                List<Pavtotal> entregas = christmasgift.gettotal();
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(entregas, settings);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (JsonReaderException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonSerializationException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonWriterException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (JsonSchemaException ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        // PAVOS POR DÍA
        public ActionResult TotalTurkeys()
        {
            return View();
        }

    }
}
