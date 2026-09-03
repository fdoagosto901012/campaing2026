using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class SearchModelService
    {
        public string phoneNumber { get; set; }
        public int? rtNumber { get; set; }
        public DateTime? datePickUp { get; set; }
    }
}
