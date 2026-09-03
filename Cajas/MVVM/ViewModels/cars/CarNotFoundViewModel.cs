using Cajas.MVVM.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.ViewModels.cars
{
    public partial class CarNotFoundViewModel : BaseViewModel
    {
        [ObservableProperty]
        string _eco = "";

        public CarNotFoundViewModel()
        {
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData != null)
            {
                Eco = (string)navigationData;
            }
            return base.InitializeAsync(navigationData);
        }
    }
}