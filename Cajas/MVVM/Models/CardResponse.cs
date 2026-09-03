using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cajas.MVVM.Models.@base;

namespace Cajas.MVVM.Models
{
    public class CardResponse : BaseModel
    {
        public card Card { get; set; }
        public EmplacamientoDTO Car { get; set; }
        public Partner Partner { get; set; }
        public Operator Operator { get; set; }
    }
}
