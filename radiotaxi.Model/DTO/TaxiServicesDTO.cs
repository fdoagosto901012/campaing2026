using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class TaxiServicesDTO : baseModel
    {
        // Datos Cliente

        public string name { get; set; }
        public string lastname1 { get; set; }
        public string lastname2 { get; set; }

        // Datos del servicio
        public int id { get; set; }
        public string startPoint { get; set; }
        public string endPoint { get; set; }
        public System.DateTime dateRequested { get; set; }
        public string createdBy { get; set; }
        public string editedBy { get; set; }
        public string phoneId { get; set; }
        public int statusServiceId { get; set; }
        public string note { get; set; }
        public System.DateTime datePickUp { get; set; }
        public bool autoRenew { get; set; }
        public int baseServiceId { get; set; }
        public int catalogServiceId { get; set; }
        public string days { get; set; }
        public Nullable<int> AppServiceId { get; set; }
        public string operatorPhoneId { get; set; }
        public string start_lat { get; set; }
        public string start_lng { get; set; }
        public string end_lat { get; set; }
        public string end_lng { get; set; }
        public Nullable<int> siteId { get; set; }
        public Nullable<int> zoneStart_Id { get; set; }
        public Nullable<int> zoneEnd_Id { get; set; }
        public Nullable<decimal> amount { get; set; }
        public string origin { get; set; }
        public string destiny { get; set; }
        public string origin_description { get; set; }
        public string destiny_description { get; set; }
        public string Refrence { get; set; }
        public Nullable<System.DateTime> board_taxi_date { get; set; }
        public Nullable<System.DateTime> finish_taxi_date { get; set; }
        public Nullable<bool> isReservation { get; set; }
        public string noteOperator { get; set; }
        public int TotalRows { get; set; }
    }
}
