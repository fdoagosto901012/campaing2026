using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.GeoLocations
{
    public class userGeolocationDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public double speed { get; set; }
        public System.Data.Entity.Spatial.DbGeography geolocation { get; set; }
        public System.DateTime DateCreate { get; set; }
        public string type { get; set; }
        public string OpenPayOrderId { get; set; }
    }
}
