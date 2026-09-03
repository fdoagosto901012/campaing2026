using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class ListFinishedDTO
    {
        public int numberOfService_Finalizado { get; set; }
        public string _mouth_name { get; set; }
        public int _day { get; set; }
        public int _mouth { get; set; }
        public int _year { get; set; }
        public string _date { get; set; }
        public Dictionary<string, string> dictionary { get; set; }


    }
}
