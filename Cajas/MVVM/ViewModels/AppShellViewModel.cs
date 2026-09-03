using Cajas.MVVM.Pages;
using Cajas.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cajas.MVVM.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        private readonly IPageFactory _pageFactory;

        public AppShellViewModel(IPageFactory pageFactory)
        {
            _pageFactory = pageFactory;
        }

        [RelayCommand]
        private async Task SignOut()
        {
            SecureStorage.Default.Remove(
                "_authentificationToken");
            var loginPage =
                _pageFactory.Create<LoginPage>();
            CurrentApplication.MainPage =
                new NavigationPage(loginPage);
        }
    }
}