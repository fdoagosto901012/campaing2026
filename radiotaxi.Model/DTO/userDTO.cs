using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class userDTO
    {
        public int userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public System.DateTime createdDt { get; set; }
        public Nullable<System.DateTime> lastLoginDt { get; set; }
        public Nullable<bool> isActive { get; set; }
        public string phone { get; set; }
        public string userpassword { get; set; }
        public string authyId { get; set; }
    }
}
