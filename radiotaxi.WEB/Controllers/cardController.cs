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

namespace radiotaxi.WEB.Controllers
{
    [SessionFilter("Admin,tarjeton")]
    [RoutePrefix("Card")]
    public class cardController : webBaseController
    {
        // GET: Client
        public ActionResult Index(int? page)
        {
            if (this.CardSearchParameters == null) this.CardSearchParameters = new CardSearchParametersDTO();
            ViewBag.parameters = this.CardSearchParameters; // Objeto que contiene los parametros de busqueda, estos se guardan en session pero estransparante para la capa del controlador.
            card Card = new card();
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            CardPagination CardPagination = null;
            if (this.CardSearchParameters != null && this.CardSearchParameters.gafet.ToUpper().Contains("OP-"))
            {
                CardPagination = Card.get_operator(pageNumber, pageSize, this.CardSearchParameters.taxi, this.CardSearchParameters.gafet, this.CardSearchParameters.name, this.CardSearchParameters.lastName1, this.CardSearchParameters.lastName2);
            }
            else { 
                CardPagination = Card.get_partner(pageNumber, pageSize, this.CardSearchParameters.taxi, this.CardSearchParameters.gafet, this.CardSearchParameters.name, this.CardSearchParameters.lastName1, this.CardSearchParameters.lastName2);
            }
            ViewBag.totalClients = CardPagination.TotalItems;
            return View(CardPagination);
        }

