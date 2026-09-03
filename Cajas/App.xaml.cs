using Cajas.MVVM.Models;
using Newtonsoft.Json;
using Cajas.MVVM.Pages;
using radiotaxi.Model;
using Cajas.Services;

namespace Cajas
{
    public partial class App : Application
    {

        private readonly IServiceProvider _services;

        private const string _TOKEN = "_token_";
        public string TOKEN
        {
            get
            {
                if (Preferences.ContainsKey(_TOKEN))
                {
                    return Preferences.Get(_TOKEN, null);
                }
                return "";
            }
            set
            {
                Preferences.Set(_TOKEN, value);
            }
        }

        private const string _USERLOG = "_userlog_";
        public userlog USERLOG
        {
            get
            {
                if (Preferences.ContainsKey(_USERLOG))
                {
                    return JsonConvert.DeserializeObject<userlog>(Preferences.Get(_USERLOG, null));
                }
                return null;
            }
            set
            {
                Preferences.Set(_USERLOG, JsonConvert.SerializeObject(value));
            }
        }

        private const string _USER = "_user_";
        public User USER
        {
            get
            {
                if (Preferences.ContainsKey(_USER))
                {
                    return JsonConvert.DeserializeObject<User>(Preferences.Get(_USER, null));
                }
                return null;
            }
            set
            {
                Preferences.Set(_USER, JsonConvert.SerializeObject(value));
            }
        }


        private const string _ROLE = "_role_";
        public string ROLE
        {
            get
            {
                if (Preferences.ContainsKey(_ROLE))
                {
                    return Preferences.Get(_ROLE, null);
                }
                return "";
            }
            set
            {
                Preferences.Set(_ROLE, value);
            }
        }


        private const string _partnerGlobal = "_global_";
        public PartnerGlobalDTO partnerGlobal
        {
            get
            {
                if (Preferences.ContainsKey(_partnerGlobal))
                {
                    return JsonConvert.DeserializeObject<PartnerGlobalDTO>(Preferences.Get(_partnerGlobal, null));
                }
                return null;
            }
            set
            {
                Preferences.Set(_partnerGlobal, JsonConvert.SerializeObject(value));
            }
        }


        static public App CurrentApp
        {
            get { return (App)App.Current; }
        }


        public App(IServiceProvider services)
        {
            InitializeComponent();
            this.TOKEN = "";

            _services = services;

            if (USERLOG != null &&
                USERLOG.Role == "Admin")
            {
                MainPage =
                    _services.GetRequiredService<AppShell>();
            }
            else
            {
                var loginPage =
                    _services.GetRequiredService<LoginPage>();
                MainPage =
                    new NavigationPage(loginPage);
            }
        }
    }
}
