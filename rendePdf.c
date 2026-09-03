        /////////// AVER
        public void GenerarImagenPrecioRotado(string texto, string rutaImagen)
        {
            using (System.Drawing.Font fuente = new System.Drawing.Font("Roboto", 23, FontStyle.Bold)) // fuente más grande
            {
                // Crear imagen temporal para medir
                using (Bitmap tempBmp = new Bitmap(1, 1))
                using (Graphics gTemp = Graphics.FromImage(tempBmp))
                {
                    SizeF size = gTemp.MeasureString(texto, fuente);

                    int ancho = (int)Math.Ceiling(size.Width);
                    int alto = (int)Math.Ceiling(size.Height);

                    // Invertimos dimensiones porque vamos a rotar
                    using (Bitmap bmp = new Bitmap(alto, ancho, PixelFormat.Format32bppArgb))
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(System.Drawing.Color.Transparent); // Fondo transparente

                        g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        // Rotar sistema de coordenadas
                        g.TranslateTransform(0, bmp.Height);
                        g.RotateTransform(-90);

                        using (SolidBrush pincel = new SolidBrush(System.Drawing.Color.FromArgb(165, 0, 0)))
                        {
                            g.DrawString(texto, fuente, pincel, 0, 0);
                        }

                        bmp.Save(rutaImagen, ImageFormat.Png);
                    }
                }
            }
        }
        public void GenerarImagenTipoRotado(string texto, string rutaImagen2)
        {
            using (System.Drawing.Font fuente = new System.Drawing.Font("Roboto", 60, FontStyle.Bold)) // fuente más grande
            {
                // Crear imagen temporal para medir
                using (Bitmap tempBmp = new Bitmap(1, 1))
                using (Graphics gTemp = Graphics.FromImage(tempBmp))
                {
                    SizeF size = gTemp.MeasureString(texto, fuente);

                    int ancho = (int)Math.Ceiling(size.Width);
                    int alto = (int)Math.Ceiling(size.Height);

                    // Invertimos dimensiones porque vamos a rotar
                    using (Bitmap bmp = new Bitmap(alto, ancho, PixelFormat.Format32bppArgb))
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(System.Drawing.Color.Transparent); // Fondo transparente
                        g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        // Rotar sistema de coordenadas
                        g.TranslateTransform(0, bmp.Height);
                        g.RotateTransform(-90);
                        using (SolidBrush pincel = new SolidBrush(System.Drawing.Color.FromArgb(0, 0, 0)))
                        {
                            g.DrawString(texto, fuente, pincel, 0, 0);
                        }
                        bmp.Save(rutaImagen2, ImageFormat.Png);
                    }
                }
            }
        }
        public void GenerarImagenFechaRotado(string texto, string rutaImagen3)
        {
            using (System.Drawing.Font fuente = new System.Drawing.Font("Roboto", 14, FontStyle.Regular)) // fuente más grande
            {
                // Crear imagen temporal para medir
                using (Bitmap tempBmp = new Bitmap(1, 1))
                using (Graphics gTemp = Graphics.FromImage(tempBmp))
                {
                    SizeF size = gTemp.MeasureString(texto, fuente);
                    int ancho = (int)Math.Ceiling(size.Width);
                    int alto = (int)Math.Ceiling(size.Height);

                    // Invertimos dimensiones porque vamos a rotar
                    using (Bitmap bmp = new Bitmap(alto, ancho, PixelFormat.Format32bppArgb))
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(System.Drawing.Color.Transparent); // Fondo transparente
                        g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        // Rotar sistema de coordenadas
                        g.TranslateTransform(0, bmp.Height);
                        g.RotateTransform(-90);
                        using (SolidBrush pincel = new SolidBrush(System.Drawing.Color.FromArgb(165, 0, 0)))
                        {
                            g.DrawString(texto, fuente, pincel, 0, 0);
                        }
                        bmp.Save(rutaImagen3, ImageFormat.Png);
                    }
                }
            }
        }
        public string pdfsharpRotateText(string text, string fontname, float fontsize, FontStyle fontstyle, SolidBrush color, int rotate = -90) {
            try
            {
                string route = Server.MapPath("~/Temp/" + this.GenerarCadenaAleatoria(10) + ".png");
                System.Drawing.Font font = new System.Drawing.Font(fontname, fontsize, fontstyle);
                // Crear imagen temporal para medir
                using (Bitmap tempBmp = new Bitmap(1, 1))
                using (Graphics gTemp = Graphics.FromImage(tempBmp))
                {
                    SizeF size = gTemp.MeasureString(text, font);
                    int ancho = (int)Math.Ceiling(size.Width);
                    int alto = (int)Math.Ceiling(size.Height);
                    // Invertimos dimensiones porque vamos a rotar
                    using (Bitmap bmp = new Bitmap(alto, ancho, PixelFormat.Format32bppArgb))
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(System.Drawing.Color.Transparent); // Fondo transparente
                        g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        // Rotar sistema de coordenadas
                        g.TranslateTransform(0, bmp.Height);
                        g.RotateTransform(rotate);
                        g.DrawString(text, font, color, 0, 0);
                        bmp.Save(route, ImageFormat.Png);
                    }
                }
                return route;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /////////// PDF CUPONES
        [HttpGet]
        [Route("exportpdf/{key}")]
        public ActionResult renderpdf(string key)
        {
            cuponsGroupPrinted obj = new cuponsGroupPrinted();
            List<cuponsGroupPrintedDTO> datos = obj.ordes(key);
            int mod = 1;// Contador de cupon.
            int page = 1;// Contador de pagina
            int position = 1;//Contador de posicion en la hoja.
            Image _background = new Image();
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

            Document document = new Document();
            // Agregamos la primer pagina por caso trivial.
            MigraDoc.DocumentObjectModel.Section CurrentSection = document.AddSection();
            CurrentSection.PageSetup.Orientation = Orientation.Landscape;
            CurrentSection.PageSetup.PageHeight = "279.4mm";
            CurrentSection.PageSetup.PageWidth = "215.9mm";
            CurrentSection.PageSetup.BottomMargin = 0;
            CurrentSection.PageSetup.RightMargin = 0;
            CurrentSection.PageSetup.TopMargin = 0;
            CurrentSection.PageSetup.LeftMargin = 0;
            foreach (cuponsGroupPrintedDTO groups in datos)
            {
                foreach (CuponDTO cupon in groups.Cupons)
                {



                    if (position == 1 && page != 1)
                    {
                        // Sección nueva al empezar una nueva hoja (después de completar 4 cupones)
                        CurrentSection = document.AddSection();
                        CurrentSection.PageSetup.Orientation = Orientation.Landscape;
                        CurrentSection.PageSetup.PageHeight = "279.4mm";
                        CurrentSection.PageSetup.PageWidth = "215.9mm";
                        CurrentSection.PageSetup.BottomMargin = 0;
                        CurrentSection.PageSetup.RightMargin = 0;
                        CurrentSection.PageSetup.TopMargin = 0;
                        CurrentSection.PageSetup.LeftMargin = 0;
                    }

                    // Datos del cupon....
                    // $1000
                    string valor = cupon.Couponrate.Valor.ToString("F0");
                    // A1
                    string tipo = cupon.Couponrate.Cupontype.Name;
                    // fecha de vencimiento agregamos 90 dias despues de que se imprimio.
                    //DateTime? vencimiento = cupon.cuponsGroupPrinted.PrintedDate?.AddDays(90);

                    // Folio 
                    string folio = cupon.CuponsID.ToString();

                    // Folio TOP
                    TextFrame Fo = CurrentSection.AddTextFrame();
                    Fo.Width = "150mm";
                    Fo.Height = "20mm";
                    Fo.RelativeVertical = RelativeVertical.Page;
                    Fo.RelativeHorizontal = RelativeHorizontal.Page;

                    Paragraph FoT = Fo.AddParagraph(); // Se agrega al TextFrame correcto
                    FoT.Format.Font.Size = new Unit(8);
                    //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
                    FoT.AddFormattedText(folio);
                    FoT.Format.Font.Name = "Roboto";
                    FoT.Format.Font.Bold = false;
                    FoT.Format.SpaceAfter = "0.1cm";
                    FoT.Format.Font.Color = Color.FromRgb(165, 0, 0); // Naranja personalizado

                    // Folio middle
                    TextFrame FoMiddle = CurrentSection.AddTextFrame();
                    FoMiddle.Width = "150mm";
                    FoMiddle.Height = "20mm";
                    FoMiddle.RelativeVertical = RelativeVertical.Page;
                    FoMiddle.RelativeHorizontal = RelativeHorizontal.Page;

                    Paragraph FoM = FoMiddle.AddParagraph(); // Se agrega al TextFrame correcto
                    FoM.Format.Font.Size = new Unit(8);
                    //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
                    FoM.AddFormattedText(folio);
                    FoM.Format.Font.Name = "Roboto";
                    FoM.Format.Font.Bold = false;
                    FoM.Format.SpaceAfter = "0.1cm";
                    FoM.Format.Font.Color = Color.FromRgb(165, 0, 0); // Naranja personalizado

                    // Folio footer
                    TextFrame FoFotter = CurrentSection.AddTextFrame();
                    FoFotter.Width = "150mm";
                    FoFotter.Height = "20mm";
                    FoFotter.RelativeVertical = RelativeVertical.Page;
                    FoFotter.RelativeHorizontal = RelativeHorizontal.Page;

                    Paragraph FoF = FoFotter.AddParagraph(); // Se agrega al TextFrame correcto
                    FoF.Format.Font.Size = new Unit(8);
                    //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
                    FoF.AddFormattedText(folio);
                    FoF.Format.Font.Name = "Roboto";
                    FoF.Format.Font.Bold = false;
                    FoF.Format.SpaceAfter = "0.1cm";
                    FoF.Format.Font.Color = Color.FromRgb(165, 0, 0); // Naranja personalizado

                    // tipo TOP
                    TextFrame TipoT = CurrentSection.AddTextFrame();
                    TipoT.Width = "150mm";
                    TipoT.Height = "20mm";
                    TipoT.RelativeVertical = RelativeVertical.Page;
                    TipoT.RelativeHorizontal = RelativeHorizontal.Page;

                    Paragraph TipoTop = TipoT.AddParagraph(); // Se agrega al TextFrame correcto
                    TipoTop.Format.Font.Size = new Unit(19);
                    //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
                    TipoTop.AddFormattedText(tipo);
                    TipoTop.Format.Font.Name = "Roboto";
                    TipoTop.Format.Font.Bold = true;
                    TipoTop.Format.SpaceAfter = "0.1cm";
                    TipoTop.Format.Font.Color = Color.FromRgb(0, 0, 0); // Naranja personalizado

                    // tipo footer
                    TextFrame TipoFotter = CurrentSection.AddTextFrame();
                    TipoFotter.Width = "150mm";
                    TipoFotter.Height = "20mm";
                    TipoFotter.RelativeVertical = RelativeVertical.Page;
                    TipoFotter.RelativeHorizontal = RelativeHorizontal.Page;

                    Paragraph TipoF = TipoFotter.AddParagraph(); // Se agrega al TextFrame correcto
                    TipoF.Format.Font.Size = new Unit(19);
                    //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
                    TipoF.AddFormattedText(tipo);
                    TipoF.Format.Font.Name = "Roboto";
                    TipoF.Format.Font.Bold = true;
                    TipoF.Format.SpaceAfter = "0.1cm";
                    TipoF.Format.Font.Color = Color.FromRgb(0, 0, 0); // Naranja personalizado

                    //Vencimiento
                    TextFrame Ven = CurrentSection.AddTextFrame();
                    Ven.Width = "150mm";
                    Ven.Height = "20mm";
                    Ven.RelativeVertical = RelativeVertical.Page;
                    Ven.RelativeHorizontal = RelativeHorizontal.Page;

                    Paragraph VenT = Ven.AddParagraph(); // Se agrega al TextFrame correcto
                    VenT.Format.Font.Size = new Unit(6);
                    //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
                    VenT.AddFormattedText("03/11/2025");
                    VenT.Format.Font.Name = "Roboto";
                    VenT.Format.Font.Bold = true;
                    VenT.Format.SpaceAfter = "0.1cm";
                    VenT.Format.Font.Color = Color.FromRgb(165, 0, 0); // Naranja personalizado

                    TextFrame VenFotter = CurrentSection.AddTextFrame();
                    VenFotter.Width = "150mm";
                    VenFotter.Height = "20mm";
                    VenFotter.RelativeVertical = RelativeVertical.Page;
                    VenFotter.RelativeHorizontal = RelativeHorizontal.Page;
                    VenT.Format.Font.Color = Color.FromRgb(165, 0, 0); // Naranja personalizado

                    Paragraph VenF = VenFotter.AddParagraph(); // Se agrega al TextFrame correcto
                    VenF.Format.Font.Size = new Unit(6);
                    //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
                    VenF.AddFormattedText("03/11/2025");
                    VenF.Format.Font.Name = "Roboto";
                    VenF.Format.Font.Bold = true;
                    VenF.Format.SpaceAfter = "0.1cm";
                    VenF.Format.Font.Color = Color.FromRgb(165, 0, 0); // Naranja personalizado

                    // PRECIO
                    TextFrame Pre = CurrentSection.AddTextFrame();
                    Pre.Width = "150mm";
                    Pre.Height = "20mm";
                    Pre.RelativeVertical = RelativeVertical.Page;
                    Pre.RelativeHorizontal = RelativeHorizontal.Page;

                    Paragraph PreT = Pre.AddParagraph(); // Se agrega al TextFrame correcto
                    PreT.Format.Font.Size = new Unit(15);
                    //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
                    PreT.AddFormattedText("$" + valor + " MXN");
                    PreT.Format.Font.Name = "Roboto";
                    PreT.Format.Font.Bold = true;
                    PreT.Format.SpaceAfter = "0.1cm";
                    PreT.Format.Font.Color = Color.FromRgb(165, 0, 0); // Naranja personalizado

                    TextFrame PreF = CurrentSection.AddTextFrame();
                    PreF.Width = "150mm";
                    PreF.Height = "20mm";
                    PreF.RelativeVertical = RelativeVertical.Page;
                    PreF.RelativeHorizontal = RelativeHorizontal.Page;

                    Paragraph PreFooter = PreF.AddParagraph(); // Se agrega al TextFrame correcto
                    PreFooter.Format.Font.Size = new Unit(15);
                    //paragraphv.Format.Alignment = ParagraphAlignment.Center; // Centrar el texto
                    PreFooter.AddFormattedText("$" + valor + " MXN");
                    PreFooter.Format.Font.Name = "Roboto";
                    PreFooter.Format.Font.Bold = true;
                    PreFooter.Format.SpaceAfter = "0.1cm";
                    PreFooter.Format.Font.Color = Color.FromRgb(165, 0, 0); // Naranja personalizado

                    // Generamos los codigos QR con los Writers
                    // Prefijo e de express
                    var result = writer.Write("k" + cupon.CuponsID);
                    var result2 = writer2.Write("k" + cupon.CuponsID);
                    System.Drawing.Bitmap qrCodeImage = new System.Drawing.Bitmap(result); // CODIGO QR

                    string QRCODE = AmazonHelper.MigraDocFilenameFromByteArray(AmazonHelper.ToByteArray(qrCodeImage));
                    Image Qr = CurrentSection.AddImage(QRCODE);
                    Qr.Height = "20mm";
                    Qr.Width = "20mm";
                    Qr.RelativeVertical = RelativeVertical.Page;
                    Qr.RelativeHorizontal = RelativeHorizontal.Page;
                    Image QrMiddle = CurrentSection.AddImage(QRCODE);
                    QrMiddle.Height = "35mm";
                    QrMiddle.Width = "35mm";
                    QrMiddle.RelativeVertical = RelativeVertical.Page;
                    QrMiddle.RelativeHorizontal = RelativeHorizontal.Page;
                    Image QrFooter = CurrentSection.AddImage(QRCODE);
                    QrFooter.Height = "20mm";
                    QrFooter.Width = "20mm";
                    QrFooter.RelativeVertical = RelativeVertical.Page;
                    QrFooter.RelativeHorizontal = RelativeHorizontal.Page;


                    // EL VALOR.
                    string Precio = "$" + valor + " MXN";
                    string imgValor = pdfsharpRotateText(
                        Precio,
                        "Roboto",
                        23,
                        FontStyle.Bold,
                        new SolidBrush(System.Drawing.Color.FromArgb(165, 0, 0))
                    );
                    // Precio Rotada
                    MigraDoc.DocumentObjectModel.Shapes.Image imgPrecioRotada = CurrentSection.AddImage(imgValor);
                    imgPrecioRotada.RelativeVertical = RelativeVertical.Page;
                    imgPrecioRotada.RelativeHorizontal = RelativeHorizontal.Page;


                    // EL FOLIO.
                    string FOLIO = folio;
                    string imgFolio = pdfsharpRotateText(
                        FOLIO,
                        "Roboto",
                        15,
                        FontStyle.Bold,
                        new SolidBrush(System.Drawing.Color.FromArgb(165, 0, 0))
                    );
                    // Precio Rotada
                    MigraDoc.DocumentObjectModel.Shapes.Image _imgFolio = CurrentSection.AddImage(imgFolio);
                    _imgFolio.RelativeVertical = RelativeVertical.Page;
                    _imgFolio.RelativeHorizontal = RelativeHorizontal.Page;

                    // FECHA
                    string Fecha = "03/11/2025";
                    string imgFecha = pdfsharpRotateText(
                        Fecha,
                        "Roboto",
                        14,
                        FontStyle.Bold,
                        new SolidBrush(System.Drawing.Color.FromArgb(165, 0, 0))
                    );
                    // Precio Rotada
                    MigraDoc.DocumentObjectModel.Shapes.Image _imgFecha = CurrentSection.AddImage(imgFecha);
                    _imgFecha.RelativeVertical = RelativeVertical.Page;
                    _imgFecha.RelativeHorizontal = RelativeHorizontal.Page;

                    // TIPO
                    string Tipo = tipo;
                    string imgTipo = pdfsharpRotateText(
                        Tipo,
                        "Roboto",
                        60,
                        FontStyle.Bold,
                        new SolidBrush(System.Drawing.Color.FromArgb(0, 0, 0))
                    );
                    // Precio Rotada
                    MigraDoc.DocumentObjectModel.Shapes.Image _imgTipo = CurrentSection.AddImage(imgTipo);
                    _imgTipo.RelativeVertical = RelativeVertical.Page;
                    _imgTipo.RelativeHorizontal = RelativeHorizontal.Page;



                    // POSISICONES
                    // Posicionar según layout horizontal en 4 columnas
                    switch (position)
                    {
                        case 1:
                            // diseño de cupon (Color hotel etc.)
                            _background = CurrentSection.Headers.Primary.AddImage(Server.MapPath("~/Content/images/Cupones/unlimitedMedia.jpg"));
                            _background.RelativeVertical = RelativeVertical.Page;
                            _background.RelativeHorizontal = RelativeHorizontal.Page;
                            _background.Width = "69.9mm";
                            _background.Height = "215.9mm";
                            _background.WrapFormat.Style = WrapStyle.Through;
                            // Posicion del diseño.
                            _background.Top = "0mm";
                            _background.Left = "0mm";

                            // Posicion del qr
                            Qr.Top = "22mm";
                            Qr.Left = "2mm";
                            QrMiddle.Top = "130mm";
                            QrMiddle.Left = "30mm";
                            QrFooter.Top = "193mm";
                            QrFooter.Left = "2mm";

                            //Folio
                            Fo.Top = "19mm"; // Misma altura
                            Fo.Left = "4mm"; // Posición horizontal
                            //FoMiddle.Top = "190mm"; // Misma altura
                            //FoMiddle.Left = "4mm"; // Posición horizontal
                            FoFotter.Top = "190.2mm"; // Misma altura
                            FoFotter.Left = "4mm"; // Posición horizontal

                            //Tipo
                            TipoT.Top = "16.7mm"; // Misma altura
                            TipoT.Left = "52.8mm"; // Posición horizontal
                            TipoFotter.Top = "187.9mm"; // Misma altura
                            TipoFotter.Left = "52.7mm"; // Posición horizontal

                            //VENCIMIENTO
                            Ven.Top = "38mm"; // Misma altura
                            Ven.Left = "48mm"; // Posición horizontal
                            VenFotter.Top = "209.2mm"; // Misma altura
                            VenFotter.Left = "47.7mm"; // Posición horizontal

                            // PRECIO
                            Pre.Top = "23.4mm"; // Misma altura
                            Pre.Left = "29.4mm"; // Posición horizontal
                            PreF.Top = "194.6mm"; // Misma altura
                            PreF.Left = "29.2mm"; // Posición horizontal

                            // POSICION DE TEXTO ROTADO CASE
                            imgPrecioRotada.Top = "47mm";
                            imgPrecioRotada.Left = "25.3mm";

                            _imgFolio.Top = "131.6mm";
                            _imgFolio.Left = "26mm";

                            _imgFecha.Top = "55.5mm";
                            _imgFecha.Left = "60.5mm";

                            _imgTipo.Top = "131.3mm";
                            _imgTipo.Left = "-1.5mm";

                            break;
                        case 2:

                            // diseño de cupon (Color hotel etc.)
                            _background = CurrentSection.Headers.Primary.AddImage(Server.MapPath("~/Content/images/Cupones/unlimitedMedia.jpg"));
                            _background.RelativeVertical = RelativeVertical.Page;
                            _background.RelativeHorizontal = RelativeHorizontal.Page;
                            _background.Width = "69.9mm";
                            _background.Height = "215.9mm";
                            _background.WrapFormat.Style = WrapStyle.Through;
                            // Posision del diseño.
                            _background.Top = "0mm";
                            _background.Left = "69.9mm";

                            // Posicion del qr
                            Qr.Top = "22mm";
                            Qr.Left = "71.9mm";
                            QrMiddle.Top = "130mm";
                            QrMiddle.Left = "99.9mm";
                            QrFooter.Top = "193mm";
                            QrFooter.Left = "71.9mm";

                            //Folio
                            Fo.Top = "19mm"; // Misma altura
                            Fo.Left = "73.9mm"; // Posición horizontal
                            //FoMiddle.Top = "190mm"; // Misma altura
                            //FoMiddle.Left = "4mm"; // Posición horizontal
                            FoFotter.Top = "190.2mm"; // Misma altura
                            FoFotter.Left = "73.9mm"; // Posición horizontal

                            //Tipo
                            TipoT.Top = "16.7mm"; // Misma altura
                            TipoT.Left = "122.7mm"; // Posición horizontal
                            TipoFotter.Top = "187.9mm"; // Misma altura
                            TipoFotter.Left = "122.6mm"; // Posición horizontal

                            //VENCIMIENTO
                            Ven.Top = "38mm"; // Misma altura
                            Ven.Left = "117.9mm"; // Posición horizontal
                            VenFotter.Top = "209.2mm"; // Misma altura
                            VenFotter.Left = "117.6mm"; // Posición horizontal

                            // PRECIO
                            Pre.Top = "23.4mm"; // Misma altura
                            Pre.Left = "99.3mm"; // Posición horizontal
                            PreF.Top = "194.6mm"; // Misma altura
                            PreF.Left = "99.1mm"; // Posición horizontal

                            // POSICION DE TEXTO ROTADO CASE
                            imgPrecioRotada.Top = "47mm";
                            imgPrecioRotada.Left = "95.2mm";

                            _imgFolio.Top = "131.6mm";
                            _imgFolio.Left = "95.9mm";

                            _imgFecha.Top = "55.5mm";
                            _imgFecha.Left = "130.4mm";

                            _imgTipo.Top = "131.3mm";
                            _imgTipo.Left = "68.45mm";

                            break;
                        case 3:

                            // diseño de cupon (Color hotel etc.)
                            _background = CurrentSection.Headers.Primary.AddImage(Server.MapPath("~/Content/images/Cupones/unlimitedMedia.jpg"));
                            _background.RelativeVertical = RelativeVertical.Page;
                            _background.RelativeHorizontal = RelativeHorizontal.Page;
                            _background.Width = "69.9mm";
                            _background.Height = "215.9mm";
                            _background.WrapFormat.Style = WrapStyle.Through;
                            // Posision del diseño.
                            _background.Top = "0mm";
                            _background.Left = "139.8mm";
                            // Posicion del qr
                            Qr.Top = "22mm";
                            Qr.Left = "141.8mm";
                            QrMiddle.Top = "130mm";
                            QrMiddle.Left = "169.8mm";
                            QrFooter.Top = "193mm";
                            QrFooter.Left = "141.8mm";

                            //Folio
                            Fo.Top = "19mm"; // Misma altura
                            Fo.Left = "143.8mm"; // Posición horizontal
                            //FoMiddle.Top = "190mm"; // Misma altura
                            //FoMiddle.Left = "4mm"; // Posición horizontal
                            FoFotter.Top = "190.2mm"; // Misma altura
                            FoFotter.Left = "143.8mm"; // Posición horizontal

                            //Tipo
                            TipoT.Top = "16.7mm"; // Misma altura
                            TipoT.Left = "192.6mm"; // Posición horizontal
                            TipoFotter.Top = "187.9mm"; // Misma altura
                            TipoFotter.Left = "192.5mm"; // Posición horizontal

                            //VENCIMIENTO
                            Ven.Top = "38mm"; // Misma altura
                            Ven.Left = "187.8mm"; // Posición horizontal
                            VenFotter.Top = "209.2mm"; // Misma altura
                            VenFotter.Left = "187.6mm"; // Posición horizontal

                            // PRECIO
                            Pre.Top = "23.4mm"; // Misma altura
                            Pre.Left = "169.2mm"; // Posición horizontal
                            PreF.Top = "194.6mm"; // Misma altura
                            PreF.Left = "169mm"; // Posición horizontal

                            // POSICION DE TEXTO ROTADO CASE
                            imgPrecioRotada.Top = "47mm";
                            imgPrecioRotada.Left = "165.1mm";

                            _imgFolio.Top = "131.6mm";
                            _imgFolio.Left = "165.8mm";

                            _imgFecha.Top = "55.5mm";
                            _imgFecha.Left = "200.3mm";

                            _imgTipo.Top = "131.3mm";
                            _imgTipo.Left = "138.3mm";
                            break;
                        case 4:

                            // diseño de cupon (Color hotel etc.)
                            _background = CurrentSection.Headers.Primary.AddImage(Server.MapPath("~/Content/images/Cupones/unlimitedMedia.jpg"));
                            _background.RelativeVertical = RelativeVertical.Page;
                            _background.RelativeHorizontal = RelativeHorizontal.Page;
                            _background.Width = "69.9mm";
                            _background.Height = "215.9mm";
                            _background.WrapFormat.Style = WrapStyle.Through;
                            // Posision del diseño.
                            _background.Top = "0mm";
                            _background.Left = "209.7mm";
                            // Posicion del qr
                            Qr.Top = "22mm";
                            Qr.Left = "211.7mm";
                            QrMiddle.Top = "130mm";
                            QrMiddle.Left = "239.7mm";
                            QrFooter.Top = "193mm";
                            QrFooter.Left = "211.7mm";

                            //VENCIMIENTO
                            Ven.Top = "38mm"; // Misma altura
                            Ven.Left = "257.7mm"; // Posición horizontal
                            VenFotter.Top = "209.2mm"; // Misma altura
                            VenFotter.Left = "257.4mm"; // Posición horizontal

                            //Folio
                            Fo.Top = "19mm"; // Misma altura
                            Fo.Left = "213.7mm"; // Posición horizontal
                            FoFotter.Top = "190.2mm"; // Misma altura
                            FoFotter.Left = "213.7mm"; // Posición horizontal

                            //Tipo
                            TipoT.Top = "16.7mm"; // Misma altura
                            TipoT.Left = "262.5mm"; // Posición horizontal
                            TipoFotter.Top = "187.9mm"; // Misma altura
                            TipoFotter.Left = "262.4mm"; // Posición horizontal

                            // PRECIO
                            Pre.Top = "23.4mm"; // Misma altura
                            Pre.Left = "239.1mm"; // Posición horizontal
                            PreF.Top = "194.6mm"; // Misma altura
                            PreF.Left = "238.9mm"; // Posición horizontal

                            // POSICION DE TEXTO ROTADO CASE
                            imgPrecioRotada.Top = "47mm";
                            imgPrecioRotada.Left = "235mm";

                            _imgFolio.Top = "131.6mm";
                            _imgFolio.Left = "236.7mm";

                            _imgFecha.Top = "55.5mm";
                            _imgFecha.Left = "270.2mm";

                            _imgTipo.Top = "131.3mm";
                            _imgTipo.Left = "208.2mm";

                            break;
                    }

                    // Incrementar posición y verificar si es necesario reiniciar
                    if (position == 4)
                    {
                        position = 1;
                        page++;
                    }
                    else
                    {
                        position++;
                    }
                }
            }

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