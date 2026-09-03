using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class UserResponse : baseModel
    {
        public UserResponse() { }
        public user User { get; set; }
        public userlog Userlog { get; set; }
    }
}
