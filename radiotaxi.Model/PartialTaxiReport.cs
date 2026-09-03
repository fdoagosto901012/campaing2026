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
    [MetadataType(typeof(MetadataTaxiReport))]
    [DataContract(IsReference = true)]
    public partial class taxiReport
    {
        [DataContract(IsReference = true)]
        public class MetadataTaxiReport
        {
            [DataMember]
            public int id { get; set; }
            [DataMember]
            [Display(Name = "Número RT")]
            public Nullable<int> taxiNumber { get; set; }
            [DataMember]
            public int reportId { get; set; }
            
            [JsonIgnore]
            public virtual report report { get; set; }

        }
    }
}
