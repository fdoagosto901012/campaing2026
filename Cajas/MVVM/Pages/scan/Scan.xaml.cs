using Cajas.MVVM.ViewModels.scan;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace Cajas.MVVM.Pages.scan;

public partial class Scan : ContentPage
{

    public bool canScan { get; set; }
    private ScanViewModel ViewModel
    {
        get { return BindingContext as ScanViewModel; }
        set { BindingContext = value; }
    }
    public Scan()
	{
		InitializeComponent();
        barcodeView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            Multiple = true
        };
    }

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (this.ViewModel.Isbusy == false && canScan == true)
        {
            canScan = false;
            foreach (var barcode in e.Results)
                Console.WriteLine($"Barcodes: {barcode.Format} -> {barcode.Value}");
            var first = e.Results?.FirstOrDefault();
            if (first is not null)
            {
                Dispatcher.Dispatch(() =>
                {
                    // Update BarcodeGeneratorView
                    barcodeGenerator.ClearValue(BarcodeGeneratorView.ValueProperty);
                    barcodeGenerator.Format = first.Format;
                    barcodeGenerator.Value = first.Value;
                    // Update Label
                    //ResultLabel.Text = $"Barcodes: {first.Format} -> {first.Value}";
                    // Send to the viewModel
                    try
                    {
                        this.ViewModel.Search(first.Value);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                });
            }
        }
    }

    void SwitchCameraButton_Clicked(object sender, EventArgs e)
    {
        barcodeView.CameraLocation = barcodeView.CameraLocation == CameraLocation.Rear ? CameraLocation.Front : CameraLocation.Rear;
    }

    void TorchButton_Clicked(object sender, EventArgs e)
    {
        barcodeView.IsTorchOn = !barcodeView.IsTorchOn;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        imgLoader.IsAnimationPlaying = true;
        canScan = true;
    }
}