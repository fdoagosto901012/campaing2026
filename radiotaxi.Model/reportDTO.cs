using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
   public class reportDTO
    {    
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> dateCreated { get; set; }
        public Nullable<System.DateTime> dateEdited { get; set; }
        public string createdBy { get; set; }
        public string editedBy { get; set; }
    
        public virtual List<taxiReport> taxiReports { get; set; }
    }
}
