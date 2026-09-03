using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class taxiService
    {
        public class MetadatataxiService{
            [JsonIgnore]
            public virtual client client1 { get; set; }
        }
    }
}
