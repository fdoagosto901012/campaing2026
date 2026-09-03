using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class day
    {
        public DateTime date_id { get; set; }
        public short date_year { get; set; }
        public byte date_month { get; set; }
        public byte date_day { get; set; }
        public byte weekday_id { get; set; }
        public string weekday_nm { get; set; }
        public string month_nm { get; set; }
        public short day_of_year { get; set; }
        public byte quarter_id { get; set; }
        public DateTime first_day_of_month { get; set; }
        public DateTime last_day_of_month { get; set; }
        public DateTime start_dts { get; set; }
        public DateTime end_dts { get; set; }
    }
}