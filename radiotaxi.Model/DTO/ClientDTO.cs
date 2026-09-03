using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public  class ClientDTO
    {
        public string name { get; set; }
        public string lastname1 { get; set; }
        public string lastname2 { get; set; }
        public string address { get; set; }
        public System.DateTime dateCreated { get; set; }
        public Nullable<System.DateTime> dateUpdated { get; set; }
        public Nullable<bool> isBlocked { get; set; }
        public string blockedReason { get; set; }
        public string createdBy { get; set; }
        public string editedBy { get; set; }
        public string phone { get; set; }
        public string note { get; set; }
        public Nullable<bool> isHome { get; set; }
        public string email { get; set; }
        public string authyId { get; set; }
        public string connectionId { get; set; }
        public Nullable<int> padronId { get; set; }
        public string gps_hub_connectionId { get; set; }
        public Nullable<int> TotalRows { get; set; }
    }
}
