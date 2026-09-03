

using Cajas.MVVM.ViewModels.cupones;
using Cajas.MVVM.ViewModels.log;

namespace Cajas.MVVM.Pages;

public partial class LoginPage : ContentPage
{
    private readonly LoginPageViewModel _viewModel;
    public LoginPage(LoginPageViewModel viewModel)
	{
		InitializeComponent();
        // Asignacion del ViewModel inyectado
        _viewModel = viewModel;
        // Asignacion del BindingContext
        BindingContext = viewModel;


    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        //imgLoader.IsAnimationPlaying = true;
    }
}