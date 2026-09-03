using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace radiotaxi.Model
{
    [MetadataType(typeof(MetaUserDepartment))]
    public partial class userDepartment
    {
        public class MetaUserDepartment {
            [JsonIgnore]
            public virtual userlog userlog { get; set; }
        }
    }
}