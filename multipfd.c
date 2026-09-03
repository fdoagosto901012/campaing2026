using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Lista de rutas de archivos PDF a unir
        string[] archivosPDF = new string[]
        {
            "documento1.pdf",
            "documento2.pdf",
            "documento3.pdf"
        };

        // Crear nuevo documento PDF de salida
        PdfDocument pdfFinal = new PdfDocument();

        foreach (string archivo in archivosPDF)
        {
            if (File.Exists(archivo))
            {
                // Abrir el documento existente
                PdfDocument inputPDF = PdfReader.Open(archivo, PdfDocumentOpenMode.Import);

                // Copiar cada página al documento final
                for (int i = 0; i < inputPDF.PageCount; i++)
                {
                    PdfPage page = inputPDF.Pages[i];
                    pdfFinal.AddPage(page);
                }
            }
            else
            {
                Console.WriteLine($"Archivo no encontrado: {archivo}");
            }
        }

        // Guardar el PDF combinado
        string nombreFinal = "PDF_Unificado.pdf";
        pdfFinal.Save(nombreFinal);
        Console.WriteLine($"PDF combinado guardado como: {nombreFinal}");
    }
}