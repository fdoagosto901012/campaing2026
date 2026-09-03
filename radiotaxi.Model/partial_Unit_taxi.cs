using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    [MetadataType(typeof(Metadataunit_taxi))]
    public partial class unit_taxi
    {
        public class Metadataunit_taxi
        {
            public Metadataunit_taxi()
            {
                
            }
            
            [JsonIgnore]
            public virtual client client { get; set; }
        }
    }
}
