using Cajas.MVVM.Models;
using Cajas.MVVM.ViewModels.cupones;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Cajas.MVVM.Pages.cupones;

public partial class CuponesView : ContentPage
{
    private readonly CuponesViewModel _viewModel;

    public CuponesView(CuponesViewModel viewModel)
    {
        InitializeComponent();
        System.Diagnostics.Debug.WriteLine(
            "CuponesPage creada mediante DI");
        // Asignacion del ViewModel inyectado
        _viewModel = viewModel;
        // Asignacion de eventos del ViewModel
        _viewModel.FocusScannerRequested += OnFocusScannerRequested;
        // Asignacion del BindingContext
        BindingContext = viewModel;
    }

    // Ciclos de vida de las vistas MAUI !! ESTUDIAR!!!
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.InitializeAsync(null);

        System.Diagnostics.Debug.WriteLine(
            "CuponesPage.OnAppearing");
        // Iniciamos el escáner cuando la vista aparece !! ARRANQUE DE LOGICA EN VIEWMODEL!!
        _viewModel.StartScanner();
        // NO ES necesario llamar esta funcon en CuponesPage_Loaded por que ya se llama aqui.
        FocusScanner();
    }

    protected override void OnDisappearing()
    {
        _viewModel.StopScanner();
        base.OnDisappearing();
    }

    // Este metodo enfocha el entry Ancla para que el escáner pueda escribir en el, SOLO ENFOCA ... NO CONTIENE LOGICA DE ESCANEO.
    private void FocusScanner()
    {
        try
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                CoupontTextInsertEntry.Focus();
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Error al enfocar el escáner: {ex}");
        }
    }

    // Evento disparado desde el ViewModel para solicitar el enfoque del escáner, SOLO ENFOCA ... NO CONTIENE LOGICA DE ESCANEO
    private void OnFocusScannerRequested(
        object? sender,
        EventArgs e)
    {
        Dispatcher.DispatchDelayed(
            TimeSpan.FromMilliseconds(350),
            FocusScanner);
    }
}
