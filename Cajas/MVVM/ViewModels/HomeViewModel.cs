using Cajas.MVVM.Models;
using Cajas.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Cajas.MVVM.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        //private readonly iAwsS3 _AwsS3 = new AwsS3();

        [ObservableProperty]
        User _user = null;

        [ObservableProperty]
        string _name = null;

        [ObservableProperty]
        ImageSource _imgs = null;

        [ObservableProperty]
        string _type = null;

        [ObservableProperty]
        bool _isbusy = false;


        [ObservableProperty]
        Partner _partner = null;



        public ICommand ICommandNavToChat { get; set; }
        public ICommand ICommandNavToScan { get; set; }
        private List<MedicineReminderModel> _reminderList;
        public List<MedicineReminderModel> ReminderList
        {
            get => _reminderList;

            set
            {
                if (_reminderList == value) return;
                _reminderList = value;
                OnPropertyChanged(nameof(ReminderList));
            }
        }

        public HomeViewModel()
        {
            _reminderList = [];
            ICommandNavToChat = new Command(() => NavToChat());
            ICommandNavToScan = new Command(() => NavToScan());
            //InitList();
            // Cargamos las variables.
            
            /*
            if (app.USER != null && app.USER.partner != null) // Esto define que es un socio.
            {
                this.Name = app.USER.partner.firstName + " ";
                this.Name += app.USER.partner.lastNameF + " ";
                this.Name += app.USER.partner.lastNameM;
                this.User = app.USER;
                // Obtenemos la imagen.
                amazonPicture IMG = app.USER.partner.amazonPictures.Where(x => x.pictureTypeId == 2).FirstOrDefault();
                string _uri = _AwsS3.getImage(IMG.bucket, IMG.key);
                // Obtenemos la imagen.
                this.Imgs = new UriImageSource
                {
                    Uri = new Uri(_uri),
                    CacheValidity = new TimeSpan(10, 0, 0, 0)
                };
                this.Type = "Socio";
            }

            */
            //InitAsync();
        }






        // This helps a ton in unit testing.
        public Task InitializationWork { get; private set; }

        // Async void is only evil because you have to be very disciplined.
        public async void Init()
        {
            // The discipline is DO NOT let exceptions escape.
            try
            {
                this.Isbusy = true;
                InitializationWork = InitAsync();
                await InitializationWork;
            }
            catch (Exception ex)
            {
                // <log the exception>
                // <display an error to the user>
            }
            finally
            {
                this.Isbusy = false;
            }
        }

        private async Task InitAsync()
        {
            // Initialization work goes here
            PartnerGlobalDTO partnerGlobal = await app.USER.getPartnerInfo();
            this.Partner = partnerGlobal.Partner;
            Console.WriteLine("a");
        }



        private static void NavToChat()
        {
            //App.Current?.MainPage?.Navigation.PushAsync(new HomeView());
            //NavigationService.Instance.NavigateToAsync<HomeViewModel>();
        }

        private static void NavToScan()
        {
            //NavigationService.Instance.NavigateToAsync<ScanViewModel>();
        }

        private void InitList()
        {
            ReminderList.Add(new MedicineReminderModel()
            {
                Medicine = "Acetaminophen",
                Dose = "10mg",
                Time = "Before launch 2:00 PM",
            });

            ReminderList.Add(new MedicineReminderModel()
            {
                Medicine = "Naproxen",
                Dose = "10mg",
                Time = "Before launch 2:10 PM",
            });

        }
    }

    public class MedicineReminderModel
    {
        public string Medicine { get; set; } = string.Empty;
        public string Dose { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
    }

}
