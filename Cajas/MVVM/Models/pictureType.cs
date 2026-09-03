using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public partial class pictureType
    {
        public pictureType()
        {
            amazonPictures = new HashSet<amazonPicture>();
        }

        public int id { get; set; }
        public string name { get; set; }

        public virtual ICollection<amazonPicture> amazonPictures { get; set; }
    }
}
