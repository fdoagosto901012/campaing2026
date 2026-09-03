using Cajas.MVVM.ViewModels;
using Cajas.MVVM.ViewModels.log;

namespace Cajas.MVVM.Pages;

public partial class AppShell : Shell
{
    private readonly AppShellViewModel _viewModel;

    public AppShell(AppShellViewModel viewModel)
	{
		InitializeComponent();
        // Asignacion del ViewModel inyectado
        _viewModel = viewModel;
        // Asignacion del BindingContext
        BindingContext = viewModel;
    }
}