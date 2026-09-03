using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.DTO
{
    public class LoadRateData
    {
        public List<Hotel> hotels { get; set; }
        public List<Cupontype> Cupontypes { get; set; }
    }
}
