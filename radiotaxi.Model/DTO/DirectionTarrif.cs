using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class DirectionTarrif
    {
        public string start_lat { get; set; }
        public string start_lng { get; set; }
        public string end_lat { get; set; }
        public string end_lng { get; set; }
        public string siteId { get; set; }
        public int zoneStart_Id { get; set; }
        public int zoneEnd_Id { get; set; }
        public decimal amount { get; set; }
        public string origin { get; set; }
        public string destiny { get; set; }
        public string origin_description { get; set; }
        public string destiny_description { get; set; }
        public string time { get; set; }
    }
}
