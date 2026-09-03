using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class cuponsGroupPrintedDTO
    {
        public int Id { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool Printed { get; set; }
        public Nullable<System.DateTime> PrintedDate { get; set; }
        public bool Deliver { get; set; }
        public Nullable<System.DateTime> DeliverDate { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<CuponDTO> Cupons { get; set; }
    }
}