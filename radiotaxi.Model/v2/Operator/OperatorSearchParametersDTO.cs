using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Operator
{
    public class OperatorSearchParametersDTO
    {
        public string gafet { get; set; }
        public string name { get; set; }
        public string lastName1 { get; set; }
        public string lastName2 { get; set; }

        public OperatorSearchParametersDTO()
        {
            this.gafet = "";
            this.name = "";
            this.lastName1 = "";
            this.lastName2 = "";
        }
    }
}
