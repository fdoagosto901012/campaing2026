using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Openpay
{
    public class CardSavedDto
    {
        public string Id { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public string Last4 { get; set; }
    }
}
