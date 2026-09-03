using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace radiotaxi.Model
{
    [MetadataType(typeof(Metaprofile))]
    public partial class profile
    {
        public class Metaprofile {
            [JsonIgnore]
            public  ICollection<userProfile> userProfiles { get; set; }
        }
    }
}