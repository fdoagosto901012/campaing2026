
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public partial class amazonPicture
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string bucket { get; set; }
        public string key { get; set; }
        public int pictureTypeId { get; set; }
        public DateTime DtCreated { get; set; }

        public virtual pictureType pictureType { get; set; }
    }
}
