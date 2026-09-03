using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace radiotaxi.Model
{
    
    [MetadataType(typeof(MetarParam))]
    public partial class parameter
    {
        public class MetarParam
        {
            [JsonIgnore]
            public virtual ICollection<roleparam> roleparams { get; set; }
        }
    }
}