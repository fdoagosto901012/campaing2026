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
    [MetadataType(typeof(MetadatartNumberService))]
    [DataContract(IsReference = true)]
    public partial class rtNumberService
    {
        [DataContract(IsReference = true)]
        public class MetadatartNumberService
        {
            [DataMember]
            [Display(Name = "Identificador")]
            public int id { get; set; }
            [DataMember]
            [Display(Name = "Número económico")]
            public Nullable<int> taxiNumber { get; set; }
            [Display(Name = "Número RT")]
            [DataMember]
            public Nullable<int> rtNumber { get; set; }
            [Display(Name = "Identificador del servicio")]
            [DataMember]
            public int serviceId { get; set; }

            [JsonIgnore]
            public virtual taxiService taxiService { get; set; }
        }
    }
}
