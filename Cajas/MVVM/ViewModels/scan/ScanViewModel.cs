using Cajas.MVVM.Models;
using Cajas.MVVM.ViewModels;
using Cajas.MVVM.ViewModels.cards;
using Cajas.MVVM.ViewModels.cars;
using Cajas.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace Cajas.MVVM.ViewModels.scan
{
    public partial class ScanViewModel : BaseViewModel
    {
        [ObservableProperty]
        bool _isbusy = false;

        string pat = @"\bT-\d+";

        public bool isScannig { get; set; }
        public ICommand ICommandNavToDoScan { get; set; }
        public ScanViewModel()
        {
            isScannig = false;
            ICommandNavToDoScan = new Command(() => DoScan());
        }

        private static void DoScan()
        {
            //App.Current?.MainPage?.Navigation.PushAsync(new HomePage());
        }

        public async void Search(string code)
        {
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            if (Isbusy == false)
            {
                Match m = r.Match(code);
                Isbusy = true;
                if (m.Success) // Es un tarjeton... (Renderiza la vista de tarjton)
                {
                    card Card = new card();
                    code = code.Replace("T-", "");
                    CardResponse _card = await Card.get(code);
                    await NavigationService.Instance.NavigateToAsync<CardViewModel>(_card);
                }
                else
                { // De otra manera estamos escaneando un vehiculo 
                    EmplacamientoDTO car = new EmplacamientoDTO();
                    car = await car.get(code);
                    if (car != null)
                    {
                        await NavigationService.Instance.NavigateToAsync<CarViewModel>(car);
                        Isbusy = false;
                        Console.WriteLine("Search ...");
                    }
                    else
                    {
                        await NavigationService.Instance.NavigateToAsync<CarNotFoundViewModel>(code);
                        Isbusy = false;
                        Console.WriteLine("Not FOUND ...");
                    }
                }
            }
        }
    }
}
