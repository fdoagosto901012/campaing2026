using Amazon.SimpleDB.Model;
using Amazon.SimpleEmail.Model;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using OfficeOpenXml;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using radiotaxi.Model;
using radiotaxi.Model.DTO;
using radiotaxi.Model.v2.Coupons;
using radiotaxi.WEB.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
using System.Drawing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ZXing;
using ZXing.Common;
using static System.Collections.Specialized.BitVector32;
using Color = MigraDoc.DocumentObjectModel.Color;
using Image = MigraDoc.DocumentObjectModel.Shapes.Image;
using Orientation = MigraDoc.DocumentObjectModel.Orientation;
using Section = MigraDoc.DocumentObjectModel.Section;
using Unit = MigraDoc.DocumentObjectModel.Unit;

namespace radiotaxi.WEB.Controllers
{
    [SessionFilter("Admin")]
    [RoutePrefix("cupones")]
    public class CuponsController : webBaseController
    {
        /// <summary>
        ///  BLOQUE DE ARIFA
        ///| </summary>
        [HttpGet]
        [Route("tarifa")]
        public async Task<ActionResult> RateList()
        {
            try
            {
                List<Hotel> hotels = await db.Hotels
                    .Include(h => h.Couponrates.Select(r => r.Cupontype))
                    //.Include(h => h.Couponrates.Select(r => r.Cupons))
                    .OrderBy(h => h.Name)
                    .ToListAsync();
                return View(hotels);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        [HttpGet]
        [Route("tarifa/crear")]
        // GET: todos los cupones
        public ActionResult createrate()
        {
            Cupon _coupon = new Cupon();
            ViewBag.hotels = new Hotel().get();
            ViewBag.cuponstypes = new Cupontype().get();
            List<Cupon> _cu = _coupon.get();
            return View(_cu);
        }

        [HttpGet]
        [Route("pago/{id}")]
        // GET: todos los cupones
        public ActionResult pay(int id)
        {
            
            CouponPayment payment = this.db.CouponPayments
                .Include(x => x.Cupons)
                .Include(x => x.CouponPaymentDetails).FirstOrDefault(x => x.CouponPaymentId == id);
            
            return View(payment);
        }

        [HttpPost]
        [Route("tarifa/crear")]
        // GET: todos los cupones
        public ActionResult createrate(List<string> response)
        {
            Cupon _coupon = new Cupon();
            ViewBag.hotels = new Hotel().get();
            ViewBag.cuponstypes = new Cupontype().get();
            List<Cupon> _cu = _coupon.get();
            return View(_cu);
        }

        // POST: Cupons/Create
        [HttpPost]
        [Route("tarifa/crear/json")]
        public async Task<JsonResult> CreateRateAsync(CouponrateDTO Object)

        {
            try
            {

                Couponrate couponrate = new Couponrate();
                couponrate.Name = DateTime.Now.ToString();
                couponrate.CreateBy = this.User.firstName + " " + this.User.lastName;
                couponrate.EditedBy = this.User.firstName + " " + this.User.lastName;
                couponrate.EditedDate = DateTime.Now;
                couponrate.CreateDate = DateTime.Now;
                couponrate.HotelID = Object.HotelID;
                couponrate.CupontypeID = Object.CupontypeID;

                couponrate.Valor = Object.Valor;
                db.Couponrates.Add(couponrate);
                db.SaveChanges();
                Console.WriteLine("Render");

                // obtenemos el objeto completo para devolverlo en JSON 
                var rates = await db.Couponrates
                    .Include(r => r.Hotel)
                    .Include(r => r.Cupontype)
                    .Where(x => x.CouponrateID == couponrate.CouponrateID)
                    .OrderBy(r => r.Hotel.Name)
                    .ThenBy(r => r.Cupontype.Name)
                    .FirstOrDefaultAsync();


                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(rates, settings);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    Console.WriteLine(
                        "Entidad: {0}, Estado: {1}",
                        entityValidationErrors.Entry.Entity.GetType().Name,
                        entityValidationErrors.Entry.State);

                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.WriteLine(
                            "Propiedad: {0}, Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }

                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(null, settings);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }




        [HttpGet]
        [Route("tarifa/Edit/{id}")]
        // GET: todos los cupones
        public ActionResult reateEdit(int id)
        {
            Cupon _coupon = new Cupon();
            ViewBag.hotels = new Hotel().get();
            ViewBag.cuponstypes = new Cupontype().get();
            List<Cupon> _cu = _coupon.get();
            return View(_cu);
        }

        [HttpPost]
        [Route("tarifa/Edit/{id}")]
        // GET: todos los cupones
        public ActionResult reateEdit(int id, List<string> data)
        {
            Cupon _coupon = new Cupon();
            ViewBag.hotels = new Hotel().get();
            ViewBag.cuponstypes = new Cupontype().get();
            List<Cupon> _cu = _coupon.get();
            return View(_cu);
        }
        /// <returns></returns>



        [Route("")]
        // GET: todos los cupones
        public ActionResult Index()
        {
            Cupon _coupon = new Cupon();
            ViewBag.hotels = new Hotel().get();
            ViewBag.cuponstypes = new Cupontype().get();
            List<Cupon> _cu = _coupon.get();
            return View(_cu);
        }

        [HttpPost]
        [Route("")]
        // GET: todos los cupones
        public ActionResult Index(SerchCuponsDTO obj)
        {
            Cupon _coupon = new Cupon();
            ViewBag.hotels = new Hotel().get();
            ViewBag.cuponstypes = new Cupontype().get();
            List<Cupon> _cu = _coupon.search(obj);
            return View(_cu);
        }

        [Route("detalle/{id}")]
        // GET: Cupons/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [Route("crear/")]
        // GET: Cupons/Create
        public ActionResult Create()
        {
            Cupon _coupon = new Cupon();
            ViewBag.hotels = new Hotel().getWithTarif();
            return View();
        }

        // POST: Cupons/Create
        [HttpPost]
        [Route("crear/")]
        public JsonResult Create(List<CouponCreateDTO> objects)
        {
            try
            {
                foreach (var obj in objects)
                {
                    // Cupones..
                    Cupon _coupon = new Cupon();
                    _coupon.save(obj);
                }
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(objects, settings);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(null, settings);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }


        // GET: Cupons/Edit/5
        [Route("editar/{id}")]
        public ActionResult Edit(int id)
        {
            Cupon _coupon = new Cupon().get(id);
            ViewBag.hotels = new Hotel().getWithTarif();
            Hotel hotel = new Hotel().getWithTarif(_coupon.Couponrate.HotelID);
            ViewBag.hotel = hotel;
            ViewBag.couponrates = hotel.Couponrates;
            return View(_coupon);
        }

        // POST: Cupons/Edit/5
        [HttpPost]
        [Route("editar/{id}")]
        public ActionResult Edit(int id, CouponCreateDTO obj)
        {
            try
            {
                Cupon _coupon = new Cupon().get(id);

                Couponrate couponrate = new Couponrate().get(obj.tarifa);
                _coupon.CouponratesID = obj.tarifa;
                _coupon.Couponrate = couponrate;
                _coupon.active = obj.active;
                _coupon.update();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cupons/Edit/5
        [HttpGet]
        [Route("tarifas/{id}")]
        public JsonResult tarifas(int id)
        {
            try
            {
                Hotel hotel = new Hotel().getWithTarif(id);
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(hotel, settings);
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

        // GET: Cupons/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cupons/Delete/5
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

        //////////////////////////////////////////////////////
        [Route("impresion")]
        //GET
        public ActionResult PrintModule()
        {
            // Obtener los grupos....
            cuponsGroupPrinted cuponsGroupPrinted = new cuponsGroupPrinted();
            List<cuponsGroupPrintedDTO> cuponsGroupPrinteds = cuponsGroupPrinted.getPrinted();
            List<cuponsGroupPrintedDTO> NOcuponsGroupPrinteds = cuponsGroupPrinted.getNoPrinted();
            ViewBag.cuponsGroupPrinteds = cuponsGroupPrinteds;
            ViewBag.NOcuponsGroupPrinteds = NOcuponsGroupPrinteds;
            // Obtenemos los datos para el buscador de no impresos.
            ViewBag.hotels = new Hotel().get();
            ViewBag.cuponstypes = new Cupontype().get();
            return View(NOcuponsGroupPrinteds);
        }

        // POST: Operator/GetCities
        [HttpPost]
        [Route("all")]
        public JsonResult LoadCupons()
        {
            try
            {
                cuponsGroupPrinted cuponsGroupPrinted = new cuponsGroupPrinted();
                List<cuponsGroupPrintedDTO> cuponsGroupPrinteds = cuponsGroupPrinted.getPrinted();
                List<cuponsGroupPrintedDTO> NOcuponsGroupPrinteds = cuponsGroupPrinted.getNoPrinted();
                Dictionary<string, List<cuponsGroupPrintedDTO>> orders = cuponsGroupPrinted.ordes();

                List<List<cuponsGroupPrintedDTO>> NOcuponsGroupPrinteds_ = null; // Una orden de impresion puede contener listas de grupos.
                LoadCuponsDTO loadCuponsDTO = new LoadCuponsDTO();
                loadCuponsDTO.cuponsGroupPrinteds = cuponsGroupPrinteds;
                loadCuponsDTO.NOcuponsGroupPrinteds = NOcuponsGroupPrinteds;
                loadCuponsDTO.orders = orders;
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(loadCuponsDTO, settings);
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
        [HttpPost]
        [Route("printedaction")]
        public JsonResult registerPrint(List<cuponsGroupPrintedDTO> group)
        {
            try
            {
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        Formatting = Formatting.Indented,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                    };

                    if (group == null) // Cuando en nulo regresar el objeto que no hay nada selccion.
                    {
                        var responsenull = new
                        {
                            status = 404,
                            response = "",
                            message = "No se encontraron cupones para imprimir."
                        };
                        string jsonnofound = JsonConvert.SerializeObject(responsenull, settings);
                        return Json(jsonnofound, JsonRequestBehavior.AllowGet);
                    }

                    List<cuponsGroupPrinted> printed = new List<cuponsGroupPrinted>();
                    cuponsGroupPrinted g = new cuponsGroupPrinted();
                    string username = this.User.firstName + " " + this.User.lastName;
                    string ramdomFolio = this.User.userId.ToString() + this.GenerarCadenaAleatoria(10);
                    foreach (var item in group)
                    {
                        cuponsGroupPrinted finded = g.get(item.Id);
                        if (finded != null)
                        {
                            // Falta ver quien mando a imprimir.
                            finded.Printed = true;
                            finded.PrintedDate = DateTime.Now;
                            finded.GroupFolio = ramdomFolio;
                            finded.update();
                            printed.Add(finded);
                        }
                    }
                    var responseObj = new
                    {
                        status = 200,
                        response = printed,
                        message = ""
                    };
                    string json = JsonConvert.SerializeObject(responseObj, settings);
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    // Aqui hay un fallo
                    var responseObj = new
                    {
                        status = 500,
                        response = "",
                        message = ex.Message
                    };

                    var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        Formatting = Formatting.Indented,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                    };
                    string json = JsonConvert.SerializeObject(responseObj, settings);
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                
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
        [HttpPost]
        [Route("deliveraction")]
        public JsonResult registerDeliver(cuponsGroupPrintedDTO group)
        {
            try
            {
                cuponsGroupPrinted g = new cuponsGroupPrinted();
                string username = this.User.firstName + " " + this.User.lastName;
                cuponsGroupPrinted finded = g.get(group.Id);
                if (finded != null)
                {
                    // Falta ver quien mando a imprimir.
                    finded.Deliver = true;
                    finded.DeliverDate = DateTime.Now;
                    finded.update();
                }
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(finded, settings);
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
        [Route("order/finish/{key}")]
        public JsonResult OrderFinished(string key)
        {
            try
            {
                cuponsGroupPrinted g = new cuponsGroupPrinted();
                List<cuponsGroupPrinted> reponse = g.ordesDisable(key);
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(reponse, settings);
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

        //GET
        [HttpGet]
        [Route("exportexcel/{key}")]
        public ActionResult exportexcel(string key)
        {
            try
            {
                cuponsGroupPrinted obj = new cuponsGroupPrinted();
                List<cuponsGroupPrintedDTO> datos = obj.ordes(key);
                // Crear el Excel
                using (var paquete = new ExcelPackage())
                {
                    var Sheet = paquete.Workbook.Worksheets.Add("Cupones");
                    Sheet.Cells["A1"].Value = "Folio";
                    Sheet.Cells["B1"].Value = "Hotel";
                    Sheet.Cells["C1"].Value = "Tipo";
                    Sheet.Cells["D1"].Value = "Valor";
                    Sheet.Cells["E1"].Value = "Fecha alta";

                    // Recorremos todos los cupones de los grupos.
                    // Cargar datos
                    int row = 2;
                    foreach (cuponsGroupPrintedDTO groups in datos)
                    {
                        foreach (CuponDTO cupon in groups.Cupons)
                        {
                            Sheet.Cells[string.Format("A{0}", row)].Value = cupon.CuponsID;
                            Sheet.Cells[string.Format("B{0}", row)].Value = cupon.Couponrate.Hotel.Name;
                            Sheet.Cells[string.Format("C{0}", row)].Value = cupon.Couponrate.Cupontype.Name;
                            Sheet.Cells[string.Format("D{0}", row)].Value = cupon.Couponrate.Valor;
                            Sheet.Cells[string.Format("E{0}", row)].Value = cupon.CreateDate;
                            Sheet.Cells[string.Format("E{0}", row)].Value = cupon.CreateDate;
                            Sheet.Cells[string.Format("E{0}", row)].Style.Numberformat.Format = "dd/MM/yyyy";
                            row++;
                        }
                    }
                    Sheet.Cells["A:AZ"].AutoFitColumns();
                    var archivoBytes = paquete.GetAsByteArray();
                    return File(archivoBytes,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "Orden:" + key + ".xlsx");
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }


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
        public string pdfsharpRotateText(string text, string fontname, float fontsize, FontStyle fontstyle, SolidBrush color, int rotate = -90)
        {
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
        XImage ConvertBitmapToXImage(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png); // or ImageFormat.Jpeg
                ms.Position = 0;
                return XImage.FromStream(ms);
            }
        }
        /////////////

        private static void DrawImageFit(XGraphics gfx, XImage img, double x, double y, double targetW, double targetH)
        {
            // Tamaño nativo del XImage en puntos (PdfSharp)
            double imgW = img.PointWidth;
            double imgH = img.PointHeight;

            if (imgW <= 0 || imgH <= 0)
            {
                // fallback: si por alguna razón no trae tamaños
                gfx.DrawImage(img, x, y, targetW, targetH);
                return;
            }

            // Escala para encajar COMPLETO (sin recortar), manteniendo proporción
            double scale = Math.Min(targetW / imgW, targetH / imgH);
            double drawW = imgW * scale;
            double drawH = imgH * scale;

            // Centrar en el rectángulo del cupón
            double dx = x + (targetW - drawW) / 2.0;
            double dy = y + (targetH - drawH) / 2.0;

            gfx.DrawImage(img, dx, dy, drawW, drawH);
        }

        /////////// PDF CUPONES
        [HttpGet]
        [Route("exportpdf/{key}")]
        public ActionResult renderpdf(string key)
        {
            try
            {
                cuponsGroupPrinted obj = new cuponsGroupPrinted();

                List<CuponDTO> cupones = obj
                    .ordes(key)
                    .SelectMany(x => x.Cupons)
                    .ToList();

                if (cupones == null || cupones.Count == 0)
                {
                    return HttpNotFound("No se encontraron cupones para generar.");
                }

                string backgroundPath = Server.MapPath(
                    "~/Content/images/Cupones/unlimitedMedia.jpg"
                );

                if (!System.IO.File.Exists(backgroundPath))
                {
                    return HttpNotFound(
                        "No se encontró la plantilla del cupón."
                    );
                }

                PdfDocument document = new PdfDocument();

                document.Info.Title = "Cupones";
                document.Info.Author = "PractiControl";

                using (XImage background = XImage.FromFile(backgroundPath))
                {
                    for (int index = 0; index < cupones.Count; index += 4)
                    {
                        /*
                         * Carta horizontal:
                         * 279.4 mm de ancho
                         * 215.9 mm de alto
                         */

                        PdfPage page = document.AddPage();

                        page.Size = PdfSharp.PageSize.Letter;
                        page.Orientation = PdfSharp.PageOrientation.Landscape;

                        using (XGraphics gfx = XGraphics.FromPdfPage(page))
                        {
                            double couponWidth = page.Width.Point / 4.0;
                            double couponHeight = page.Height.Point;

                            for (int column = 0; column < 4; column++)
                            {
                                int couponIndex = index + column;

                                if (couponIndex >= cupones.Count)
                                {
                                    break;
                                }

                                double couponX = column * couponWidth;

                                DrawCoupon(
                                    gfx,
                                    background,
                                    cupones[couponIndex],
                                    couponX,
                                    0,
                                    couponWidth,
                                    couponHeight
                                );
                            }
                        }
                    }
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    document.Save(stream, false);

                    return File(
                        stream.ToArray(),
                        "application/pdf",
                        "Cupones-" + key + ".pdf"
                    );
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(
                    500,
                    "No fue posible generar el PDF: " + ex.Message
                );
            }
        }

        private void DrawCoupon(
        XGraphics gfx,
        XImage background,
        CuponDTO coupon,
        double couponX,
        double couponY,
        double couponWidth,
        double couponHeight)
        {
            const double templateWidth = 662.0;
            const double templateHeight = 2048.0;

            bool showBackground = true;   // plantilla
            bool showData = true;         // textos + QR

            // Dibujar la plantilla completa.
            if (showBackground)
            {
                gfx.DrawImage(
                    background,
                    couponX,
                    couponY,
                    couponWidth,
                    couponHeight
                );
            }
            else
            {
                gfx.DrawRectangle(
                    XBrushes.White,
                    couponX,
                    couponY,
                    couponWidth,
                    couponHeight
                );
            }

            if (!showData)
                return;

            // Conversión de coordenadas de la plantilla a puntos del PDF.
            Func<double, double> X = px =>
                couponX + ((px / templateWidth) * couponWidth);

            Func<double, double> Y = px =>
                couponY + ((px / templateHeight) * couponHeight);

            Func<double, double> W = px =>
                (px / templateWidth) * couponWidth;

            Func<double, double> H = px =>
                (px / templateHeight) * couponHeight;

            XBrush redBrush = new XSolidBrush(
                XColor.FromArgb(198, 0, 0)
            );

            XBrush blackBrush = XBrushes.Black;

            string folio = coupon.CuponsID.ToString();

            string couponType =
                coupon.Couponrate != null &&
                coupon.Couponrate.Cupontype != null
                    ? coupon.Couponrate.Cupontype.Name
                    : string.Empty;

            decimal value =
                coupon.Couponrate != null
                    ? coupon.Couponrate.Valor
                    : 0;

            string price =
                value.ToString(
                    "C0",
                    new CultureInfo("es-MX")
                ) + " MXN";

            // Temporalmente se toma un año después de la creación.
            string expirationDate =
                coupon.CreateDate
                    .AddYears(1)
                    .ToString("dd/MM/yyyy");

            using (Bitmap qrBitmap = GenerateQrBitmap(
                "k" + coupon.CuponsID,
                600))
            using (MemoryStream qrStream = new MemoryStream())
            {
                qrBitmap.Save(qrStream, ImageFormat.Png);
                qrStream.Position = 0;

                using (XImage qrImage = XImage.FromStream(qrStream))
                {
                    /*
                     * =====================================================
                     * TALÓN SUPERIOR
                     * =====================================================
                     */

                    // Folio superior izquierdo.
                    DrawCenteredText(
                        gfx,
                        folio,
                        new XFont("Arial", 8.5, XFontStyle.Regular),
                        redBrush,
                        X(25),
                        Y(171),
                        W(175),
                        H(43)
                    );

                    // QR superior izquierdo.
                    gfx.DrawImage(
                        qrImage,
                        X(30),
                        Y(217),
                        W(170),
                        H(170)
                    );

                    // Tipo superior derecho (A1, A2...)
                    DrawCenteredText(
                        gfx,
                        couponType,
                        new XFont("Arial", 17, XFontStyle.Bold),
                        blackBrush,
                        X(492),
                        Y(165),          // antes 160
                        W(128),
                        H(70)
                    );

                    // Precio superior
                    DrawCenteredText(
                        gfx,
                        price,
                        new XFont("Arial", 13.5, XFontStyle.Bold),
                        redBrush,
                        X(265),
                        Y(228),          // antes 218
                        W(345),
                        H(55)
                    );

                    // Vigencia superior
                    DrawCenteredText(
                        gfx,
                        expirationDate,
                        new XFont("Arial", 7.5, XFontStyle.Bold),
                        redBrush,
                        X(430),          // antes 446
                        Y(358),          // antes 354
                        W(185),          // antes 170
                        H(28)
                    );

                    /*
                     * =====================================================
                     * CUERPO CENTRAL
                     * =====================================================
                     */

                    // Tipo grande A1/A2 en el costado izquierdo.
                    DrawRotatedText(
                        gfx,
                        couponType,
                        new XFont("Arial", 39, XFontStyle.Bold),
                        blackBrush,
                        X(18),
                        Y(1280),
                        W(185),
                        H(280),
                        -90
                    );

                    // Precio grande central.
                    DrawRotatedText(
                        gfx,
                        price,
                        new XFont("Arial", 24, XFontStyle.Bold),
                        redBrush,
                        X(230),
                        Y(490),
                        W(95),
                        H(500),
                        -90
                    );

                    // Folio junto al QR central.
                    DrawRotatedText(
                        gfx,
                        folio,
                        new XFont("Arial", 14, XFontStyle.Regular),
                        redBrush,
                        X(225),
                        Y(1240),
                        W(70),
                        H(325),
                        -90
                    );

                    // QR central.
                    gfx.DrawImage(
                        qrImage,
                        X(310),
                        Y(1260),
                        W(300),
                        H(300)
                    );

                    // Fecha de vigencia central.
                    DrawRotatedText(
                        gfx,
                        expirationDate,
                        new XFont("Arial", 14, XFontStyle.Bold),
                        redBrush,
                        X(559),
                        Y(510),
                        W(68),
                        H(350),
                        -90
                    );

                    /*
                     * =====================================================
                     * TALÓN INFERIOR
                     * =====================================================
                     */

                    // Folio inferior izquierdo.
                    // Se coloca justo encima del QR, sin tocar el logotipo.
                    DrawCenteredText(
                        gfx,
                        folio,
                        new XFont("Arial", 8.5, XFontStyle.Regular),
                        redBrush,
                        X(25),
                        Y(1795),
                        W(175),
                        H(35)
                    );

                    // QR inferior izquierdo.
                    gfx.DrawImage(
                        qrImage,
                        X(30),
                        Y(1835),
                        W(170),
                        H(170)
                    );

                    // Tipo inferior derecho: A1, A2, etc.
                    // Se baja para colocarlo junto al texto TAXI.
                    DrawCenteredText(
                        gfx,
                        couponType,
                        new XFont("Arial", 17, XFontStyle.Bold),
                        blackBrush,
                        X(500),
                        Y(1795),
                        W(120),
                        H(55)
                    );

                    // Precio inferior.
                    // Se coloca debajo de TAXI y del tipo del cupón.
                    DrawCenteredText(
                        gfx,
                        price,
                        new XFont("Arial", 13.5, XFontStyle.Bold),
                        redBrush,
                        X(260),
                        Y(1850),
                        W(355),
                        H(50)
                    );

                    // Vigencia inferior.
                    // Se baja hasta la zona correspondiente de la plantilla.
                    DrawCenteredText(
                        gfx,
                        expirationDate,
                        new XFont("Arial", 7.5, XFontStyle.Bold),
                        redBrush,
                        X(430),
                        Y(1985),
                        W(168),
                        H(25)
                    );
                }
            }
        }



        /* ===================== Helpers ===================== */

        private static void DrawImageContain(XGraphics gfx, XImage img, double x, double y, double targetW, double targetH)
        {
            (double imgWpt, double imgHpt) = GetImageSizeInPoints(img);

            // Escala para encajar COMPLETO (sin recortar), manteniendo proporción
            double scale = Math.Min(targetW / imgWpt, targetH / imgHpt);
            double drawW = imgWpt * scale;
            double drawH = imgHpt * scale;

            // Centrar en el rectángulo del cupón
            double dx = x + (targetW - drawW) / 2.0;
            double dy = y + (targetH - drawH) / 2.0;

            gfx.DrawImage(img, dx, dy, drawW, drawH);
        }

        private static (double wpt, double hpt) GetImageSizeInPoints(XImage img)
        {
            // Usar medidas propias de XImage si están disponibles
            if (img.PointWidth > 0 && img.PointHeight > 0)
                return (img.PointWidth, img.PointHeight);

            // Fallback usando píxeles y DPI (asumimos 72 si vienen 0)
            double dpiX = img.HorizontalResolution > 0 ? img.HorizontalResolution : 72.0;
            double dpiY = img.VerticalResolution > 0 ? img.VerticalResolution : 72.0;
            double wpt = img.PixelWidth * (72.0 / dpiX);
            double hpt = img.PixelHeight * (72.0 / dpiY);
            return (wpt, hpt);
        }



        // Obtener la informacion
        // POST: Cupons/Create
        [HttpGet]
        [Route("loaddata/")]
        public JsonResult loaddata()
        {
            try
            {
                Cupon _coupon = new Cupon();
                List<Hotel> hotels = new Hotel().get();
                List<Cupontype> Cupontypes= new Cupontype().get();
                LoadRateData loadRateData = new LoadRateData();
                loadRateData.Cupontypes = Cupontypes;
                loadRateData.hotels = hotels;
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(loadRateData, settings);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(null, settings);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }


        // NUEVOS HELPERS

        private static Bitmap GenerateQrBitmap(
        string content,
        int size)
        {
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,

                Options = new EncodingOptions
                {
                    Width = size,
                    Height = size,
                    Margin = 0,
                    PureBarcode = true
                }
            };

            return new Bitmap(writer.Write(content));
        }


        private static void DrawCenteredText(
        XGraphics gfx,
        string text,
        XFont font,
        XBrush brush,
        double x,
        double y,
        double width,
        double height)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            XRect rect = new XRect(
                x,
                y,
                width,
                height
            );

            XStringFormat format = new XStringFormat
            {
                Alignment = XStringAlignment.Center,
                LineAlignment = XLineAlignment.Center
            };

            gfx.DrawString(
                text,
                font,
                brush,
                rect,
                format
            );
        }


        private static void DrawRotatedText(
        XGraphics gfx,
        string text,
        XFont font,
        XBrush brush,
        double x,
        double y,
        double width,
        double height,
        double angle)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            XGraphicsState state = gfx.Save();

            /*
             * Para -90 grados trasladamos el origen a la parte
             * inferior izquierda del rectángulo.
             */
            if (angle == -90)
            {
                gfx.TranslateTransform(x, y + height);
                gfx.RotateTransform(-90);

                XRect rotatedRect = new XRect(
                    0,
                    0,
                    height,
                    width
                );

                gfx.DrawString(
                    text,
                    font,
                    brush,
                    rotatedRect,
                    new XStringFormat
                    {
                        Alignment = XStringAlignment.Center,
                        LineAlignment = XLineAlignment.Center
                    }
                );
            }
            else
            {
                gfx.TranslateTransform(
                    x + width,
                    y
                );

                gfx.RotateTransform(angle);

                XRect rotatedRect = new XRect(
                    0,
                    0,
                    height,
                    width
                );

                gfx.DrawString(
                    text,
                    font,
                    brush,
                    rotatedRect,
                    new XStringFormat
                    {
                        Alignment = XStringAlignment.Center,
                        LineAlignment = XLineAlignment.Center
                    }
                );
            }

            gfx.Restore(state);
        }

    }
}