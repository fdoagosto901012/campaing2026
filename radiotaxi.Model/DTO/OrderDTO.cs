using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class OrderDTO
    {
        public userDTO user { get; set; }
        public DirectionTarrif orderRate { get; set; }
        public orderPreferences orderPreferences { get; set; }
    }
}
