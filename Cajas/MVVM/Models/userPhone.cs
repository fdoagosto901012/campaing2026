
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public partial class userPhone
    {
        public userPhone()
        {

        }

        public int id { get; set; }
        public int userId { get; set; }
        public string phone { get; set; }
        public int phoneTypeId { get; set; }
        public bool? active { get; set; }
        public bool? isValid { get; set; }
        public int? source { get; set; }
    }
}
