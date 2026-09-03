using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public partial class userBloodType
    {
        public userBloodType()
        {
            userHealthInformations = new HashSet<userHealthInformation>();
        }

        public int id { get; set; }
        public string name { get; set; }

        public virtual ICollection<userHealthInformation> userHealthInformations { get; set; }
    }
}
