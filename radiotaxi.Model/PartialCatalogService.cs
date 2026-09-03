using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    [MetadataType(typeof(MetadataCatalogService))]
    [DataContract(IsReference = true)]
    public partial class catalogService : baseModel
    {
        public List<catalogService> get(){
            try
            {
                //return db.catalogServices.ToList();
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [DataContract(IsReference = true)]
        public class MetadataCatalogService
        {
            public MetadataCatalogService()
            {
                this.taxiServices = new HashSet<taxiService>();
            }
            [DataMember]
            [Display(Name = "Identificador")]
            public int id { get; set; }
            [Display(Name = "Servicio")]
            [DataMember]
            public string name { get; set; }
            [JsonIgnore]
            public virtual ICollection<taxiService> taxiServices { get; set; }
        }
    }
}
