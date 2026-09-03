using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public  class ClientSearchParametersDTO
    {
        public string phoneNumber { get; set; } 
        public string name { get; set; }
        public string lastName1 { get; set; } 
        public string lastName2 { get; set; }


        public ClientSearchParametersDTO() {
            this.phoneNumber = "";
            this.name = "";
            this.lastName1 = "";
            this.lastName2 = "";
        }  
    }
}
