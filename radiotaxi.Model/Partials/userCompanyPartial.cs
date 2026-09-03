using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace radiotaxi.Model
{
    [MetadataType(typeof(metauserCompany))]
    public partial class userCompany
    {
        public class metauserCompany{
            [JsonIgnore]
            public virtual userlog userlog { get; set; }
        }
    }
}