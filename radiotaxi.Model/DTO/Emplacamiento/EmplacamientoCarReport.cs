using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public  class EmplacamientoCarReport
    {
        public decimal No_Economico { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Capacidad { get; set; }
        public DateTime FechaOp { get; set; }
        public int AutoAno { get; set; }
        public string status { get; set; }
    }
}
