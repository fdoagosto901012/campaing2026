using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cajas.MVVM.Pages;
using Cajas.MVVM.Pages.blocks;
using Cajas.MVVM.Pages.Cards;
using Cajas.MVVM.Pages.cars;
using Cajas.MVVM.Pages.convs;
using Cajas.MVVM.Pages.cupones;
using Cajas.MVVM.Pages.debs;
using Cajas.MVVM.Pages.scan;
using Cajas.MVVM.ViewModels;
using Cajas.MVVM.ViewModels.blocks;
using Cajas.MVVM.ViewModels.cards;
using Cajas.MVVM.ViewModels.cars;
using Cajas.MVVM.ViewModels.convs;
using Cajas.MVVM.ViewModels.cupones;
using Cajas.MVVM.ViewModels.debs;
using Cajas.MVVM.ViewModels.log;
using Cajas.MVVM.ViewModels.scan;

namespace Cajas.Services
{
    public class NavigationService
    {
        protected readonly Dictionary<Type, Type> _mappings;

        static NavigationService _instance;

        public NavigationService()
        {
            _mappings = new Dictionary<Type, Type>();

            CreatePageViewModelMappings();
        }

        public static NavigationService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NavigationService();

                return _instance;
            }
        }

        protected Application CurrentApplication
        {
            get { return Application.Current; }
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : BaseViewModel
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToAsync(Type viewModelType)
        {
            return InternalNavigateToAsync(viewModelType, null);
        }

        public void NavigateToAsyncShell(Shell shell)
        {
            try
            {
                CurrentApplication.MainPage = shell;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Task NavigateToAsync(Type viewModelType, object parameter)
        {
            return InternalNavigateToAsync(viewModelType, parameter);
        }

        public async Task NavigateBackAsync()
        {
            await CurrentApplication.MainPage.Navigation.PopAsync();
        }

        protected Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!_mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }

            return _mappings[viewModelType];
        }

        protected Page CreateAndBindPage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            }

            Page page = Activator.CreateInstance(pageType) as Page;

            return page;
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreateAndBindPage(viewModelType, parameter);

            if (page is LoginPage) // Es el login pues mandala al mainpage.
            {
                CurrentApplication.MainPage = new NavigationPage(page);
            }
            else
            {
                if (CurrentApplication.MainPage is NavigationPage navigationPage)
                {
                    await navigationPage.PushAsync(page);
                }
            }
            await (page.BindingContext as BaseViewModel).InitializeAsync(parameter);
        }

        void CreatePageViewModelMappings()
        {

            // ViewModel / View
            _mappings.Add(typeof(OnboardingPageViewModel), typeof(OnboardingPage));
            _mappings.Add(typeof(LoginPageViewModel), typeof(LoginPage));

            _mappings.Add(typeof(BlocksViewModel), typeof(Blocks));
            _mappings.Add(typeof(ConvsViewModel), typeof(Convs));
            _mappings.Add(typeof(DebsViewModel), typeof(Debs));

            _mappings.Add(typeof(HomeViewModel), typeof(Home));
            _mappings.Add(typeof(CarViewModel), typeof(Car));
            _mappings.Add(typeof(CarNotFoundViewModel), typeof(CarNotFound));
            _mappings.Add(typeof(ScanViewModel), typeof(Scan));
            _mappings.Add(typeof(CardViewModel), typeof(Card));
            
            // CUPONES FAINE... 
            _mappings.Add(typeof(CuponesViewModel), typeof(CuponesView));
        }

    }
}
