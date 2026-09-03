using PdfSharp.Pdf;
using PdfSharp.Drawing;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        // 1. Crear documento MigraDoc
        Document doc = new Document();
        Section section = doc.AddSection();

        // Configura márgenes personalizados (opcional)
        section.PageSetup.LeftMargin = Unit.FromCentimeter(2);
        section.PageSetup.TopMargin = Unit.FromCentimeter(3);

        // Añade contenido MigraDoc
        section.AddParagraph("Texto normal de MigraDoc.");

        // 2. Renderiza el documento
        PdfDocumentRenderer renderer = new PdfDocumentRenderer(true)
        {
            Document = doc
        };
        renderer.RenderDocument();

        PdfDocument pdf = renderer.PdfDocument;

        // 3. Obtener márgenes para usar como coordenadas iniciales
        double leftMargin = section.PageSetup.LeftMargin.Point; // en puntos
        double topMargin = section.PageSetup.TopMargin.Point;

        // 4. Dibujar texto rotado con PDFsharp/XGraphics
        XGraphics gfx = XGraphics.FromPdfPage(pdf.Pages[0]);
        gfx.TranslateTransform(leftMargin + 100, topMargin + 50); // desplazamiento desde los márgenes
        gfx.RotateTransform(-45);

        XFont font = new XFont("Arial", 16, XFontStyle.Bold);
        gfx.DrawString("Texto rotado relativo a la sección", font, XBrushes.DarkGreen, new XPoint(0, 0));

        // 5. Guardar y abrir PDF
        string filename = "SeccionConTextoRotado.pdf";
        pdf.Save(filename);
        Process.Start(filename);
    }
}