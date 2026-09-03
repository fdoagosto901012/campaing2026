using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace radiotaxi.Model
{
    [MetadataType(typeof(Metadepartment))]
    public partial class department
    {
        public class Metadepartment
        {
            [JsonIgnore]
            public virtual ICollection<userDepartment> userDepartments { get; set; }
        }
    }
}