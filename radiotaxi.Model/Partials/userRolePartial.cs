using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace radiotaxi.Model
{
    [MetadataType(typeof(MetauserRole))]
    public partial class userRole
    {
         public class MetauserRole {
            [JsonIgnore]
            public virtual userlog userlog { get; set; }
        }
    }
}