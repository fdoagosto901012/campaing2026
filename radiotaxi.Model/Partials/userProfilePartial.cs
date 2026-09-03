using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace radiotaxi.Model
{
    [MetadataType(typeof(MetaUserProfile))]
    public partial class userProfile
    {
        public class MetaUserProfile {
            [JsonIgnore]
            public virtual user user { get; set; }
        }
    }
}