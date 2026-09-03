using Cajas.MVVM.Pages;
using Cajas.MVVM.Pages.cupones;
using Cajas.MVVM.ViewModels;
using Cajas.MVVM.ViewModels.cupones;
using Cajas.MVVM.ViewModels.log;
using Cajas.Platforms.Windows;
using Cajas.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
#endif


namespace Cajas
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    // Font Awesome
                    fonts.AddFont("fa-solid-900.ttf", "FASolid");
                    fonts.AddFont("fa-regular-400.ttf", "FARegular");
                    fonts.AddFont("fa-brands-400.ttf", "FABrands");
                }).ConfigureLifecycleEvents(events =>
                {   
                    #if WINDOWS
                    events.AddWindows(wndLifeCycleBuilder =>
                    {
                        wndLifeCycleBuilder.OnWindowCreated(window =>
                        {
                            IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                            Microsoft.UI.WindowId win32WindowsId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                            Microsoft.UI.Windowing.AppWindow winuiAppWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(win32WindowsId);
                            if (winuiAppWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter p)
                            {
                                p.Maximize();
                                //p.IsResizable = false;
                                //p.IsMaximizable = false;
                                //p.IsMinimizable = false;
                            }
                        });
                    });
                    #endif
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

#if WINDOWS

            builder.Services.AddSingleton<
                IQrScannerService,
                radiotaxi.Platforms.Services.Windows.WindowsQrScannerService>();
            builder.Services.AddSingleton<ISessionService, SessionService>();
            builder.Services.AddSingleton<ITicketPrinterService>(
                _ => new WindowsRawPrinterService());
#endif
            builder.Services.AddTransient<CuponesViewModel>();
            builder.Services.AddTransient<CuponesView>();

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginPageViewModel>();

            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<AppShellViewModel>();

            builder.Services.AddSingleton<IPageFactory, PageFactory>();

            return builder.Build();
        }




    }
}
