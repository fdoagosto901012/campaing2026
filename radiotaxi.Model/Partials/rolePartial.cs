using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace radiotaxi.Model
{
    [MetadataType(typeof(MetaRole))]
    public partial class role
    {
        public class MetaRole {
            [JsonIgnore]
            public virtual ICollection<userRole> userRoles { get; set; }
        }
    }
}