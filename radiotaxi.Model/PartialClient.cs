using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    [MetadataType(typeof(Metadataclient))]
    public partial class client : baseModel
    {
        public Nullable<int> TotalRows { get; set; }

        public client(string id) {
            try
            {
                client Client = (client)db.clients.Find(id);
                var properties = typeof(client).GetProperties().ToList();
                if (Client != null)
                {
                    foreach (var p in properties)
                    {
                        var found = properties.FirstOrDefault(i => i.Name == p.Name);
                        if (found != null)
                        {
                            found.SetValue(this, p.GetValue(Client));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        // Guardar 
        public client save()
        {
            try
            {
                /*
                client _existClient = db.clients.Find(this.phone);
                if (_existClient == null)
                {
                    Message = "Cliente creado";
                    StatusAction = success;
                    this.dateCreated = DateTime.Now;
                    db.clients.Add(this);
                    db.SaveChanges();
                }
                else
                {
                    this.Message = "El Cliente ya existe";
                    this.StatusAction = error;
                }
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
                Message = "Verifique la información del cliente.";
                Console.WriteLine(e.Message);
                StatusAction = error;
                return this;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                StatusAction = error;
                return this;
            }
        }

        // Actualizar
        public bool update()
        {
            try
            {
                /*
                client Client = (client)db.clients.Find(this.phone);
                Client.name = this.name;
                Client.lastname1 = this.lastname1;
                Client.lastname2 = this.lastname2;
                Client.address = this.address;
                Client.note = this.note;
                Client.isBlocked = this.isBlocked;
                Client.blockedReason = this.blockedReason;
                db.Entry(Client).State = EntityState.Modified;
                db.SaveChanges();
                Message = "Se actualizo correctamente.";
                StatusAction = success;*/
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                StatusAction = error;
                return false;
            }
        }

        // Borrar
        public bool delete()
        {
            return true;

        }

        public ClientPagination get_clients(int page = 1, int pagesize = 100, string phoneNumber = null, string Name = null, string lastName1 = null, string lastName2 = null)
        {
            phoneNumber = phoneNumber == "" || phoneNumber is null ? "null" : phoneNumber;
            Name = Name == "" || Name is null ? "null" : Name;
            lastName1 = lastName1 == "" || lastName1 is null ? "null" : lastName1;
            lastName2 = lastName2 == "" || lastName2 is null ? "null" : lastName2;
            string query = "GetAllClientsTest " + page + ", " + pagesize + ", " + phoneNumber + ", " + Name + ", " + lastName1 + ", " + lastName2;
            List<ClientDTO> clients = this.db.Database.SqlQuery<ClientDTO>(query).ToList();
            int NumberOfClients = 0;
            if (clients.Count > 0) NumberOfClients = (int)clients.FirstOrDefault().TotalRows;
            int TotalPages = (int)Math.Ceiling((double)NumberOfClients / pagesize);
            ClientPagination clientPagination = new ClientPagination(NumberOfClients, clients, page, pagesize);
            return clientPagination;
        }

        public ClientPagination search_clients(int page = 1, int pagesize = 100, string phone = null, string name = null, string father = null, string mother = null)
        {
            /*
             GetAllClientsTest pagina ,Numero por pagina , telefono, Nombre , paterno, materno
             GetAllClientsTest 1, 100, null, 'Fernando', 'MAGAÑA ', null
             */

            List<ClientDTO> clients = this.db.Database.SqlQuery<ClientDTO>("GetAllClientsTest " + page + ", " + pagesize + ", null, null, null, null").ToList();
            int NumberOfClients = 0;
            if (clients.Count > 0) NumberOfClients = (int)clients.FirstOrDefault().TotalRows;
            int TotalPages = (int)Math.Ceiling((double)NumberOfClients / pagesize);

            ClientPagination clientPagination = new ClientPagination(NumberOfClients, clients, page, pagesize);

            return clientPagination;
        }

        public class Metadataclient
        {
            public Metadataclient()
            {
                this.taxiServices = new HashSet<taxiService>();
            }
            [Required(ErrorMessage = "Requiredo")]
            [Display(Name = "Nombre")]
            public string name { get; set; }
            [Required(ErrorMessage = "Requiredo")]
            [Display(Name = "Apellido paterno")]
            public string lastname1 { get; set; }
            [Required(ErrorMessage = "Requiredo")]
            [Display(Name = "Apellido materno")]
            public string lastname2 { get; set; }
            [Display(Name = "Dirección")]
            [Required(ErrorMessage = "Requiredo")]
            public string address { get; set; }
            [Display(Name = "Fecha Creación")]
            public System.DateTime dateCreated { get; set; }
            [Display(Name = "Fecha Edición")]
            public Nullable<System.DateTime> dateUpdated { get; set; }
            [Display(Name = "¿Bloqueado?")]
            public bool isBlocked { get; set; }
            [Display(Name = "Razón de bloqueo")]
            public string blockedReason { get; set; }
            [Display(Name = "Creado Por")]
            public string createdBy { get; set; }
            [Display(Name = "Editado Por")]
            public string editedBy { get; set; }
            [Display(Name = "Teléfono")]
            [StringLength(10, MinimumLength = 10, ErrorMessage = "El número telefónico debe contener 10 dígitos")]
            [Required(ErrorMessage = "Requiredo")]
            //[RegularExpression(@"^[0-9]$", ErrorMessage = "Solamente número con clave lada")]
            //[Range(0, int.MaxValue, ErrorMessage = "Error de rango")]
            public string phone { get; set; }
            [Display(Name = "Observación")]
            public string note { get; set; }
            [Display(Name = "Es de casa?")]
            public Nullable<bool> isHome { get; set; }
            [JsonIgnore]
            public virtual ICollection<taxiService> taxiServices { get; set; }
            [JsonIgnore]
            public virtual ICollection<taxiService> taxiServices1 { get; set; }


        }
    }
}