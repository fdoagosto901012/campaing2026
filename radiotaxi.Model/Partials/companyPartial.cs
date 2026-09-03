using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace radiotaxi.Model
{
    [MetadataType(typeof(metacompany))]
    public partial class company
    {
        public class metacompany {
            [JsonIgnore]
            public virtual ICollection<userCompany> userCompanies { get; set; }
        }
    }
}