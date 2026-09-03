using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public partial class userAddress
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public userAddress()
        {
        }

        public int id { get; set; }
        public int userId { get; set; }
        public string supermanzana { get; set; }
        public string manzana { get; set; }
        public string lote { get; set; }
        public string street { get; set; }
        public string interiorNumber { get; set; }
        public string colony { get; set; }
        public string postalCode { get; set; }
        public int? sectionId { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public string address { get; set; }
        public string address2 { get; set; }
        public int? stateId { get; set; }
        public int? cityId { get; set; }
        public bool? active { get; set; }
        public int? zoneId { get; set; }
        public DateTime? createdDate { get; set; }
        public DateTime? editDate { get; set; }
        public string createdBy { get; set; }
        public string editBy { get; set; }
        public bool? isValid { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual city city { get; set; }
        public virtual state state { get; set; }
        public virtual User user { get; set; }
    }
}
