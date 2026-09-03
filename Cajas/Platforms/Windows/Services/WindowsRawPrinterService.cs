#if WINDOWS

using Cajas.MVVM.Models;
using Cajas.Services;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using GraphicsState = System.Drawing.Drawing2D.GraphicsState;
using System.Drawing.Imaging;

// Alias explícitos para evitar conflictos con Microsoft.Maui.Graphics
using Bitmap = System.Drawing.Bitmap;
using Brush = System.Drawing.Brush;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;
using PointF = System.Drawing.PointF;
using Size = System.Drawing.Size;
using SizeF = System.Drawing.SizeF;
using System.Runtime.InteropServices;
using System.Text;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace Cajas.Platforms.Windows;

public sealed class WindowsRawPrinterService : ITicketPrinterService
{
    private const int TicketWidthPx = 576;
    private const int OuterMargin = 18;
    private const int LeftPanelWidth = 182;
    private const int ColumnGap = 10;
    private const int FeedLinesBeforeCut = 7;

    public WindowsRawPrinterService()
    {
    }

    public async Task<PrintTicketResult> PrintCouponPaymentAsync(
        CouponPaymentTicket ticket,
        CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(ticket);
            cancellationToken.ThrowIfCancellationRequested();

            byte[] data = await BuildTicketAsync(ticket, cancellationToken);
            string printerName = GetDefaultPrinterName();

            SendBytesToPrinter(
                printerName,
                data,
                $"Pago de cupones #{ticket.PaymentId}");

            return PrintTicketResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Error imprimiendo ticket: {ex}");

            return PrintTicketResult.Error(ex.Message);
        }
    }

    private static async Task<byte[]> BuildTicketAsync(
        CouponPaymentTicket ticket,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using Bitmap ticketBitmap = await CreateTicketBitmapAsync(
            ticket,
            cancellationToken);

        byte[] rasterData = ConvertBitmapToEscPosRaster(ticketBitmap);

        using var stream = new MemoryStream();

        // Inicializar impresora ESC/POS.
        stream.Write(new byte[] { 0x1B, 0x40 });

        // Centrar la imagen completa.
        stream.Write(new byte[] { 0x1B, 0x61, 0x01 });

        stream.Write(rasterData, 0, rasterData.Length);

        // Alimentar papel para que el contenido rebase la cuchilla.
        stream.Write(new byte[] { 0x1B, 0x64, FeedLinesBeforeCut });

        // Corte total.
        stream.Write(new byte[] { 0x1D, 0x56, 0x00 });

        return stream.ToArray();
    }

    private static async Task<Bitmap> CreateTicketBitmapAsync(
        CouponPaymentTicket ticket,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        int couponCount = ticket.Coupons?.Count ?? 0;
        int couponRowsHeight = Math.Max(1, couponCount) * 42;

        const int headerHeight = 250;
        const int detailsHeight = 184;
        int couponsHeight = 86 + couponRowsHeight;
        const int totalHeight = 100;
        const int statusHeight = 94;
        const int qrHeight = 410;
        const int footerHeight = 190;

        int ticketHeight =
            headerHeight +
            detailsHeight +
            couponsHeight +
            totalHeight +
            statusHeight +
            qrHeight +
            footerHeight;

        var bitmap = new Bitmap(
            TicketWidthPx,
            ticketHeight,
            PixelFormat.Format32bppArgb);

        using Graphics graphics = Graphics.FromImage(bitmap);

        graphics.Clear(Color.White);
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        graphics.TextRenderingHint =
            System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

        using var blackBrush = new SolidBrush(Color.Black);
        using var whiteBrush = new SolidBrush(Color.White);
        using var grayBrush = new SolidBrush(Color.FromArgb(65, 65, 65));
        using var thinPen = new Pen(Color.Black, 1);
        using var mediumPen = new Pen(Color.Black, 2);
        using var dottedPen = new Pen(Color.Black, 1)
        {
            DashStyle = DashStyle.Dash
        };

        using var brandFont = new Font(
            "Arial",
            37,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        using var titleFont = new Font(
            "Arial",
            27,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        using var labelFont = new Font(
            "Consolas",
            19,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        using var valueFont = new Font(
            "Consolas",
            21,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        using var smallFont = new Font(
            "Arial",
            16,
            FontStyle.Regular,
            GraphicsUnit.Pixel);

        using var smallBoldFont = new Font(
            "Arial",
            18,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        using var couponFont = new Font(
            "Consolas",
            21,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        using var couponBoldFont = new Font(
            "Consolas",
            21,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        using var totalLabelFont = new Font(
            "Arial",
            26,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        using var totalValueFont = new Font(
            "Arial",
            39,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        using var statusFont = new Font(
            "Arial",
            18,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        using var qrTitleFont = new Font(
            "Arial",
            18,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        using var sideAmountFont = new Font(
            "Arial",
            126,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        using var reprintFont = new Font(
            "Arial",
            22,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        using var footerTitleFont = new Font(
            "Arial",
            28,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        using var footerTextFont = new Font(
            "Arial",
            20,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        int fullX = OuterMargin;
        int fullWidth = TicketWidthPx - (OuterMargin * 2);

        int rightX = OuterMargin + LeftPanelWidth + ColumnGap;
        int rightWidth = TicketWidthPx - rightX - OuterMargin;
        int dividerX = OuterMargin + LeftPanelWidth + (ColumnGap / 2);

        int y = 14;

        // =========================================================
        // ENCABEZADO A TODO EL ANCHO
        // =========================================================

        using Bitmap? logo = await LoadLogoAsync(cancellationToken);

        if (logo is not null)
        {
            const int maxLogoWidth = 120;
            const int maxLogoHeight = 92;

            Rectangle logoRect = FitRectangle(
                logo.Size,
                new Rectangle(
                    fullX,
                    y,
                    fullWidth,
                    maxLogoHeight),
                maxLogoWidth,
                maxLogoHeight);

            graphics.DrawImage(logo, logoRect);
            y += maxLogoHeight + 2;
        }

        DrawCenteredText(
            graphics,
            "PRACTICONTROL",
            brandFont,
            blackBrush,
            new RectangleF(fullX, y, fullWidth, 46));

        y += 53;

        Rectangle titleBar = new Rectangle(
            fullX,
            y,
            fullWidth,
            45);

        graphics.FillRectangle(blackBrush, titleBar);

        DrawCenteredText(
            graphics,
            "COMPROBANTE DE PAGO",
            titleFont,
            whiteBrush,
            titleBar);

        y += 61;

        DrawDashedHorizontalLine(
            graphics,
            dottedPen,
            fullX,
            fullX + fullWidth,
            y);

        int detailsTop = y + 18;

        // =========================================================
        // PRECIO GRANDE LATERAL
        // =========================================================

        string sideAmount = $"${ticket.TotalAmount:N2}";

        int splitSectionHeight =
            detailsHeight +
            couponsHeight +
            totalHeight +
            statusHeight +
            qrHeight - 18;

        DrawVerticalAmountTopToBottom(
            graphics,
            sideAmount,
            sideAmountFont,
            blackBrush,
            new RectangleF(
                OuterMargin,
                detailsTop,
                LeftPanelWidth,
                splitSectionHeight));

        // =========================================================
        // INDICADOR DE REIMPRESIÓN
        // Misma orientación vertical que el precio
        // =========================================================
        if (string.Equals(
                ticket.title,
                "Reimpresion",
                StringComparison.OrdinalIgnoreCase))
        {
            DrawVerticalAmountTopToBottom(
                graphics,
                "REIMPRESIÓN",
                reprintFont,
                blackBrush,
                new RectangleF(
                    OuterMargin - 50,
                    detailsTop + 300,
                    LeftPanelWidth,
                    300));
        }

        int contentBottom = detailsTop + splitSectionHeight;

        graphics.DrawLine(
            dottedPen,
            dividerX,
            detailsTop - 2,
            dividerX,
            contentBottom);

        // =========================================================
        // INFORMACIÓN DEL PAGO
        // =========================================================

        int infoY = detailsTop;
        const int labelWidth = 115;
        const int lineHeight = 27;

        DrawLabelValue(
            graphics,
            "Folio:",
            ticket.PaymentId.ToString(),
            labelFont,
            valueFont,
            blackBrush,
            rightX,
            infoY,
            labelWidth,
            rightWidth);

        infoY += lineHeight;

        DrawLabelValue(
            graphics,
            "Fecha:",
            ticket.PaymentDate.ToString("dd/MM/yyyy HH:mm:ss"),
            labelFont,
            valueFont,
            blackBrush,
            rightX,
            infoY,
            labelWidth,
            rightWidth);

        infoY += lineHeight;

        DrawLabelValue(
            graphics,
            "Lo atendió:",
            NullFallback(ticket.CashierName, "-"),
            labelFont,
            valueFont,
            blackBrush,
            rightX,
            infoY,
            labelWidth,
            rightWidth);

        infoY += lineHeight;

        DrawLabelValue(
            graphics,
            "Operador:",
            NullFallback(ticket.OperatorNumber, "-"),
            labelFont,
            valueFont,
            blackBrush,
            rightX,
            infoY,
            labelWidth,
            rightWidth);

        infoY += lineHeight + 5;

        DrawWrappedText(
            graphics,
            NullFallback(ticket.OperatorName, "SIN NOMBRE"),
            valueFont,
            blackBrush,
            new RectangleF(
                rightX,
                infoY,
                rightWidth,
                58),
            StringAlignment.Near);

        int couponTop = detailsTop + detailsHeight;

        DrawDashedHorizontalLine(
            graphics,
            dottedPen,
            rightX,
            rightX + rightWidth,
            couponTop);

        // =========================================================
        // CUPONES
        // =========================================================

        int couponY = couponTop + 15;

        graphics.DrawString(
            "CUPÓN",
            smallBoldFont,
            blackBrush,
            rightX,
            couponY);

        DrawRightAlignedText(
            graphics,
            "IMPORTE",
            smallBoldFont,
            blackBrush,
            rightX + rightWidth,
            couponY);

        couponY += 27;

        graphics.DrawLine(
            thinPen,
            rightX,
            couponY,
            rightX + rightWidth,
            couponY);

        couponY += 11;

        if (ticket.Coupons is not null && ticket.Coupons.Count > 0)
        {
            foreach (var coupon in ticket.Coupons)
            {
                cancellationToken.ThrowIfCancellationRequested();

                string description =
                    string.IsNullOrWhiteSpace(coupon.Code)
                        ? $"Cupón #{coupon.CouponId}"
                        : coupon.Code.Trim();

                graphics.DrawString(
                    TruncateToFit(
                        graphics,
                        description,
                        couponFont,
                        rightWidth - 125),
                    couponFont,
                    blackBrush,
                    rightX,
                    couponY);

                DrawRightAlignedText(
                    graphics,
                    $"${coupon.Amount:N2}",
                    couponBoldFont,
                    blackBrush,
                    rightX + rightWidth,
                    couponY);

                couponY += 42;
            }
        }
        else
        {
            graphics.DrawString(
                "Sin cupones",
                couponFont,
                grayBrush,
                rightX,
                couponY);
        }

        int totalTop = detailsTop + detailsHeight + couponsHeight;

        DrawDashedHorizontalLine(
            graphics,
            dottedPen,
            rightX,
            rightX + rightWidth,
            totalTop);

        // =========================================================
        // TOTAL
        // =========================================================

        graphics.DrawString(
            "TOTAL",
            totalLabelFont,
            blackBrush,
            rightX,
            totalTop + 28);

        DrawRightAlignedText(
            graphics,
            $"${ticket.TotalAmount:N2}",
            totalValueFont,
            blackBrush,
            rightX + rightWidth,
            totalTop + 16);

        int statusTop = totalTop + totalHeight;

        DrawDashedHorizontalLine(
            graphics,
            dottedPen,
            rightX,
            rightX + rightWidth,
            statusTop);

        // =========================================================
        // ESTADO DEL PAGO
        // =========================================================

        using var checkFont = new Font(
            "Arial",
            37,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        DrawCenteredText(
            graphics,
            "✓",
            checkFont,
            blackBrush,
            new RectangleF(
                rightX,
                statusTop + 7,
                rightWidth,
                44));

        DrawCenteredText(
            graphics,
            "PAGO REALIZADO CORRECTAMENTE",
            statusFont,
            blackBrush,
            new RectangleF(
                rightX,
                statusTop + 54,
                rightWidth,
                34));

        int qrTop = statusTop + statusHeight;

        DrawDashedHorizontalLine(
            graphics,
            dottedPen,
            rightX,
            rightX + rightWidth,
            qrTop);

        // =========================================================
        // QR DEL FOLIO DE PAGO
        // =========================================================

        DrawCenteredText(
            graphics,
            "ESCANEA EL QR",
            qrTitleFont,
            blackBrush,
            new RectangleF(
                rightX,
                qrTop + 15,
                rightWidth,
                30));

        DrawCenteredText(
            graphics,
            "PARA CONSULTAR EL FOLIO",
            smallBoldFont,
            blackBrush,
            new RectangleF(
                rightX,
                qrTop + 47,
                rightWidth,
                26));

        using Bitmap qrBitmap = GenerateQrBitmap(
            ticket.PaymentId.ToString(),
            size: 250);

        int qrX = rightX + ((rightWidth - qrBitmap.Width) / 2);
        int qrY = qrTop + 90;

        graphics.DrawImage(
            qrBitmap,
            qrX,
            qrY,
            qrBitmap.Width,
            qrBitmap.Height);

        DrawCenteredText(
            graphics,
            "FOLIO DE PAGO",
            smallBoldFont,
            blackBrush,
            new RectangleF(
                rightX,
                qrY + qrBitmap.Height + 5,
                rightWidth,
                24));

        DrawCenteredText(
            graphics,
            ticket.PaymentId.ToString(),
            valueFont,
            blackBrush,
            new RectangleF(
                rightX,
                qrY + qrBitmap.Height + 30,
                rightWidth,
                28));

        // =========================================================
        // PIE A TODO EL ANCHO
        // =========================================================

        int qrContentBottom =
            qrY +
            qrBitmap.Height +
            68;

        int footerTop = Math.Max(
            detailsTop + splitSectionHeight + 18,
            qrContentBottom + 24);

        DrawDashedHorizontalLine(
            graphics,
            dottedPen,
            fullX,
            fullX + fullWidth,
            footerTop);

        DrawWrappedText(
            graphics,
            "Este comprobante acredita el pago de los cupones indicados. " +
            "Conserve este ticket para cualquier aclaración.",
            footerTextFont,
            blackBrush,
            new RectangleF(
                fullX + 8,
                footerTop + 12,
                fullWidth - 16,
                84),
            StringAlignment.Center);

        DrawDashedHorizontalLine(
            graphics,
            dottedPen,
            fullX,
            fullX + fullWidth,
            footerTop + 108);

        DrawCenteredText(
            graphics,
            "GRACIAS",
            footerTitleFont,
            blackBrush,
            new RectangleF(
                fullX,
                footerTop + 120,
                fullWidth,
                43));

        return bitmap;
    }

    private static async Task<Bitmap?> LoadLogoAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            await using Stream imageStream =
                await FileSystem.OpenAppPackageFileAsync("logotc.png");

            cancellationToken.ThrowIfCancellationRequested();

            using var original = new Bitmap(imageStream);

            return PrepareLogoBitmap(
                original,
                maxWidth: 115,
                maxHeight: 92);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"No fue posible cargar el logo: {ex.Message}");

            return null;
        }
    }

    private static Bitmap PrepareLogoBitmap(
        Bitmap source,
        int maxWidth,
        int maxHeight)
    {
        double widthScale = (double)maxWidth / source.Width;
        double heightScale = (double)maxHeight / source.Height;
        double scale = Math.Min(1d, Math.Min(widthScale, heightScale));

        int targetWidth = Math.Max(
            1,
            (int)Math.Round(source.Width * scale));

        int targetHeight = Math.Max(
            1,
            (int)Math.Round(source.Height * scale));

        var result = new Bitmap(
            targetWidth,
            targetHeight,
            PixelFormat.Format32bppArgb);

        using Graphics graphics = Graphics.FromImage(result);

        graphics.Clear(Color.White);
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        graphics.DrawImage(
            source,
            new Rectangle(0, 0, targetWidth, targetHeight));

        return result;
    }

    private static Bitmap GenerateQrBitmap(
        string content,
        int size)
    {
        var writer = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Width = size,
                Height = size,
                Margin = 1,
                CharacterSet = "UTF-8",
                ErrorCorrection =
                    ZXing.QrCode.Internal.ErrorCorrectionLevel.M
            }
        };

        var pixelData = writer.Write(content);

        var bitmap = new Bitmap(
            pixelData.Width,
            pixelData.Height,
            PixelFormat.Format32bppArgb);

        BitmapData bitmapData = bitmap.LockBits(
            new Rectangle(
                0,
                0,
                pixelData.Width,
                pixelData.Height),
            ImageLockMode.WriteOnly,
            PixelFormat.Format32bppArgb);

        try
        {
            Marshal.Copy(
                pixelData.Pixels,
                0,
                bitmapData.Scan0,
                pixelData.Pixels.Length);
        }
        finally
        {
            bitmap.UnlockBits(bitmapData);
        }

        return bitmap;
    }

    private static void DrawVerticalAmountTopToBottom(
    Graphics graphics,
    string text,
    Font font,
    Brush brush,
    RectangleF bounds)
    {
        GraphicsState state = graphics.Save();

        try
        {
            /*
             * Colocamos el origen en la esquina superior derecha
             * de la columna izquierda.
             */
            graphics.TranslateTransform(
                bounds.Right,
                bounds.Top);

            /*
             * Giro de 90 grados en sentido horario.
             *
             * Esto hace que el texto se lea de arriba hacia abajo:
             *
             * $
             * 1
             * 2
             * 3
             * .
             * 0
             * 0
             */
            graphics.RotateTransform(90f);

            using var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.None,
                FormatFlags = StringFormatFlags.NoWrap
            };

            graphics.DrawString(
                text,
                font,
                brush,
                new RectangleF(
                    0,
                    0,
                    bounds.Height,
                    bounds.Width),
                format);
        }
        finally
        {
            graphics.Restore(state);
        }
    }

    private static Bitmap CreateRotatedTextBitmap(
        string text,
        Font font,
        Color color,
        int maxWidth,
        int maxHeight)
    {
        using var measureBitmap = new Bitmap(1, 1);
        using Graphics measureGraphics = Graphics.FromImage(measureBitmap);

        SizeF measured = measureGraphics.MeasureString(text, font);

        int sourceWidth = Math.Max(1, (int)Math.Ceiling(measured.Width) + 16);
        int sourceHeight = Math.Max(1, (int)Math.Ceiling(measured.Height) + 16);

        using var source = new Bitmap(
            sourceWidth,
            sourceHeight,
            PixelFormat.Format32bppArgb);

        using (Graphics graphics = Graphics.FromImage(source))
        {
            graphics.Clear(Color.White);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint =
                System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            using var brush = new SolidBrush(color);

            graphics.DrawString(
                text,
                font,
                brush,
                new PointF(8, 8));
        }

        source.RotateFlip(RotateFlipType.Rotate90FlipXY);

        double widthScale = (double)maxWidth / source.Width;
        double heightScale = (double)maxHeight / source.Height;
        double scale = Math.Min(1d, Math.Min(widthScale, heightScale));

        int targetWidth = Math.Max(
            1,
            (int)Math.Round(source.Width * scale));

        int targetHeight = Math.Max(
            1,
            (int)Math.Round(source.Height * scale));

        var result = new Bitmap(
            targetWidth,
            targetHeight,
            PixelFormat.Format32bppArgb);

        using Graphics resultGraphics = Graphics.FromImage(result);

        resultGraphics.Clear(Color.White);
        resultGraphics.InterpolationMode =
            InterpolationMode.HighQualityBicubic;

        resultGraphics.DrawImage(
            source,
            new Rectangle(0, 0, targetWidth, targetHeight));

        return result;
    }

    private static byte[] ConvertBitmapToEscPosRaster(
        Bitmap bitmap)
    {
        int width = bitmap.Width;
        int height = bitmap.Height;
        int bytesPerRow = (width + 7) / 8;

        using var stream = new MemoryStream();

        // GS v 0 m xL xH yL yH
        stream.WriteByte(0x1D);
        stream.WriteByte(0x76);
        stream.WriteByte(0x30);
        stream.WriteByte(0x00);

        stream.WriteByte((byte)(bytesPerRow & 0xFF));
        stream.WriteByte((byte)((bytesPerRow >> 8) & 0xFF));
        stream.WriteByte((byte)(height & 0xFF));
        stream.WriteByte((byte)((height >> 8) & 0xFF));

        const int blackThreshold = 178;

        for (int y = 0; y < height; y++)
        {
            for (int byteIndex = 0;
                 byteIndex < bytesPerRow;
                 byteIndex++)
            {
                byte currentByte = 0;

                for (int bit = 0; bit < 8; bit++)
                {
                    int x = (byteIndex * 8) + bit;

                    if (x >= width)
                    {
                        continue;
                    }

                    Color pixel = bitmap.GetPixel(x, y);

                    int red =
                        ((pixel.R * pixel.A) +
                         (255 * (255 - pixel.A))) / 255;

                    int green =
                        ((pixel.G * pixel.A) +
                         (255 * (255 - pixel.A))) / 255;

                    int blue =
                        ((pixel.B * pixel.A) +
                         (255 * (255 - pixel.A))) / 255;

                    int luminance =
                        (red * 299 +
                         green * 587 +
                         blue * 114) / 1000;

                    if (luminance < blackThreshold)
                    {
                        currentByte |= (byte)(0x80 >> bit);
                    }
                }

                stream.WriteByte(currentByte);
            }
        }

        return stream.ToArray();
    }

    private static Rectangle FitRectangle(
        Size imageSize,
        Rectangle available,
        int maxWidth,
        int maxHeight)
    {
        double scale = Math.Min(
            (double)Math.Min(available.Width, maxWidth) /
            imageSize.Width,
            (double)Math.Min(available.Height, maxHeight) /
            imageSize.Height);

        int width = Math.Max(
            1,
            (int)Math.Round(imageSize.Width * scale));

        int height = Math.Max(
            1,
            (int)Math.Round(imageSize.Height * scale));

        int x = available.X + ((available.Width - width) / 2);
        int y = available.Y + ((available.Height - height) / 2);

        return new Rectangle(x, y, width, height);
    }

    private static void DrawLabelValue(
        Graphics graphics,
        string label,
        string value,
        Font labelFont,
        Font valueFont,
        Brush brush,
        int x,
        int y,
        int labelWidth,
        int totalWidth)
    {
        graphics.DrawString(
            label,
            labelFont,
            brush,
            x,
            y);

        string fittedValue = TruncateToFit(
            graphics,
            value,
            valueFont,
            totalWidth - labelWidth);

        graphics.DrawString(
            fittedValue,
            valueFont,
            brush,
            x + labelWidth,
            y);
    }

    private static void DrawCenteredText(
        Graphics graphics,
        string text,
        Font font,
        Brush brush,
        RectangleF rectangle)
    {
        using var format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter
        };

        graphics.DrawString(
            text,
            font,
            brush,
            rectangle,
            format);
    }

    private static void DrawRightAlignedText(
        Graphics graphics,
        string text,
        Font font,
        Brush brush,
        float right,
        float y)
    {
        SizeF size = graphics.MeasureString(text, font);

        graphics.DrawString(
            text,
            font,
            brush,
            right - size.Width,
            y);
    }

    private static void DrawWrappedText(
        Graphics graphics,
        string text,
        Font font,
        Brush brush,
        RectangleF rectangle,
        StringAlignment alignment)
    {
        using var format = new StringFormat
        {
            Alignment = alignment,
            LineAlignment = StringAlignment.Near,
            Trimming = StringTrimming.EllipsisWord
        };

        graphics.DrawString(
            text,
            font,
            brush,
            rectangle,
            format);
    }

    private static void DrawDashedHorizontalLine(
        Graphics graphics,
        Pen pen,
        int startX,
        int endX,
        int y)
    {
        graphics.DrawLine(
            pen,
            startX,
            y,
            endX,
            y);
    }

    private static string TruncateToFit(
        Graphics graphics,
        string text,
        Font font,
        float maxWidth)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        if (graphics.MeasureString(text, font).Width <= maxWidth)
        {
            return text;
        }

        const string ellipsis = "...";
        string candidate = text;

        while (candidate.Length > 0)
        {
            candidate = candidate[..^1];

            if (graphics.MeasureString(
                    candidate + ellipsis,
                    font).Width <= maxWidth)
            {
                return candidate + ellipsis;
            }
        }

        return ellipsis;
    }

    private static string NullFallback(
        string? value,
        string fallback)
    {
        return string.IsNullOrWhiteSpace(value)
            ? fallback
            : value.Trim();
    }

    private static string GetDefaultPrinterName()
    {
        uint requiredSize = 0;

        GetDefaultPrinter(null, ref requiredSize);

        int error = Marshal.GetLastWin32Error();

        const int ErrorFileNotFound = 2;

        if (requiredSize == 0)
        {
            if (error == ErrorFileNotFound)
            {
                throw new InvalidOperationException(
                    "Windows no tiene una impresora predeterminada.");
            }

            throw new Win32Exception(
                error,
                "No fue posible obtener la impresora predeterminada.");
        }

        var printerName = new StringBuilder((int)requiredSize);

        if (!GetDefaultPrinter(printerName, ref requiredSize))
        {
            throw new Win32Exception(
                Marshal.GetLastWin32Error(),
                "No fue posible obtener la impresora predeterminada.");
        }

        string result = printerName.ToString().Trim();

        if (string.IsNullOrWhiteSpace(result))
        {
            throw new InvalidOperationException(
                "El nombre de la impresora predeterminada está vacío.");
        }

        return result;
    }

    private static void SendBytesToPrinter(
        string printerName,
        byte[] data,
        string documentName)
    {
        if (!OpenPrinter(
                printerName,
                out IntPtr printerHandle,
                IntPtr.Zero))
        {
            throw new Win32Exception(
                Marshal.GetLastWin32Error(),
                $"No fue posible abrir la impresora '{printerName}'.");
        }

        try
        {
            var documentInfo = new DOC_INFO_1
            {
                pDocName = documentName,
                pDataType = "RAW",
                pOutputFile = null
            };

            int jobId = StartDocPrinter(
                printerHandle,
                1,
                ref documentInfo);

            if (jobId == 0)
            {
                throw new Win32Exception(
                    Marshal.GetLastWin32Error(),
                    "No se pudo iniciar el trabajo de impresión.");
            }

            try
            {
                if (!StartPagePrinter(printerHandle))
                {
                    throw new Win32Exception(
                        Marshal.GetLastWin32Error(),
                        "No se pudo iniciar la página.");
                }

                try
                {
                    if (!WritePrinter(
                            printerHandle,
                            data,
                            data.Length,
                            out int bytesWritten))
                    {
                        throw new Win32Exception(
                            Marshal.GetLastWin32Error(),
                            "No se pudo enviar el ticket.");
                    }

                    if (bytesWritten != data.Length)
                    {
                        throw new InvalidOperationException(
                            $"Se enviaron {bytesWritten} de " +
                            $"{data.Length} bytes.");
                    }
                }
                finally
                {
                    EndPagePrinter(printerHandle);
                }
            }
            finally
            {
                EndDocPrinter(printerHandle);
            }
        }
        finally
        {
            ClosePrinter(printerHandle);
        }
    }

    [StructLayout(
        LayoutKind.Sequential,
        CharSet = CharSet.Unicode)]
    private struct DOC_INFO_1
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pDocName;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string? pOutputFile;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string pDataType;
    }

    [DllImport(
        "winspool.drv",
        EntryPoint = "GetDefaultPrinterW",
        SetLastError = true,
        CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetDefaultPrinter(
        StringBuilder? pszBuffer,
        ref uint pcchBuffer);

    [DllImport(
        "winspool.drv",
        SetLastError = true,
        CharSet = CharSet.Unicode)]
    private static extern bool OpenPrinter(
        string pPrinterName,
        out IntPtr phPrinter,
        IntPtr pDefault);

    [DllImport(
        "winspool.drv",
        SetLastError = true,
        CharSet = CharSet.Unicode)]
    private static extern int StartDocPrinter(
        IntPtr hPrinter,
        int level,
        ref DOC_INFO_1 pDocInfo);

    [DllImport(
        "winspool.drv",
        SetLastError = true)]
    private static extern bool StartPagePrinter(
        IntPtr hPrinter);

    [DllImport(
        "winspool.drv",
        SetLastError = true)]
    private static extern bool WritePrinter(
        IntPtr hPrinter,
        byte[] pBytes,
        int dwCount,
        out int dwWritten);

    [DllImport(
        "winspool.drv",
        SetLastError = true)]
    private static extern bool EndPagePrinter(
        IntPtr hPrinter);

    [DllImport(
        "winspool.drv",
        SetLastError = true)]
    private static extern bool EndDocPrinter(
        IntPtr hPrinter);

    [DllImport(
        "winspool.drv",
        SetLastError = true)]
    private static extern bool ClosePrinter(
        IntPtr hPrinter);
}

#endif
