using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace radiotaxi.WEB.Helper
{
    public class PdfService
    {
        public byte[] GeneratePdf(string title, string content)
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Add content with PDFsharp
            XFont font = new XFont("Verdana", 20, XFontStyle.Bold);
            gfx.DrawString(title, font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.TopCenter);

            // Add more detailed content using MigraDoc
            Document doc = new Document();
            Section section = doc.AddSection();
            section.AddParagraph(content);

            // Render MigraDoc content
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();
            renderer.PdfDocument.Save("output.pdf");

            // Save PDF to memory stream
            using (var stream = new MemoryStream())
            {
                document.Save(stream, false);
                return stream.ToArray();
            }
        }
    }
}