        [HttpPost]
        public ActionResult Index(int? page, string taxi, string gafet, string name, string lastName1, string lastName2)
        {
            this.CardSearchParameters = new CardSearchParametersDTO();
            this.CardSearchParameters.taxi = taxi; // Este es el gafet de operador
            this.CardSearchParameters.gafet = gafet; // Este es el gafet de operador
            this.CardSearchParameters.name = name;
            this.CardSearchParameters.lastName1 = lastName1;
            this.CardSearchParameters.lastName2 = lastName2;
            ViewBag.parameters = this.CardSearchParameters;
            card Card = new card();
            int statusConnection = 0;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            CardPagination CardPagination = null;
            if (gafet != null && gafet.ToUpper().Contains("OP-"))
            {
                CardPagination = Card.get_operator(pageNumber, pageSize, taxi, gafet, name, lastName1, lastName2);
            }
            else { 
                CardPagination = Card.get_partner(pageNumber, pageSize, taxi, gafet, name, lastName1, lastName2);
            }
            ViewBag.totalClients = CardPagination.TotalItems;
            return View(CardPagination);
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public ActionResult Edit(int id)
        {
            try
            {
                card _card = new card();
                _card = _card.get(id);
                return View(_card);
            }
            catch (Exception ex)
            {
                // ENVIAR A LA VISTA DE ERROR (GERMAN)
                throw;
            }
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public ActionResult Edit(int id, card card)
        {
            try
            {
                card.isActive = card.isActive == null ? false : true;
                card.low = card.low == null ? false : true;
                card.deliver = card.deliver == null ? false : true;
                // Actualizacion el objeto orignal
                card _card = new card();
                _card = _card.get(id);
                _card.turn = card.turn;
                _card.isActive = card.isActive;
                _card.deliver = card.deliver;
                _card.note = card.note;
                _card.low = card.low;
                if (_card.deliver == true && _card.deliverDate == null)
                {
                    _card.deliverDate = DateTime.Now;
                }
                if (_card.low == true && _card.lowDate == null)
                {
                    _card.lowDate = DateTime.Now;
                }
                _card.Update();
                this.Message = new MessageDTO();
                this.Message.Type = 200; // OK, correcto todo.
                this.Message.Message = "Se guardo todo correctamente.";
                // Redireccionar a show.
                return Redirect("/card/details/" + id);
            }
            catch (Exception ex)
            {
                // ENVIAR A LA VISTA DE ERROR (GERMAN)
                throw;
            }
        }

        [HttpGet]
        [Route("generatepdf/{id}")]
        public ActionResult generatepdf(int id)
        {
            try
            {
                // REGISTRAR CUANTAS VECES SE GENERA EL TARJETON - En el objeto PINK
                // Obtenemos los datos del tarjeton.
                card _card = new card().get(id);
                // Obtenemos la informacion del TAXI.
                EmplacamientoDTO _emplacamiento = new Emplacamiento().get(_card.taxi);
                if (_card.partnerReference.Contains("OP-"))
                {
                    // Obtenemos la informacion del operador.
                    Operator _operator = new Operator().get(_card.partnerReference);
                    // Obtenemos la imagen del operador
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

                    var result = writer.Write("T-" + _card.id);
                    var result2 = writer2.Write("T-" + _card.id);
                    System.Drawing.Bitmap qrCodeImage = new System.Drawing.Bitmap(result); // CODIGO QR
                    System.Drawing.Bitmap BarCodeImage = new System.Drawing.Bitmap(result2); // CODIGO DE BARRAS
                                                                                             // Obtenida toda la informacion procedemos a renderizar el tarjeton...
                    Document document = new Document();
                    MigraDoc.DocumentObjectModel.Section section = document.AddSection();
                    section.PageSetup.Orientation = Orientation.Landscape;
                    section.PageSetup.PageHeight = "19cm";
                    section.PageSetup.PageWidth = "14cm";

                    Image myImage = section.Headers.Primary.AddImage(Server.MapPath("~/Content/images/tarjeton.png"));
                    myImage.Height = "14cm";
                    myImage.Width = "19cm";
                    myImage.RelativeVertical = RelativeVertical.Page;
                    myImage.RelativeHorizontal = RelativeHorizontal.Page;
                    myImage.WrapFormat.Style = WrapStyle.Through;

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
                        image.Height = "6.8cm";
                        image.Width = "5.6cm";
                        image.Top = "3.92cm";
                        image.Left = "12.6cm";
                    }
                    else
                    {
                        image.Height = "6.8cm";
                        image.Width = "5.6cm";
                        image.Top = "3.92cm";
                        image.Left = "12.6cm";
                    }
                    image.RelativeVertical = RelativeVertical.Page;
                    image.RelativeHorizontal = RelativeHorizontal.Page;
                    image.WrapFormat.Style = WrapStyle.Through;


                    //Footer QR code
                    Image objImgLogo = section.AddImage(AmazonHelper.MigraDocFilenameFromByteArray(AmazonHelper.ToByteArray(qrCodeImage)));
                    objImgLogo.Height = "3.5cm";
                    objImgLogo.Width = "3.5cm";
                    objImgLogo.RelativeVertical = RelativeVertical.Page;
                    objImgLogo.RelativeHorizontal = RelativeHorizontal.Page;
                    objImgLogo.Top = "7.3cm";
                    objImgLogo.Left = "0.78cm";
                    objImgLogo.WrapFormat.Style = WrapStyle.Through;

                    // Codigo de barras
                    Image objImgBarcode = section.AddImage(AmazonHelper.MigraDocFilenameFromByteArray(AmazonHelper.ToByteArray(BarCodeImage)));
                    objImgBarcode.Height = "1.4cm";
                    objImgBarcode.Width = "4.2cm";
                    objImgBarcode.RelativeVertical = RelativeVertical.Page;
                    objImgBarcode.RelativeHorizontal = RelativeHorizontal.Page;
                    objImgBarcode.Top = "12.2cm";
                    objImgBarcode.Left = "0.5cm";
                    objImgBarcode.WrapFormat.Style = WrapStyle.Through;



                    // Nombre...
                    TextFrame tf = section.AddTextFrame();
                    tf.Width = "138mm";
                    tf.Height = "50mm";
                    tf.RelativeVertical = RelativeVertical.Page;
                    tf.RelativeHorizontal = RelativeHorizontal.Page;
                    tf.Top = "3.5cm";
                    tf.Left = "1cm";
                    Paragraph paragraph = tf.AddParagraph();
                    paragraph.Format.Font.Size = new Unit(36);
                    paragraph.AddText(_operator.firstName.ToUpper());
                    paragraph.Format.Font.Name = "Impact";
                    paragraph.Format.Font.Bold = true;
                    paragraph.Format.SpaceAfter = "0.2cm";
                    paragraph.Format.Font.Color = Colors.Red;


                    // Apellido Paterno
                    TextFrame tf1 = section.AddTextFrame();
                    tf1.Width = "138mm";
                    tf1.Height = "50mm";
                    tf1.RelativeVertical = RelativeVertical.Page;
                    tf1.RelativeHorizontal = RelativeHorizontal.Page;
                    tf1.Top = "4.8cm";
                    tf1.Left = "1cm";
                    Paragraph paragraph1 = tf1.AddParagraph();
                    paragraph1.Format.Font.Size = new Unit(36);
                    paragraph1.AddText(_operator.lastNameF.ToUpper());
                    paragraph1.Format.Font.Name = "Impact";
                    paragraph1.Format.Font.Bold = true;
                    paragraph1.Format.SpaceAfter = "0.2cm";
                    paragraph1.Format.Font.Color = Colors.Red;

                    // Apellido Materno
                    TextFrame tf2 = section.AddTextFrame();
                    tf2.Width = "138mm";
                    tf2.Height = "50mm";
                    tf2.RelativeVertical = RelativeVertical.Page;
                    tf2.RelativeHorizontal = RelativeHorizontal.Page;
                    tf2.Top = "6.1cm";
                    tf2.Left = "1cm";
                    Paragraph paragraph2 = tf2.AddParagraph();
                    paragraph2.Format.Font.Size = new Unit(36);
                    paragraph2.AddText(_operator.lastNameM.ToUpper());
                    paragraph2.Format.Font.Name = "Impact";
                    paragraph2.Format.Font.Bold = true;
                    paragraph2.Format.SpaceAfter = "0.2cm";
                    paragraph2.Format.Font.Color = Colors.Red;


                    //numero de operador

                    //if (_card.partnerReference.)
                    //{

                    //}

                    //var tte = _card.partnerReference.Split('-');
                    string s = _card.partnerReference;
                    string[] partes = s.Split('-'); // Divide en "op" y "1000003"

                    string prefijo = partes[0];
                    int numero = int.Parse(partes[1]); // Convierte "1000003" en entero

                    if(numero > 1000000)
                    {
                        int nnumero = numero - 1000000;

                        TextFrame tf3 = section.AddTextFrame();
                        tf3.Width = "138mm";
                        tf3.Height = "50mm";
                        tf3.RelativeVertical = RelativeVertical.Page;
                        tf3.RelativeHorizontal = RelativeHorizontal.Page;
                        tf3.Top = "11.5cm";
                        tf3.Left = "12.8cm";
                        Paragraph paragraph3 = tf3.AddParagraph();
                        paragraph3.Format.Font.Size = new Unit(38);
                        paragraph3.AddText("TTE-" + nnumero);
                        paragraph3.Format.Font.Name = "Impact";
                        paragraph3.Format.Font.Bold = true;
                        paragraph3.Format.SpaceAfter = "0.2cm";
                        paragraph3.Format.Font.Color = Colors.Red;


                    }
                    else
                    {
                        TextFrame tf3 = section.AddTextFrame();
                        tf3.Width = "138mm";
                        tf3.Height = "50mm";
                        tf3.RelativeVertical = RelativeVertical.Page;
                        tf3.RelativeHorizontal = RelativeHorizontal.Page;
                        tf3.Top = "11.5cm";
                        tf3.Left = "12.8cm";
                        Paragraph paragraph3 = tf3.AddParagraph();
                        paragraph3.Format.Font.Size = new Unit(38);
                        paragraph3.AddText(_card.partnerReference);
                        paragraph3.Format.Font.Name = "Impact";
                        paragraph3.Format.Font.Bold = true;
                        paragraph3.Format.SpaceAfter = "0.2cm";
                        paragraph3.Format.Font.Color = Colors.Red;
                    }


                    

                    // Numero de taxi
                    TextFrame tf4 = section.AddTextFrame();
                    tf4.Width = "210mm";
                    tf4.Height = "85mm";
                    tf4.RelativeVertical = RelativeVertical.Page;
                    tf4.RelativeHorizontal = RelativeHorizontal.Page;
                    tf4.Top = "8.9cm";
                    tf4.Left = "7cm";
                    Paragraph paragraph4 = tf4.AddParagraph();
                    paragraph4.Format.Font.Size = new Unit(40);
                    //paragraph4.AddText(collection.Get("taxiNumber").PadLeft(4, '0'));
                    paragraph4.AddText(_card.taxi.PadLeft(4, '0'));
                    paragraph4.Format.Font.Name = "Impact";
                    paragraph4.Format.Font.Bold = true;
                    paragraph4.Format.Font.Color = Colors.Red;

                    // Placas taxi
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
                    paragraph6.Format.Font.Color = Colors.Red;


                    // VIGENCIA
                    TextFrame tf5 = section.AddTextFrame();
                    tf5.Width = "138mm";
                    tf5.Height = "50mm";
                    tf5.RelativeVertical = RelativeVertical.Page;
                    tf5.RelativeHorizontal = RelativeHorizontal.Page;
                    tf5.Top = "11.3cm";
                    tf5.Left = ".9cm";
                    Paragraph paragraph5 = tf5.AddParagraph();
                    paragraph5.Format.Font.Size = new Unit(20);
                    paragraph5.AddText(expDate);
                    paragraph5.Format.Font.Name = "Impact";
                    paragraph5.Format.Font.Bold = true;
                    paragraph5.Format.SpaceAfter = "0.2cm";
                    paragraph5.Format.Font.Color = Colors.Red;


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
                        return File(stream, "application/pdf", "T_" + _card.partnerReference + ".pdf");
                    }
                }
                else {
                    // De no ser operador o no contener OP-
                    // Obtenemos la informacion del operador.
                    //int _id = 0;
                    //Int32.TryParse(_card.partnerReference, out _id);
                    Partner _operator = new Partner().get(_card.partnerReference);
                    // Obtenemos la imagen del operador
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
                    var result = writer.Write("T-" + _card.id);
                    var result2 = writer2.Write("T-" + _card.id);
                    System.Drawing.Bitmap qrCodeImage = new System.Drawing.Bitmap(result); // CODIGO QR
                    System.Drawing.Bitmap BarCodeImage = new System.Drawing.Bitmap(result2); // CODIGO DE BARRAS
                                                                                             // Obtenida toda la informacion procedemos a renderizar el tarjeton...
                    Document document = new Document();
                    MigraDoc.DocumentObjectModel.Section section = document.AddSection();
                    section.PageSetup.Orientation = Orientation.Landscape;
                    section.PageSetup.PageHeight = "19cm";
                    section.PageSetup.PageWidth = "14cm";

                    Image myImage = section.Headers.Primary.AddImage(Server.MapPath("~/Content/images/tarjeton.png"));
                    myImage.Height = "14cm";
                    myImage.Width = "19cm";
                    myImage.RelativeVertical = RelativeVertical.Page;
                    myImage.RelativeHorizontal = RelativeHorizontal.Page;
                    myImage.WrapFormat.Style = WrapStyle.Through;

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
                        image.Height = "6.8cm";
                        image.Width = "5.6cm";
                        image.Top = "3.92cm";
                        image.Left = "12.6cm";
                    }
                    else
                    {
                        image.Height = "6.8cm";
                        image.Width = "5.6cm";
                        image.Top = "3.92cm";
                        image.Left = "12.6cm";
                    }
                    image.RelativeVertical = RelativeVertical.Page;
                    image.RelativeHorizontal = RelativeHorizontal.Page;
                    image.WrapFormat.Style = WrapStyle.Through;

                    //Footer QR code
                    Image objImgLogo = section.AddImage(AmazonHelper.MigraDocFilenameFromByteArray(AmazonHelper.ToByteArray(qrCodeImage)));
                    objImgLogo.Height = "3.5cm";
                    objImgLogo.Width = "3.5cm";
                    objImgLogo.RelativeVertical = RelativeVertical.Page;
                    objImgLogo.RelativeHorizontal = RelativeHorizontal.Page;
                    objImgLogo.Top = "7.3cm";
                    objImgLogo.Left = "0.78cm";
                    objImgLogo.WrapFormat.Style = WrapStyle.Through;

                    // Codigo de barras
                    Image objImgBarcode = section.AddImage(AmazonHelper.MigraDocFilenameFromByteArray(AmazonHelper.ToByteArray(BarCodeImage)));
                    objImgBarcode.Height = "1.4cm";
                    objImgBarcode.Width = "4.2cm";
                    objImgBarcode.RelativeVertical = RelativeVertical.Page;
                    objImgBarcode.RelativeHorizontal = RelativeHorizontal.Page;
                    objImgBarcode.Top = "12.2cm";
                    objImgBarcode.Left = "0.5cm";
                    objImgBarcode.WrapFormat.Style = WrapStyle.Through;
                    
                    // Nombre...
                    TextFrame tf = section.AddTextFrame();
                    tf.Width = "138mm";
                    tf.Height = "50mm";
                    tf.RelativeVertical = RelativeVertical.Page;
                    tf.RelativeHorizontal = RelativeHorizontal.Page;
                    tf.Top = "3.5cm";
                    tf.Left = "1cm";
                    Paragraph paragraph = tf.AddParagraph();
                    paragraph.Format.Font.Size = new Unit(36);
                    paragraph.AddText(_operator.firstName.ToUpper());
                    paragraph.Format.Font.Name = "Impact";
                    paragraph.Format.Font.Bold = true;
                    paragraph.Format.SpaceAfter = "0.2cm";
                    paragraph.Format.Font.Color = Colors.Red;

                    // Apellido Paterno
                    TextFrame tf1 = section.AddTextFrame();
                    tf1.Width = "138mm";
                    tf1.Height = "50mm";
                    tf1.RelativeVertical = RelativeVertical.Page;
                    tf1.RelativeHorizontal = RelativeHorizontal.Page;
                    tf1.Top = "4.8cm";
                    tf1.Left = "1cm";
                    Paragraph paragraph1 = tf1.AddParagraph();
                    paragraph1.Format.Font.Size = new Unit(36);
                    paragraph1.AddText(_operator.lastNameF.ToUpper());
                    paragraph1.Format.Font.Name = "Impact";
                    paragraph1.Format.Font.Bold = true;
                    paragraph1.Format.SpaceAfter = "0.2cm";
                    paragraph1.Format.Font.Color = Colors.Red;

                    // Apellido Materno
                    TextFrame tf2 = section.AddTextFrame();
                    tf2.Width = "138mm";
                    tf2.Height = "50mm";
                    tf2.RelativeVertical = RelativeVertical.Page;
                    tf2.RelativeHorizontal = RelativeHorizontal.Page;
                    tf2.Top = "6.1cm";
                    tf2.Left = "1cm";
                    Paragraph paragraph2 = tf2.AddParagraph();
                    paragraph2.Format.Font.Size = new Unit(36);
                    paragraph2.AddText(_operator.lastNameM.ToUpper());
                    paragraph2.Format.Font.Name = "Impact";
                    paragraph2.Format.Font.Bold = true;
                    paragraph2.Format.SpaceAfter = "0.2cm";
                    paragraph2.Format.Font.Color = Colors.Red;

                    //numero de operador
                    TextFrame tf3 = section.AddTextFrame();
                    tf3.Width = "138mm";
                    tf3.Height = "50mm";
                    tf3.RelativeVertical = RelativeVertical.Page;
                    tf3.RelativeHorizontal = RelativeHorizontal.Page;
                    tf3.Top = "11.5cm";
                    tf3.Left = "12.8cm";
                    Paragraph paragraph3 = tf3.AddParagraph();
                    paragraph3.Format.Font.Size = new Unit(38);
                    paragraph3.AddText(_card.partnerReference);
                    paragraph3.Format.Font.Name = "Impact";
                    paragraph3.Format.Font.Bold = true;
                    paragraph3.Format.SpaceAfter = "0.2cm";
                    paragraph3.Format.Font.Color = Colors.Red;

                    // Numero de taxi
                    TextFrame tf4 = section.AddTextFrame();
                    tf4.Width = "210mm";
                    tf4.Height = "85mm";
                    tf4.RelativeVertical = RelativeVertical.Page;
                    tf4.RelativeHorizontal = RelativeHorizontal.Page;
                    tf4.Top = "8.9cm";
                    tf4.Left = "7cm";
                    Paragraph paragraph4 = tf4.AddParagraph();
                    paragraph4.Format.Font.Size = new Unit(40);
                    //paragraph4.AddText(collection.Get("taxiNumber").PadLeft(4, '0'));
                    paragraph4.AddText(_card.taxi.PadLeft(4, '0'));
                    paragraph4.Format.Font.Name = "Impact";
                    paragraph4.Format.Font.Bold = true;
                    paragraph4.Format.Font.Color = Colors.Red;

                    // Placas taxi
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
                    paragraph6.Format.Font.Color = Colors.Red;

                    // VIGENCIA
                    TextFrame tf5 = section.AddTextFrame();
                    tf5.Width = "138mm";
                    tf5.Height = "50mm";
                    tf5.RelativeVertical = RelativeVertical.Page;
                    tf5.RelativeHorizontal = RelativeHorizontal.Page;
                    tf5.Top = "11.3cm";
                    tf5.Left = ".9cm";
                    Paragraph paragraph5 = tf5.AddParagraph();
                    paragraph5.Format.Font.Size = new Unit(20);
                    paragraph5.AddText(expDate);
                    paragraph5.Format.Font.Name = "Impact";
                    paragraph5.Format.Font.Bold = true;
                    paragraph5.Format.SpaceAfter = "0.2cm";
                    paragraph5.Format.Font.Color = Colors.Red;

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
                        return File(stream, "application/pdf", "T_" + _card.partnerReference + ".pdf");
                    }
                }
            }
            catch (Exception ex)
            {
                // ENVIAR A LA VISTA DE ERROR (GERMAN)
                throw;
            }
        }
        
        // GET: card/Details/5
        public ActionResult Details(int id)
        {

            ///////// MENSAJE DE EDIT //////////////
            ViewBag.messageType = null;
            ViewBag.message = null;
            // Mensaje de otra vista.
            if (this.Message != null)
            {
                ViewBag.messageType = this.Message.Type;
                ViewBag.message = this.Message.Message;
                this.Message = null;
            }
            /////////////////////////////

            card card = new card();
            card = card.get(id);
            return View(card);
        }

        // GET: card/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: card/Create
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

        // GET: card/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: card/Delete/5
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

        // DATOS JSON DE EL OPERAODR
        [SessionFilter("Admin")]
        [HttpGet]
        [Route("json/get/{id}")]
        public JsonResult get(int id)
        {
            try
            {
                card card = new card();
                card = card.get(id);
                string resultset = "";
                return Json(resultset, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
