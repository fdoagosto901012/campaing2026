using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public partial class email
    {
        public int id { get; set; }
        public string email1 { get; set; }
        public int userId { get; set; }
        public int emailPriorityTypeId { get; set; }
        public bool? active { get; set; }

    }
}
