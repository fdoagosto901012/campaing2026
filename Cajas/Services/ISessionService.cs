using radiotaxi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.Services
{
    public interface ISessionService
    {
        bool IsLoggedIn { get; }
        userlog? CurrentUser { get; }
        string Role { get; }
        void Login(userlog user);
        void Logout();
    }
}
