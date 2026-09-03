using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.API.cards
{
    public class CardResponse : baseModel 
    {
        public CardResponse() { }
        public card Card { get; set; }
        public EmplacamientoDTO Car { get; set; }
        public Partner Partner { get; set; }
        public Operator Operator { get; set; }
        C_permissions permiso { get; set; }



    }
}
