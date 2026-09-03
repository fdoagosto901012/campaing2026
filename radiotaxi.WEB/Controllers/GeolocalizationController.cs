using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using radiotaxi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace radiotaxi.WEB.Controllers
{
    [RoutePrefix("Geolocalization")]
    public class GeolocalizationController : webBaseController
    {
        // GET: Geolocalization
        public ActionResult Index()
        {
            return View();
        }

        // GET: Geolocalization/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Geolocalization/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Geolocalization/Create
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

        // GET: Geolocalization/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Geolocalization/Edit/5
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

        // GET: Geolocalization/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Geolocalization/Delete/5
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

        // GET: Geolocalization/Create
        [Route("map")]
        public ActionResult geo()
        {
            // Obtenemos la ubicacion...
            return View();
        }

        // POST: Partner/GetCities
        [HttpGet]
        [Route("points/")]
        public JsonResult getpoint()
        {
            try
            {
                List<userGeolocation> points = new userGeolocation().get();
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(points, settings);
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

        // POST: Partner/GetCities
        [HttpGet]
        [Route("points/{id}")]
        public JsonResult getpoint(int id)
        {
            try
            {
                List<city> cities = new city().get(id);
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };
                string json = JsonConvert.SerializeObject(cities, settings);
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

    }
}
