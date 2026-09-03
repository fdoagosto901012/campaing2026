using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace radiotaxi.Model
{
    public class activismDTO
    {
        public int userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public int roleId { get; set; }
    }
}