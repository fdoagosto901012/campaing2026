using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace radiotaxi.Model
{
    [MetadataType(typeof(MetarRpleParam))]
    public partial class roleparam
    {
        public class MetarRpleParam {
            [JsonIgnore]
            public virtual role role { get; set; }
        }
    }
}