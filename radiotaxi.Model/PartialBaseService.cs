using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace radiotaxi.Model
{
    [MetadataType(typeof(MetadataBaseService))]
    [DataContract(IsReference = true)]
    public partial class baseService : baseModel
    {




        public baseService(int id)
        {
            try
            {
                /*
                baseService BaseService = (baseService)db.baseServices.Find(id);
                var properties = typeof(baseService).GetProperties().ToList();
                if (BaseService != null)
                {
                    foreach (var p in properties)
                    {
                        var found = properties.FirstOrDefault(i => i.Name == p.Name);
                        if (found != null)
                        {
                            found.SetValue(this, p.GetValue(BaseService));
                        }
                    }
                }*/
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        // Guardar 
        public baseService save()
        {
            try
            {
                /*
                baseService _existBase = db.baseServices.Find(this.id);
                if (_existBase == null)
                {
                    Message = "Base creado";
                    StatusAction = success;
                    db.baseServices.Add(this);
                    db.SaveChanges();
                    return this;
                }
                else
                {
                    Message = "El Base ya existe";
                    StatusAction = error;
                    return _existBase;
                }
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

        // Actualizar
        public bool update()
        {
            try
            {
                /*
                baseService BaseService = (baseService)db.baseServices.Find(this.id);
                db.Entry(BaseService).State = EntityState.Modified;
                db.SaveChanges();
                Message = "Se actualizo correctamente.";
                StatusAction = success;
                */
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

        public List<baseService> get() {
            try
            {
                /*List<baseService> BaseServices = db.baseServices.ToList();
                return BaseServices;*/
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [DataContract(IsReference = true)]
        public class MetadataBaseService
        {
            public MetadataBaseService()
            {
                this.taxiServices = new HashSet<taxiService>();
            }
            [DataMember]
            [Display(Name = "Identificador")]
            public int id { get; set; }
            [Display(Name = "Base")]
            [DataMember]
            public string name { get; set; }
            [JsonIgnore]
            public ICollection<taxiService> taxiServices { get; set; }
        }
    }
}
