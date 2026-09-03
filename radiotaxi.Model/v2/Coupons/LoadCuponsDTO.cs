using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Coupons
{
    public class LoadCuponsDTO
    {
        public List<cuponsGroupPrintedDTO> cuponsGroupPrinteds {  get; set; }
        public List<cuponsGroupPrintedDTO> NOcuponsGroupPrinteds {  get; set; }
        public Dictionary<string, List<cuponsGroupPrintedDTO>> orders { get; set; }
    }
}