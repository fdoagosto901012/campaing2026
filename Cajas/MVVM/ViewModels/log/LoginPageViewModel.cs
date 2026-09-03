using Cajas.MVVM.Models;
using Cajas.MVVM.ViewModels;
using Cajas.MVVM.ViewModels.scan;
using Cajas.Services;
using Cajas.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Openpay;
using Openpay.Entities;
using Openpay.Entities.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Cajas.MVVM.Pages;
using Cajas.MVVM.Pages.scan;
using radiotaxi.Model;
using Customer = Openpay.Entities.Customer;
using Charge = Openpay.Entities.Charge;
using Cajas.MVVM.ViewModels.cupones;

namespace Cajas.MVVM.ViewModels.log
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        private readonly ISessionService _sessionService;
        [ObservableProperty]
        string _nickname = "";
        [ObservableProperty]
        string _password = "";
        [ObservableProperty]
        bool isBusy = false;

        [ObservableProperty]
        bool _isfocused = false;
        public ICommand ICommandNavToHomePage { get; set; }
        public ICommand ICommandNavToScan { get; set; }
        public ICommand ICommandNavToCupon { get; set; }

        // =====================================================
        // ALERTAS - LOGIN
        // =====================================================

        [ObservableProperty]
        private bool showAlertModal;

        [ObservableProperty]
        private string alertTitle = string.Empty;

        [ObservableProperty]
        private string alertMessage = string.Empty;

        [ObservableProperty]
        private string alertIcon = "\uf05a";

        [ObservableProperty]
        private Color alertColor = Colors.Gray;

        [ObservableProperty]
        private Color alertBackgroundColor = Colors.White;

        private CancellationTokenSource? _alertCancellationTokenSource;

        // =====================================================
        // FIN ALERTAS - LOGIN
        // =====================================================

        private readonly IPageFactory _pageFactory;

        public LoginPageViewModel(IPageFactory pageFactory, ISessionService sessionService)
        {
            // Resenet
            this.app.TOKEN = "";
            this.app.USERLOG = null;

            _sessionService = sessionService;
            _pageFactory = pageFactory;

            this.Nickname = "superadmin";
            this.Password = "toor2024";
            app.TOKEN = ""; // Inicializamos el token en nulo cuando regrese a la vista login. SI HAY TOKEN ESTA LOGEADO DE LO CONTRARO NO SE ENCUENTRA LOGEADO.
            
            //pay();
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            try
            {
                IsBusy = true;

                // Le damos oportunidad a MAUI/Windows
                // de actualizar visualmente la vista.
                await Task.Yield();

                string nickname = Nickname;
                string password = Password;

                userlog user = new userlog();

                bool loginResponse = await Task.Run(() =>
                {
                    return user.log(
                        nickname,
                        password);
                });

                if (!loginResponse)
                {
                    await ShowAlertAsync(
                        "Acceso denegado",
                        "Usuario o contraseña incorrectos.",
                        AlertType.Error);

                    return;
                }

                // Compatibilidad con tu código anterior.
                app.ROLE = user.Role;
                app.USERLOG = user;

                // SessionService singleton.
                _sessionService.Login(user);

                if (_sessionService.Role == "Admin")
                {
                    var appShell =
                        _pageFactory.Create<AppShell>();

                    CurrentApplication.MainPage =
                        appShell;

                    return;
                }

                // Aquí irían otros roles.
            }
            catch (Exception ex)
            {
                await ShowAlertAsync(
                    "Error de inicio de sesión",
                    ex.Message,
                    AlertType.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }


        [RelayCommand]
        private async Task NavToScan()
        {
            await NavigationService.Instance.NavigateToAsync<ScanViewModel>();
            }
        [RelayCommand]
        private async Task NavToCupon()
        {
            IsBusy = true;
            await NavigationService.Instance.NavigateToAsync<CuponesViewModel>();
            IsBusy = false;
        }


        // =====================================================
        // ALERTAS - LOGIN
        // =====================================================

        [RelayCommand]
        private void CloseAlert()
        {
            _alertCancellationTokenSource?.Cancel();
            ShowAlertModal = false;
        }

        private async Task ShowAlertAsync(
            string title,
            string message,
            AlertType type,
            int autoCloseMilliseconds = 3000)
        {
            _alertCancellationTokenSource?.Cancel();
            _alertCancellationTokenSource?.Dispose();

            var cancellationTokenSource =
                new CancellationTokenSource();

            _alertCancellationTokenSource =
                cancellationTokenSource;

            AlertTitle = title;
            AlertMessage = message;

            switch (type)
            {
                case AlertType.Success:
                    AlertIcon = "\uf058";
                    AlertColor = Color.FromArgb("#176B32");
                    AlertBackgroundColor = Color.FromArgb("#EAF5E9");
                    break;

                case AlertType.Warning:
                    AlertIcon = "\uf071";
                    AlertColor = Color.FromArgb("#D99000");
                    AlertBackgroundColor = Color.FromArgb("#FFF5DE");
                    break;

                case AlertType.Error:
                    AlertIcon = "\uf057";
                    AlertColor = Color.FromArgb("#E53935");
                    AlertBackgroundColor = Color.FromArgb("#FDECEC");
                    break;

                default:
                    AlertIcon = "\uf05a";
                    AlertColor = Color.FromArgb("#1976D2");
                    AlertBackgroundColor = Color.FromArgb("#E7F2FC");
                    break;
            }
            ShowAlertModal = true;
            try
            {
                await Task.Delay(
                    autoCloseMilliseconds,
                    cancellationTokenSource.Token);
                if (_alertCancellationTokenSource ==
                    cancellationTokenSource)
                {
                    ShowAlertModal = false;
                }
            }
            catch (TaskCanceledException)
            {
                // Se cerró manualmente o se mostró otra alerta.
            }
            finally
            {
                cancellationTokenSource.Dispose();
                if (_alertCancellationTokenSource ==
                    cancellationTokenSource)
                {
                    _alertCancellationTokenSource = null;
                }
            }
        }

        public enum AlertType
        {
            Info,
            Success,
            Warning,
            Error
        }

        // =====================================================
        // FIN ALERTAS - LOGIN
        // =====================================================
    }
}
