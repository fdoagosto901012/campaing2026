using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.API.cards
{
    public class creditCardDTO
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string Ultimos4 { get; set; }
        public string token { get; set; }
        public System.DateTime DateCreate { get; set; }
        public Nullable<bool> active { get; set; }


        public string type { get; set; }

        public string brand { get; set; }
        public string card_number { get; set; }
        public string bank_name { get; set; }
        public string customer_id { get; set; }
    }
}
