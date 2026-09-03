using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cajas.MVVM.Models;
using Cajas.MVVM.ViewModels;
using Openpay;

namespace Cajas.MVVM.ViewModels.payments
{
    public class paymentsViewModel : BaseViewModel
    {
        public paymentsViewModel()
        {

        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData != null)
            {
                if (navigationData is EmplacamientoDTO)
                {
                    //this.Car = (EmplacamientoDTO)navigationData;
                }
            }
            return base.InitializeAsync(navigationData);
        }
        private void pay()
        {
            OpenpayAPI openpayAPI = new OpenpayAPI("sk_3433941e467c4875b178ce26348b0fac", "moiep6umtcnanql3jrxp");
            openpayAPI.Production = false; // Default value = false

        }
    }
}
