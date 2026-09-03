using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PagedList;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using radiotaxi.Client.Interface;
using radiotaxi.Client.Request;
using radiotaxi.Model;
using radiotaxi.WEB.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace radiotaxi.WEB.Controllers
{
    [SessionFilter]
    public class taxiReportController : Controller
    {
        static readonly iTaxiReport taxiReportRequest = new rTaxiReport();
        static readonly iClient clientRequest = new rClient();

        public ActionResult GenerateReport(int id)
        {
            try
            {
                int statusConnection = 0;
                report report = taxiReportRequest.GetById(id, Session["access_token"].ToString(), ref statusConnection);
                // Create new PDF document
                Document document = new Document();
                document.Info.Title = "Reporte de Radio Taxi";
                document.Info.Author = "Radio Taxi System";

                // Create new page
                //PdfPage page = document.AddPage();
                //ENCABEZADO
                MigraDoc.DocumentObjectModel.Section section = document.AddSection();

                Image objImgLogo = section.AddParagraph().AddImage(Server.MapPath("~/img/logo.png"));
                objImgLogo.Height = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(10);
                objImgLogo.Width = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(10);

                Paragraph paragraph1 = section.AddParagraph();
                paragraph1.Format.LineSpacingRule = MigraDoc.DocumentObjectModel.LineSpacingRule.Exactly;
                paragraph1.Format.LineSpacing = 11;
                paragraph1.Format.Alignment = ParagraphAlignment.Center;
                paragraph1.Format.Font.Size = 12;
                paragraph1.AddFormattedText("Radio Taxi", TextFormat.Bold);
                paragraph1 = section.AddParagraph();




                Paragraph paragraph2 = section.AddParagraph();
                paragraph2.AddText("");
                paragraph2.Format.SpaceAfter = "10mm";

                //TABLA DATOS
                Table tableCar = new Table();
                Column columnCar = tableCar.AddColumn(Unit.FromCentimeter(5));
                columnCar.Format.Alignment = ParagraphAlignment.Left;
                tableCar.AddColumn(Unit.FromCentimeter(12));
                Row rowCar = tableCar.AddRow();
                Cell cellCar = rowCar.Cells[0];
                cellCar.AddParagraph("Nombre del reportado:");
                cellCar.AddParagraph("");
                cellCar = rowCar.Cells[1];
                cellCar.Format.Font.Bold = true;
                cellCar.AddParagraph(report.name);
                cellCar.AddParagraph("");
                document.LastSection.Add(tableCar);
                rowCar = tableCar.AddRow();
                cellCar = rowCar.Cells[0];
                cellCar.AddParagraph("Teléfono:");
                cellCar.AddParagraph("");
                cellCar = rowCar.Cells[1];
                cellCar.Format.Font.Bold = true;
                cellCar.AddParagraph(report.phone);
                cellCar.AddParagraph("");
                rowCar = tableCar.AddRow();
                cellCar = rowCar.Cells[0];
                cellCar.AddParagraph("Dirección:");
                cellCar.AddParagraph("");
                cellCar = rowCar.Cells[1];
                cellCar.Format.Font.Bold = true;
                cellCar.AddParagraph(report.address);
                cellCar.AddParagraph("");
                rowCar = tableCar.AddRow();
                cellCar = rowCar.Cells[0];
                cellCar.AddParagraph("Descripción:");
                cellCar.AddParagraph("");
                cellCar = rowCar.Cells[1];
                cellCar.Format.Font.Bold = true;
                cellCar.Format.Alignment = ParagraphAlignment.Justify;
                cellCar.AddParagraph(report.description);
                cellCar.AddParagraph("");
                rowCar = tableCar.AddRow();
                cellCar = rowCar.Cells[0];
                cellCar.AddParagraph("Fecha de creación:");
                cellCar.AddParagraph("");
                cellCar = rowCar.Cells[1];
                cellCar.Format.Font.Bold = true;
                cellCar.AddParagraph(report.dateCreated.Value.ToString("dd/MM/yyyy"));
                cellCar.AddParagraph("");
                rowCar = tableCar.AddRow();
                cellCar = rowCar.Cells[0];
                cellCar.AddParagraph("Taxis Involucrados:");
                cellCar.AddParagraph("");
                cellCar = rowCar.Cells[1];
                cellCar.Format.Font.Bold = true;
                //cellCar.AddParagraph(report.taxiReports == null ? "" : string.Join(",", report.taxiReports.Select(x => x.taxiNumber.Value)));                
                //Paragraph paragraph6 = section.AddParagraph();
                //paragraph6.Format.SpaceBefore = "10mm";

                //paragraph6.AddText("ALATA POR PAGO DE DERECHO Y T/C " + DateTime.Now.Year + " (ALTA)");

                //paragraph6 = section.AddParagraph();
                //paragraph6.AddText("");
                //paragraph6.Format.Alignment = ParagraphAlignment.Center;
                //paragraph6.AddText("ATENTAMENTE");
                //paragraph6.Format.SpaceAfter = "20mm";
                //paragraph6.Format.SpaceBefore = "5mm";    
                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();

                // Associate the MigraDoc document with a renderer
                pdfRenderer.Document = document;

                // Layout and render document to PDF
                pdfRenderer.RenderDocument();


                // Send PDF to browser
                MemoryStream stream = new MemoryStream();
                pdfRenderer.Save(stream, false);
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", stream.Length.ToString());
                Response.BinaryWrite(stream.ToArray());
                Response.Flush();
                stream.Close();
                stream.Position = 0;
                Response.End();
                return File(stream, "application/pdf", report.id + ".pdf");
            }
            catch
            {
                return null;
            }

            
        }

        // GET: taxiReport
        [HttpGet]
        public ActionResult Index(string phone, string createdBy, string dateCreated, int? page, int? size)
        {
            if (Session["role"].ToString().Equals("AdminRT"))
            {
                int statusConnection = 0;
                Paginator<report> reports;
                SearchModelReport search = new SearchModelReport();
                search.phone = phone;
                search.createdBy = createdBy;
                search.dateCreated = dateCreated;

                int pageSize = size ?? 20;
                int pageNumber = (page ?? 1);
                ViewBag.phoneValue = phone ?? "";
                ViewBag.createdByValue = createdBy ?? "";
                ViewBag.dateCreatedValue = dateCreated;
                ViewBag.sizeValue = size;

                if (!String.IsNullOrEmpty(phone) || !String.IsNullOrEmpty(createdBy) || !String.IsNullOrEmpty(dateCreated))
                {
                    reports = taxiReportRequest.search(Session["access_token"].ToString(), search, pageNumber, pageSize, ref statusConnection);
                }
                else
                {
                    reports = taxiReportRequest.search(Session["access_token"].ToString(), search, pageNumber, pageSize, ref statusConnection);
                }

                if (reports != null)
                {
                    IPagedList<report> pageOrders = new StaticPagedList<report>(reports.Data, pageNumber, pageSize, reports.itemCount);
                    return View(pageOrders);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return RedirectToAction("Index", "Search");
            }
        }

        // GET: taxiReport/Details/5
        public ActionResult Details(int id)
        {
            int statusConnection = 0;
            report report = taxiReportRequest.GetById(id, Session["access_token"].ToString(), ref statusConnection);
            return View(report);
        }

        // GET: taxiReport/Create
        public ActionResult Create(string phone)
        {

            ViewBag.message = null;
            ViewBag.storage = false;

            int statusConnectionClient = 0;
            try
            {
                if (!String.IsNullOrEmpty(phone))
            {
                client client = clientRequest.GetById(phone, Session["access_token"].ToString(), ref statusConnectionClient);
                return View(new reportDTO { phone = phone, name = client.name + client.lastname1, address = client.address });
            }
            else
            {
                return View();
            }
            }
            catch (Exception)
            {
                return View();
            }

            
        }

        // POST: taxiReport/Create
        [HttpPost]
        public ActionResult Create(report collection)
        {
            try
            {
                // TODO: Add insert logic here
                collection.createdBy = Session["name"].ToString();
                collection.dateCreated = DateTime.Now.ToLocalTime();
                int statusConnection = 0;
                taxiReportRequest.Add(ref collection, Session["access_token"].ToString(), ref statusConnection);
                if (statusConnection == 201)
                {
                    return RedirectToAction("Details", "taxiReport", new { @id = collection.id });
                }
                else
                {
                    return View(collection);
                }
            }
            catch
            {
                return View(collection);
            }
        }

        // GET: taxiReport/Edit/5
        public ActionResult Edit(int id)
        {
            int statusConnection = 0;
            report report = taxiReportRequest.GetById(id, Session["access_token"].ToString(), ref statusConnection);
            reportDTO reportDTO = new reportDTO { id = report.id, name = report.name, phone = report.phone, address = report.address, createdBy = report.createdBy, editedBy = report.editedBy, dateCreated = report.dateCreated, dateEdited = report.dateEdited, description = report.description };
            reportDTO.taxiReports = null; //report.taxiReports.ToList();
            return View(reportDTO);
        }

        // POST: taxiReport/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, report collection)
        {
            try
            {
                // TODO: Add update logic here
                collection.editedBy = Session["name"].ToString();
                collection.dateEdited = DateTime.Now.ToLocalTime();
                int statusConnection = 0;
                taxiReportRequest.Update(id, collection, Session["access_token"].ToString(), ref statusConnection);

                if (statusConnection == 204)
                {
                    return RedirectToAction("Index", "Search");
                }
                else
                {
                    return View(collection);
                }
            }
            catch
            {
                return View(collection);
            }
        }

        // GET: taxiReport/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: taxiReport/Delete/5
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
    }
}
