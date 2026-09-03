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
using System.Security.Cryptography;
using PdfSharp.Pdf.IO;

namespace radiotaxi.WEB.Controllers
{
    //[SessionFilter("Admin")]
    [RoutePrefix("Credential")]
    public class credentialController : webBaseController
    {
        [HttpGet]
        [Route("view/{gafet}")]
        public ActionResult generatepdf(string gafet)
        {
            ///////// Aqui va el codigo para generar el PDF
            ///
            try
            {
                gafet = gafet.ToUpper();
                if (gafet.Contains("OP-"))
                {
                    Operator _operator = new Operator().get(gafet);

                    if (_operator != null && _operator.bucket != null && _operator.key != null)
                    {
                        _operator.urlImage = AmazonHelper.getImageUser(_operator.bucket, _operator.key);
                    }
                    // Fecha vencimiento.
                    string expDate = DateTime.Now.AddYears(1).AddDays(15).ToShortDateString().ToString();

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

                    var result = writer.Write("C" + gafet.Replace("-", ""));
                    var result2 = writer2.Write("C" + gafet.Replace("-", ""));

                    System.Drawing.Bitmap qrCodeImage = new System.Drawing.Bitmap(result); // CODIGO QR
                    System.Drawing.Bitmap BarCodeImage = new System.Drawing.Bitmap(result2); // CODIGO DE BARRAS


                    // Obtenida toda la informacion procedemos a renderizar el tarjeton...
                    Document document = new Document();
                    MigraDoc.DocumentObjectModel.Section section = document.AddSection();
                    section.PageSetup.Orientation = Orientation.Portrait;
                    section.PageSetup.PageHeight = "5.4cm";
                    section.PageSetup.PageWidth = "8.6cm";

                    Image fImage = section.Headers.Primary.AddImage(Server.MapPath("~/Content/images/FrontOP.png"));
                    fImage.Height = "5.4cm";
                    fImage.Width = "8.6cm";
                    fImage.RelativeVertical = RelativeVertical.Page;
                    fImage.RelativeHorizontal = RelativeHorizontal.Page;
                    fImage.WrapFormat.Style = WrapStyle.Through;

                    //Image Face
                    byte[] imageStream = AmazonHelper.GetImageStream(_operator.urlImage);
                    Image image = section.AddImage(AmazonHelper.MigraDocFilenameFromByteArray(imageStream));
                    int height = 0;
                    using (PdfSharp.Drawing.XImage image2 = PdfSharp.Drawing.XImage.FromGdiPlusImage(AmazonHelper.toImage(imageStream)))
                    {
                        height = image2.PixelHeight;
                    }
                    if (height >= 270)
                    {
                        image.Height = "2.8cm";
                        image.Width = "2.12cm";
                        image.Top = ".4cm";
                        image.Left = "5.2cm";
                    }
                    else
                    {
                        image.Height = "2.8cm";
                        image.Width = "2.15cm";
                        image.Top = ".4cm";
                        image.Left = "5.2cm";
                    }
                    image.RelativeVertical = RelativeVertical.Page;
                    image.RelativeHorizontal = RelativeHorizontal.Page;
                    image.WrapFormat.Style = WrapStyle.Through;




                    // Codigo de barras
                    Image objImgBarcode = section.AddImage(AmazonHelper.MigraDocFilenameFromByteArray(AmazonHelper.ToByteArray(BarCodeImage)));
                    objImgBarcode.Height = "1cm";
                    objImgBarcode.Width = "5.2cm";
                    objImgBarcode.RelativeVertical = RelativeVertical.Page;
                    objImgBarcode.RelativeHorizontal = RelativeHorizontal.Page;
                    objImgBarcode.Top = "4.25cm";
                    objImgBarcode.Left = "3.7cm";
                    objImgBarcode.WrapFormat.Style = WrapStyle.Through;



                    // Nombre...
                    TextFrame tf = section.AddTextFrame();
                    tf.Width = "5.2cm";
                    tf.Height = "1cm";
                    tf.RelativeVertical = RelativeVertical.Page;
                    tf.RelativeHorizontal = RelativeHorizontal.Page;
                    tf.Top = "3.3cm";
                    tf.Left = "3.8cm";
                    Paragraph paragraph = tf.AddParagraph();
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    paragraph.Format.Font.Size = new Unit(11);
                    paragraph.AddText(_operator.NOMBRE.ToUpper());
                    paragraph.Format.Font.Name = "Impact";
                    paragraph.Format.Font.Bold = true;
                    paragraph.Format.SpaceAfter = "0.2cm";
                    paragraph.Format.Font.Color = Colors.Black;

                    // Apellidos
                    TextFrame tf1 = section.AddTextFrame();
                    tf1.Width = "4cm";
                    tf1.Height = "1cm";
                    tf1.RelativeVertical = RelativeVertical.Page;
                    tf1.RelativeHorizontal = RelativeHorizontal.Page;
                    tf1.Top = "3.7cm";
                    tf1.Left = "4.25cm";
                    Paragraph paragraph1 = tf1.AddParagraph();
                    paragraph1.Format.Alignment = ParagraphAlignment.Center;
                    paragraph1.Format.Font.Size = new Unit(11);
                    paragraph1.AddText(_operator.lastNameF.ToUpper() + " " + _operator.lastNameM.ToUpper());
                    paragraph1.Format.Font.Name = "Impact";
                    paragraph1.Format.Font.Bold = true;
                    paragraph1.Format.SpaceAfter = "0.2cm";
                    paragraph1.Format.Font.Color = Colors.Black;

                    /*
                    // Apellido Paterno
                    TextFrame tf1 = section.AddTextFrame();
                    tf1.Width = "20mm";
                    tf1.Height = "40mm";
                    tf1.RelativeVertical = RelativeVertical.Page;
                    tf1.RelativeHorizontal = RelativeHorizontal.Page;
                    tf1.Top = "3.7cm";
                    tf1.Left = "5.65cm";
                    Paragraph paragraph1 = tf1.AddParagraph();
                    paragraph1.Format.Font.Size = new Unit(11);
                    paragraph1.AddText(_operator.lastNameF.ToUpper());
                    paragraph1.Format.Font.Name = "Impact";
                    paragraph1.Format.Font.Bold = true;
                    paragraph1.Format.SpaceAfter = "0.2cm";
                    paragraph1.Format.Font.Color = Colors.Black;

                    // Apellido Materno
                    TextFrame tf2 = section.AddTextFrame();
                    tf2.Width = "20mm";
                    tf2.Height = "60mm";
                    tf2.RelativeVertical = RelativeVertical.Page;
                    tf2.RelativeHorizontal = RelativeHorizontal.Page;
                    tf2.Top = "4.1cm";
                    tf2.Left = "5.65cm";
                    Paragraph paragraph2 = tf2.AddParagraph();
                    paragraph2.Format.Font.Size = new Unit(11);
                    paragraph2.AddText(_operator.lastNameM.ToUpper());
                    paragraph2.Format.Font.Name = "Impact";
                    paragraph2.Format.Font.Bold = true;
                    paragraph2.Format.SpaceAfter = "0.2cm";
                    paragraph2.Format.Font.Color = Colors.Black;
                    */

                    //numero de operador
                    TextFrame tf3 = section.AddTextFrame();
                    tf3.Width = "4cm";
                    tf3.Height = "50mm";
                    tf3.RelativeVertical = RelativeVertical.Page;
                    tf3.RelativeHorizontal = RelativeHorizontal.Page;
                    tf3.Top = "1.2cm";
                    tf3.Left = "1.4cm";
                    Paragraph paragraph3 = tf3.AddParagraph();
                    paragraph3.Format.Alignment = ParagraphAlignment.Center;
                    paragraph3.Format.Font.Size = new Unit(15);
                    paragraph3.AddText(_operator.partnerReference);
                    paragraph3.Format.Font.Name = "Impact";
                    paragraph3.Format.Font.Bold = true;
                    paragraph3.Format.SpaceAfter = "0.2cm";
                    paragraph3.Format.Font.Color = Colors.Black;

                    /*/ Numero de taxi
                    TextFrame tf4 = section.AddTextFrame();
                    tf4.Width = "210mm";
                    tf4.Height = "85mm";
                    tf4.RelativeVertical = RelativeVertical.Page;
                    tf4.RelativeHorizontal = RelativeHorizontal.Page;
                    tf4.Top = "0.9cm";
                    tf4.Left = "0.7cm";
                    Paragraph paragraph4 = tf4.AddParagraph();
                    paragraph4.Format.Font.Size = new Unit(11);
                    //paragraph4.AddText(collection.Get("taxiNumber").PadLeft(4, '0'));
                    paragraph4.AddText(_card.taxi.PadLeft(4, '0'));
                    paragraph4.Format.Font.Name = "Impact";
                    paragraph4.Format.Font.Bold = true;
                    paragraph4.Format.Font.Color = Colors.Red;*/

                    /*/ Placas taxi
                    TextFrame tf6 = section.AddTextFrame();
                    tf6.Width = "138mm";
                    tf6.Height = "50mm";
                    tf6.RelativeVertical = RelativeVertical.Page;
                    tf6.RelativeHorizontal = RelativeHorizontal.Page;
                    tf6.Top = "11.2cm";
                    tf6.Left = "6.2cm";
                    Paragraph paragraph6 = tf6.AddParagraph();
                    paragraph6.Format.Font.Size = new Unit(28);
                    paragraph6.AddText(_emplacamiento.Placas);
                    paragraph6.Format.Font.Name = "Impact";
                    paragraph6.Format.Font.Bold = true;
                    paragraph6.Format.SpaceAfter = "0.2cm";
                    paragraph6.Format.Font.Color = Colors.Red;*/



                    // Segunda hoja
                    Section section2 = document.AddSection();
                    section2.PageSetup.Orientation = Orientation.Portrait;
                    section2.PageSetup.PageHeight = "5.4cm";
                    section2.PageSetup.PageWidth = "8.6cm";


                    // VIGENCIA
                    TextFrame tf5 = section2.AddTextFrame();
                    tf5.Width = "138mm";
                    tf5.Height = "50mm";
                    tf5.RelativeVertical = RelativeVertical.Page;
                    tf5.RelativeHorizontal = RelativeHorizontal.Page;
                    tf5.Top = "4.1cm";
                    tf5.Left = "3.14cm";
                    Paragraph paragraph5 = tf5.AddParagraph();
                    paragraph5.Format.Font.Size = new Unit(11);
                    string fechaingreso = ((DateTime)_operator.FECHAING).ToString("dd/MM/yyyy");
                    paragraph5.AddText(fechaingreso);
                    paragraph5.Format.Font.Name = "Impact";
                    paragraph5.Format.Font.Bold = true;
                    paragraph5.Format.SpaceAfter = "0.2cm";
                    paragraph5.Format.Font.Color = Colors.Black;


                    Image bImage = section2.Headers.Primary.AddImage(Server.MapPath("~/Content/images/Back.png"));
                    bImage.Height = "5.4cm";
                    bImage.Width = "8.6cm";
                    bImage.RelativeVertical = RelativeVertical.Page;
                    bImage.RelativeHorizontal = RelativeHorizontal.Page;
                    bImage.WrapFormat.Style = WrapStyle.Through;

                    //Footer QR code
                    Image objImgLogo = section2.AddImage(AmazonHelper.MigraDocFilenameFromByteArray(AmazonHelper.ToByteArray(qrCodeImage)));
                    objImgLogo.Height = "2.8cm";
                    objImgLogo.Width = "2.8cm";
                    objImgLogo.RelativeVertical = RelativeVertical.Page;
                    objImgLogo.RelativeHorizontal = RelativeHorizontal.Page;
                    objImgLogo.Top = "2.4cm";
                    objImgLogo.Left = "0.6";
                    objImgLogo.WrapFormat.Style = WrapStyle.Through;

                    section2.PageSetup.LeftMargin = "0cm";
                    section2.PageSetup.RightMargin = "0cm";
                    section2.PageSetup.TopMargin = "0cm";
                    section2.PageSetup.BottomMargin = "0cm";

                    PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
                    // Associate the MigraDoc document with a renderer
                    pdfRenderer.Document = document;
                    // Layout and render document to PDF
                    pdfRenderer.RenderDocument();

                    // Send PDF to browser                
                    using (MemoryStream stream = new MemoryStream())
                    {
                        pdfRenderer.Save(stream, false);
                        var _document = PdfReader.Open(stream);
                        var page = _document.Pages[1];
                        page.Rotate = (page.Rotate + 180) % 360;
                        pdfRenderer.PdfDocument = _document;
                        pdfRenderer.Save(stream, false);
                        Response.Clear();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-length", stream.Length.ToString());
                        Response.BinaryWrite(stream.ToArray());
                        Response.Flush();
                        stream.Position = 0;
                        stream.Close();
                        Response.End();
                        return File(stream, "application/pdf", "T_" + gafet + ".pdf");
                    }
                }
                else {
                    Partner _partner = new Partner().get(gafet);

                    if (_partner == null)
                    {
                        return RedirectToAction("ErrorCredencial", "Error", new { mensaje = "No se encontro al socio en la base de datos." });
                        
                    }

                    if (_partner != null && _partner.bucket != null && _partner.key != null)
                    {
                        _partner.urlImage = AmazonHelper.getImageUser(_partner.bucket, _partner.key);
                    }
                    else {
                        return RedirectToAction("ErrorCredencial", "Error", new { mensaje = "El socio no cuenta con foto." });
                    }



                    // Fecha vencimiento.
                    string expDate = DateTime.Now.AddYears(1).AddDays(15).ToShortDateString().ToString();

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

                    var result = writer.Write("C-" + gafet);
                    var result2 = writer2.Write("C-" + gafet);
                    System.Drawing.Bitmap qrCodeImage = new System.Drawing.Bitmap(result); // CODIGO QR
                    System.Drawing.Bitmap BarCodeImage = new System.Drawing.Bitmap(result2); // CODIGO DE BARRAS
                    // Obtenida toda la informacion procedemos a renderizar el tarjeton...
                    Document document = new Document();
                    MigraDoc.DocumentObjectModel.Section section = document.AddSection();
                    section.PageSetup.Orientation = Orientation.Portrait;
                    section.PageSetup.PageHeight = "5.4cm";
                    section.PageSetup.PageWidth = "8.6cm";
                    Image fImage = section.Headers.Primary.AddImage(Server.MapPath("~/Content/images/Front.png"));
                    fImage.Height = "5.4cm";
                    fImage.Width = "8.6cm";
                    fImage.RelativeVertical = RelativeVertical.Page;
                    fImage.RelativeHorizontal = RelativeHorizontal.Page;
                    fImage.WrapFormat.Style = WrapStyle.Through;

                    try
                    {
                        //Image Face
                        byte[] imageStream = AmazonHelper.GetImageStream(_partner.urlImage);
                        Image image = section.AddImage(AmazonHelper.MigraDocFilenameFromByteArray(imageStream));
                        int height = 0;
                        using (PdfSharp.Drawing.XImage image2 = PdfSharp.Drawing.XImage.FromGdiPlusImage(AmazonHelper.toImage(imageStream)))
                        {
                            height = image2.PixelHeight;
                        }
                        if (height >= 270)
                        {
                            image.Height = "2.8cm";
                            image.Width = "2.12cm";
                            image.Top = ".4cm";
                            image.Left = "5.2cm";
                        }
                        else
                        {
                            image.Height = "2.8cm";
                            image.Width = "2.15cm";
                            image.Top = ".4cm";
                            image.Left = "5.2cm";
                        }
                        image.RelativeVertical = RelativeVertical.Page;
                        image.RelativeHorizontal = RelativeHorizontal.Page;
                        image.WrapFormat.Style = WrapStyle.Through;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                    // Codigo de barras
                    Image objImgBarcode = section.AddImage(AmazonHelper.MigraDocFilenameFromByteArray(AmazonHelper.ToByteArray(BarCodeImage)));
                    objImgBarcode.Height = "1cm";
                    objImgBarcode.Width = "5.2cm";
                    objImgBarcode.RelativeVertical = RelativeVertical.Page;
                    objImgBarcode.RelativeHorizontal = RelativeHorizontal.Page;
                    objImgBarcode.Top = "4.25cm";
                    objImgBarcode.Left = "3.7cm";
                    objImgBarcode.WrapFormat.Style = WrapStyle.Through;

                    // Nombre...
                    TextFrame tf = section.AddTextFrame();
                    tf.Width = "4cm";
                    tf.Height = "1cm";
                    tf.RelativeVertical = RelativeVertical.Page;
                    tf.RelativeHorizontal = RelativeHorizontal.Page;
                    tf.Top = "3.3cm";
                    tf.Left = "4.25cm";
                    Paragraph paragraph = tf.AddParagraph();
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    paragraph.Format.Font.Size = new Unit(11);
                    paragraph.AddText(_partner.firstName.ToUpper());
                    paragraph.Format.Font.Name = "Impact";
                    paragraph.Format.Font.Bold = true;
                    paragraph.Format.SpaceAfter = "0.2cm";
                    paragraph.Format.Font.Color = Colors.Black;

                    // Apellido Paterno y Materno
                    TextFrame tf1 = section.AddTextFrame();
                    tf1.Width = "4cm";
                    tf1.Height = "1cm";
                    tf1.RelativeVertical = RelativeVertical.Page;
                    tf1.RelativeHorizontal = RelativeHorizontal.Page;
                    tf1.Top = "3.7cm";
                    tf1.Left = "4.25cm";
                    Paragraph paragraph1 = tf1.AddParagraph();
                    paragraph1.Format.Alignment = ParagraphAlignment.Center;
                    paragraph1.Format.Font.Size = new Unit(11);
                    paragraph1.AddText(_partner.lastNameF.ToUpper() + " " + _partner.lastNameM.ToUpper());
                    paragraph1.Format.Font.Name = "Impact";
                    paragraph1.Format.Font.Bold = true;
                    paragraph1.Format.SpaceAfter = "0.2cm";
                    paragraph1.Format.Font.Color = Colors.Black;

                    //Eco
                    TextFrame tf3 = section.AddTextFrame();
                    tf3.Width = "4cm";
                    tf3.Height = "50mm";
                    tf3.RelativeVertical = RelativeVertical.Page;
                    tf3.RelativeHorizontal = RelativeHorizontal.Page;
                    tf3.Top = "1.2cm";
                    tf3.Left = "1.4cm";
                    Paragraph paragraph3 = tf3.AddParagraph();
                    paragraph3.Format.Alignment = ParagraphAlignment.Center;
                    paragraph3.Format.Font.Size = new Unit(30);
                    paragraph3.AddText(_partner.partnerReference);
                    paragraph3.Format.Font.Name = "Impact";
                    paragraph3.Format.Font.Bold = true;
                    paragraph3.Format.SpaceAfter = "0.2cm";
                    paragraph3.Format.Font.Color = Colors.Black;

                    // Segunda hoja
                    Section section2 = document.AddSection();
                    section2.PageSetup.Orientation = Orientation.Portrait;
                    section2.PageSetup.PageHeight = "5.4cm";
                    section2.PageSetup.PageWidth = "8.6cm";

                    if (_partner.fechaingreso != null)
                    {
                        // VIGENCIA
                        TextFrame tf5 = section2.AddTextFrame();
                        tf5.Width = "138mm";
                        tf5.Height = "50mm";
                        tf5.RelativeVertical = RelativeVertical.Page;
                        tf5.RelativeHorizontal = RelativeHorizontal.Page;
                        tf5.Top = "4.1cm";
                        tf5.Left = "3.14cm";
                        Paragraph paragraph5 = tf5.AddParagraph();
                        paragraph5.Format.Font.Size = new Unit(11);
                        string fechaingreso = ((DateTime)_partner.fechaingreso).ToString("dd/MM/yyyy");
                        paragraph5.AddText(fechaingreso);
                        paragraph5.Format.Font.Name = "Impact";
                        paragraph5.Format.Font.Bold = true;
                        paragraph5.Format.SpaceAfter = "0.2cm";
                        paragraph5.Format.Font.Color = Colors.Black;
                    }
                    else {
                        return RedirectToAction("ErrorCredencial", "Error", new { mensaje = "Sin fecha de ingreso" });
                    }

                    Image bImage = section2.Headers.Primary.AddImage(Server.MapPath("~/Content/images/Back.png"));
                    bImage.Height = "5.4cm";
                    bImage.Width = "8.6cm";
                    bImage.RelativeVertical = RelativeVertical.Page;
                    bImage.RelativeHorizontal = RelativeHorizontal.Page;
                    bImage.WrapFormat.Style = WrapStyle.Through;

                    //Footer QR code
                    Image objImgLogo = section2.AddImage(AmazonHelper.MigraDocFilenameFromByteArray(AmazonHelper.ToByteArray(qrCodeImage)));
                    objImgLogo.Height = "2.8cm";
                    objImgLogo.Width = "2.8cm";
                    objImgLogo.RelativeVertical = RelativeVertical.Page;
                    objImgLogo.RelativeHorizontal = RelativeHorizontal.Page;
                    objImgLogo.Top = "2.4cm";
                    objImgLogo.Left = "0.6";
                    objImgLogo.WrapFormat.Style = WrapStyle.Through;

                    section2.PageSetup.LeftMargin = "0cm";
                    section2.PageSetup.RightMargin = "0cm";
                    section2.PageSetup.TopMargin = "0cm";
                    section2.PageSetup.BottomMargin = "0cm";

                    // Crear el renderizador de PDF
                    PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                    renderer.Document = document;
                    renderer.RenderDocument();
                    // Guardar el documento PDF en la memoria
                    using (MemoryStream stream = new MemoryStream())
                    {
                        renderer.Save(stream, false);
                        var _document = PdfReader.Open(stream);
                        var page = _document.Pages[1];
                        page.Rotate = (page.Rotate + 180) % 360;
                        renderer.PdfDocument = _document;
                        renderer.Save(stream, false);
                        return File(stream.ToArray(), "application/pdf", "T_" + gafet + ".pdf");
                    }
                }
            }
            catch (Exception ex)
            {
                // ENVIAR A LA VISTA DE ERROR (GERMAN)
                return RedirectToAction("ErrorCredencial", "Error", new { mensaje = "Sistem error, " + ex.Message });
            }

        }
    }
}