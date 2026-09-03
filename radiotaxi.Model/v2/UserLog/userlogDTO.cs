using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class userlogDTO : baseModel
    {
        public bool alreadyExisted { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        private userlog User { get; set; }
        public bool isLoggedIn { get; set; }
        public bool isError { get; set; }
        public string ErrorMessage { get; set; }
        public bool isPasswordValid { get; set; }
        public string Role { get; set; }
        public int TotalRows { get; set; }
        public String RoleName { get; set; }
        public String partnerReference { get; set; }

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
        public string fcmToken { get; set; }
        public Nullable<int> padronID { get; set; }
    }
}
