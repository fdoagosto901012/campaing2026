using radiotaxi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.Services
{
    public class SessionService : ISessionService
    {
        public bool IsLoggedIn =>
            CurrentUser != null;

        public userlog? CurrentUser { get; private set; }

        public string Role { get; private set; } = string.Empty;

        public void Login(userlog user)
        {
            CurrentUser = user;
            Role = user.Role ?? string.Empty;
        }

        public void Logout()
        {
            CurrentUser = null;
            Role = string.Empty;
        }
    }
}
