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

    [MetadataType(typeof(MetadataAppService))]
    public partial class AppService
    {
        public class MetadataAppService {
            [JsonIgnore]
            public virtual ICollection<taxiService> taxiServices { get; set; }
        }
    }
}
