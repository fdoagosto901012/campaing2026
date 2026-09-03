using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class ConvenioResponse
    {
        public ConvenioResponse() { }
        public Convenio convenio { get; set; }
        public List<ConveniosDetalle> partidas { get; set; }
    }
}