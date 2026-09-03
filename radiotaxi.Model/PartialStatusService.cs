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
    [MetadataType(typeof(MetadataStatusService))]
    public partial class statusService : baseModel
    {

        public List<statusService> get() {

            try
            {
                //return db.statusServices.ToList();
                return null;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public class MetadataStatusService
        {
            public MetadataStatusService()
            {
                this.taxiServices = new HashSet<taxiService>();
            }
            [DataMember]
            [Display(Name = "Identificador")]
            public int id { get; set; }
            [Display(Name = "Estado")]
            [DataMember]
            public string name { get; set; }
            [JsonIgnore]
            public virtual ICollection<taxiService> taxiServices { get; set; }
        }
    }
}
