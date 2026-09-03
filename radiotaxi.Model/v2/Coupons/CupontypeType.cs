using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class CupontypeDTO
    {
        public int CupontypeID { get; set; }
        public string Name { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public System.DateTime EditedDate { get; set; }
        public bool active { get; set; }
    }
}
