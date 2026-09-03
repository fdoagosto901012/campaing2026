using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class roleDTO
    {
        public int id { get; set; }
        public string role { get; set; }
        public int companyid { get; set; }
        public string company { get; set; }
        public string department { get; set; }
        public int departmentid { get; set; }
    }
}
