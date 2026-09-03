using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public partial class userHealthInformation
    {
        public int id { get; set; }
        public string allergies { get; set; }
        public bool organDonor { get; set; }
        public int userId { get; set; }
        public int? bloodTypeID { get; set; }

        public virtual userBloodType userBloodType { get; set; }
    }
}
