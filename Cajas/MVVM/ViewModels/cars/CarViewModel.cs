using Cajas.MVVM.Models;
using Cajas.MVVM.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.ViewModels.cars
{
    public partial class CarViewModel : BaseViewModel
    {

        [ObservableProperty]
        EmplacamientoDTO _car = null;

        public CarViewModel()
        {

        }


        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData != null)
            {
                if (navigationData is EmplacamientoDTO)
                {
                    this.Car = (EmplacamientoDTO)navigationData;
                }
            }
            return base.InitializeAsync(navigationData);
        }
    }
}
