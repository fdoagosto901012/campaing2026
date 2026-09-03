using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace radiotaxi.Model
{
    [MetadataType(typeof(Metadataroleparam))]
    [DataContract(IsReference = true)]
    public partial class roleparam2
    {
        public class Metadataroleparam
        {
            [DataMember(Name = "id", Order = 0)]
            public int id { get; set; }
            [DataMember(Name = "paramId", Order = 1)]
            public Nullable<int> paramId { get; set; }
            [DataMember(Name = "roleId", Order = 2)]
            public Nullable<int> roleId { get; set; }

            public virtual parameter parameter { get; set; }

            public virtual role role { get; set; }
        }
    }
}