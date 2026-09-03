using Newtonsoft.Json;
using radiotaxi.Model.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace radiotaxi.Model
{
    [MetadataType(typeof(MetadataTaxiService))]
    public partial class taxiService : baseModel
    {
        // Guardar 
        public taxiService save()
        {
            try
            {
                /*
                taxiService _existClient = db.taxiServices.Find(this.id);
                Message = "Cliente creado";
                StatusAction = success;
                db.taxiServices.Add(this);
                db.SaveChanges();
                return this;
                */
                return null;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                Message = e.Message;
                StatusAction = error;
                return null;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                StatusAction = error;
                return null;
            }
        }

        public TaxiServicesPagination getPending(int page = 1, int pagesize = 100) {
            try
            {
                string query = "SevicesPending " + page + ", " + pagesize;
                List<TaxiServicesDTO> services = this.db.Database.SqlQuery<TaxiServicesDTO>(query).ToList();
                int NumberOfServices = 0;
                if (services.Count > 0) NumberOfServices = (int)services.FirstOrDefault().TotalRows;
                int TotalPages = (int)Math.Ceiling((double)NumberOfServices / pagesize);
                TaxiServicesPagination taxiServicesPagination = new TaxiServicesPagination(NumberOfServices, services, page, pagesize);
                return taxiServicesPagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public TaxiServicesPagination getCanceled(int page = 1, int pagesize = 100)
        {
            try
            {
                string query = "SevicesCanceled " + page + ", " + pagesize;
                List<TaxiServicesDTO> services = this.db.Database.SqlQuery<TaxiServicesDTO>(query).ToList();
                int NumberOfServices = 0;
                if (services.Count > 0) NumberOfServices = (int)services.FirstOrDefault().TotalRows;
                int TotalPages = (int)Math.Ceiling((double)NumberOfServices / pagesize);
                TaxiServicesPagination taxiServicesPagination = new TaxiServicesPagination(NumberOfServices, services, page, pagesize);
                return taxiServicesPagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public TaxiServicesPagination getAsigment(int page = 1, int pagesize = 100)
        {
            try
            {
                string query = "SevicesAssigned " + page + ", " + pagesize;
                List<TaxiServicesDTO> services = this.db.Database.SqlQuery<TaxiServicesDTO>(query).ToList();
                int NumberOfServices = 0;
                if (services.Count > 0) NumberOfServices = (int)services.FirstOrDefault().TotalRows;
                int TotalPages = (int)Math.Ceiling((double)NumberOfServices / pagesize);
                TaxiServicesPagination taxiServicesPagination = new TaxiServicesPagination(NumberOfServices, services, page, pagesize);
                return taxiServicesPagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public TaxiServicesPagination getTotal(int page = 1, int pagesize = 100)
        {
            try
            {
                string query = "SevicesTotal " + page + ", " + pagesize;
                List<TaxiServicesDTO> services = this.db.Database.SqlQuery<TaxiServicesDTO>(query).ToList();
                int NumberOfServices = 0;
                if (services.Count > 0) NumberOfServices = (int)services.FirstOrDefault().TotalRows;
                int TotalPages = (int)Math.Ceiling((double)NumberOfServices / pagesize);
                TaxiServicesPagination taxiServicesPagination = new TaxiServicesPagination(NumberOfServices, services, page, pagesize);
                return taxiServicesPagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public Dictionary<string, string> getCounterResume() {

            Dictionary<string, string> counters = new Dictionary<string, string>();
            //ServicesCountResume
            using (SqlConnection dbConnection = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                try
                {
                    string query = "ServicesCountResume";
                    Dictionary<string, int> resume = new Dictionary<string, int>();
                    var cmd = db.Database.Connection.CreateCommand();
                    cmd.CommandText = query;
                    db.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();
                    do
                    {
                        while (reader.Read())
                        {
                            if (this.HasColumn(reader,"Asigment"))
                            {
                                counters.Add("Asigment", reader["Asigment"].ToString());
                            }
                            if (this.HasColumn(reader, "Pending"))
                            {
                                counters.Add("Pending", reader["Pending"].ToString());
                            }
                            if (this.HasColumn(reader, "Canceled"))
                            {
                                counters.Add("Canceled", reader["Canceled"].ToString());
                            }
                            if (this.HasColumn(reader, "Finish"))
                            {
                                counters.Add("Finish", reader["Finish"].ToString());
                            }
                            if (this.HasColumn(reader, "board"))
                            {
                                counters.Add("board", reader["board"].ToString());
                            }
                            if (this.HasColumn(reader, "total"))
                            {
                                counters.Add("total", reader["total"].ToString());
                            }
                        }
                    }
                    while (reader.NextResult());
                    return counters;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }finally { 
                    db.Database.Connection.Close(); 
                }

            }
        }

        public class MetadataTaxiService
        {
            public MetadataTaxiService()
            {
                this.rtNumberServices = new List<rtNumberService>();    
            }

            public int id { get; set; }
            [Required(ErrorMessage = "Requiredo")]
            [Display(Name = "Origen")]
            public string startPoint { get; set; }
            [Display(Name = "Destino")]
            public string endPoint { get; set; }
            [Display(Name = "Fecha solicitado")]
            public System.DateTime dateRequested { get; set; }
            [Display(Name = "Creado Por")]
            public string createdBy { get; set; }
            [Display(Name = "Editado Por")]
            public string editedBy { get; set; }
            [Display(Name = "Teléfono Cliente")]
            [StringLength(10, MinimumLength = 10, ErrorMessage = "El número telefónico debe contener 10 dígitos")]
            [Required(ErrorMessage = "Requiredo")]
            //[RegularExpression(@"^[0-9]$", ErrorMessage = "Solamente número con clave lada")]
            //[Range(0, int.MaxValue, ErrorMessage = "Error de rango")]
            public string phoneId { get; set; }
            [Display(Name = "Estado")]
            public int statusServiceId { get; set; }
            [Display(Name = "Observación")]
            public string note { get; set; }
            [Display(Name = "Hora del Servicio")]
            [DataType(DataType.DateTime)]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
            public System.DateTime datePickUp { get; set; }
            [Display(Name = "Renovación Automática")]
            public bool autoRenew { get; set; }
            [Display(Name = "Base")]
            public int baseServiceId { get; set; }
            [Display(Name = "Tipo de servicio")]
            public int catalogServiceId { get; set; }
            [Display(Name = "´Días de renovación")]
            public string days { get; set; }

            /*
            [JsonIgnore]
            public virtual client client { get; set; }
            [JsonIgnore]
            public virtual client client1 { get; set; }
             */ 


            public virtual List<rtNumberService> rtNumberServices { get; set; }
            public virtual statusService statusService { get; set; }
            public virtual baseService baseService { get; set; }
        }
    }
}
