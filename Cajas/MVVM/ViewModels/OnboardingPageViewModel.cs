using Cajas.MVVM.ViewModels.log;
using Cajas.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cajas.MVVM.ViewModels
{
    public class OnboardingPageViewModel : BaseViewModel
    {
        private List<string> onboardingList;
        private int position = 0;

        public List<string> OnboardingList
        {
            get => onboardingList;
            set
            {
                if (onboardingList == value) return;
                onboardingList = value;
                OnPropertyChanged(nameof(OnboardingList));
            }
        }

        public int Position
        {
            get => position;
            set
            {
                if (position == value) return;
                position = value;
                OnPropertyChanged(nameof(Position));
            }
        }


        public ICommand ICommandNavToLoginPage { get; set; }

        public OnboardingPageViewModel()
        {
            onboardingList = [];
            ICommandNavToLoginPage = new Command(() => NavigateToLoginPage());
            InitilizeOnboardingList();
            CarouselRotateService();
        }

        private static void NavigateToLoginPage()
        {
            NavigationService.Instance.NavigateToAsync<LoginPageViewModel>();
        }

        private void InitilizeOnboardingList()
        {
            OnboardingList.Add("Tu comodidad y bienestar son la mejor ruta hacia nuevas aventuras.");
            OnboardingList.Add("Cada kilómetro recorrido es una oportunidad para aprender algo nuevo.");
            OnboardingList.Add("¡Gracias por viajar con nosotros, que tengas un gran día!");
        }

        private async void CarouselRotateService()
        {
            if (OnboardingList != null && OnboardingList.Count != 0)
            {
                using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
                while (await timer.WaitForNextTickAsync())
                {
                    Position = (Position + 1) % OnboardingList.Count;
                }
            }
        }
    }
}