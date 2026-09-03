using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace radiotaxi.Model
{
    public class RegisterResponseDTO
    {
        public int status { get; set; }
        public string message { get; set; }
        public user User { get; set; }
    }
}