using System.Collections.ObjectModel;
using System.Windows.Input;
using Cajas.MVVM.Models;

namespace Cajas.MVVM.Pages.cupones;

public partial class CuponesView : ContentPage
{
    public ObservableCollection<CouponItem> Coupons { get; set; }

    public string OperatorName { get; set; } = "Juan Pérez López";

    public string OperatorCredential { get; set; } = "12345";

    public string OperatorStatus { get; set; } = "ACTIVO";

    public int CouponsCount => Coupons.Count;

    public decimal TotalAmount => Coupons.Sum(x => x.Amount);

    public string CurrentUser { get; set; } = "SUPERADMIN";

    public string RegisterNumber { get; set; } = "1";

    public string StatusMessage { get; set; }
        = "Listo para escanear el siguiente cupón";

    public string LastCouponCode =>
        Coupons.LastOrDefault()?.Code ?? string.Empty;

    public ICommand SearchOperatorCommand { get; }

    public ICommand AddCouponCommand { get; }

    public ICommand ChargeCouponsCommand { get; }

    public ICommand ClearCouponsCommand { get; }

    public CuponesView()
    {
        InitializeComponent();

        Coupons = new ObservableCollection<CouponItem>
        {
            new CouponItem
            {
                Number = 1,
                Code = "548789654",
                Amount = 150.00m,
                ScannedDate = new DateTime(2025, 5, 20, 10, 15, 30),
                Status = "VÁLIDO"
            },
            new CouponItem
            {
                Number = 2,
                Code = "548789655",
                Amount = 200.00m,
                ScannedDate = new DateTime(2025, 5, 20, 10, 15, 45),
                Status = "VÁLIDO"
            },
            new CouponItem
            {
                Number = 3,
                Code = "548789656",
                Amount = 100.00m,
                ScannedDate = new DateTime(2025, 5, 20, 10, 16, 2),
                Status = "VÁLIDO"
            },
            new CouponItem
            {
                Number = 4,
                Code = "548789657",
                Amount = 250.00m,
                ScannedDate = new DateTime(2025, 5, 20, 10, 16, 18),
                Status = "VÁLIDO"
            },
            new CouponItem
            {
                Number = 5,
                Code = "548789658",
                Amount = 150.00m,
                ScannedDate = new DateTime(2025, 5, 20, 10, 16, 33),
                Status = "VÁLIDO"
            },
            new CouponItem
            {
                Number = 6,
                Code = "548789659",
                Amount = 100.00m,
                ScannedDate = new DateTime(2025, 5, 20, 10, 16, 50),
                Status = "VÁLIDO"
            },
            new CouponItem
            {
                Number = 7,
                Code = "548789660",
                Amount = 150.00m,
                ScannedDate = new DateTime(2025, 5, 20, 10, 17, 5),
                Status = "VÁLIDO"
            },
            new CouponItem
            {
                Number = 8,
                Code = "548789661",
                Amount = 150.00m,
                ScannedDate = new DateTime(2025, 5, 20, 10, 17, 20),
                Status = "VÁLIDO"
            }
        };

        SearchOperatorCommand = new Command(async () =>
            await SearchOperatorAsync());

        AddCouponCommand = new Command(async () =>
            await AddCouponAsync());

        ChargeCouponsCommand = new Command(async () =>
            await OpenChargeModalAsync());

        ClearCouponsCommand = new Command(async () =>
            await ClearCouponsAsync());

        BindingContext = this;
    }

    private async Task SearchOperatorAsync()
    {
        string operatorId = OperatorEntry.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(operatorId))
        {
            await DisplayAlert(
                "Operador / socio",
                "Ingrese un ID o número de identificación.",
                "Aceptar");

            OperatorEntry.Focus();
            return;
        }

        // Aquí posteriormente llamarás al servicio real:
        // var operatorData =
        //     await _operatorService.GetByIdAsync(operatorId);

        await DisplayAlert(
            "Búsqueda",
            $"Aquí se buscará al operador con identificación {operatorId}.",
            "Aceptar");
    }

    private async Task AddCouponAsync()
    {
        string couponCode = CouponCodeEntry.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(couponCode))
        {
            await DisplayAlert(
                "Cupón",
                "Ingrese o escanee el código del cupón.",
                "Aceptar");

            CouponCodeEntry.Focus();
            return;
        }

        bool alreadyExists = Coupons.Any(x =>
            string.Equals(
                x.Code,
                couponCode,
                StringComparison.OrdinalIgnoreCase));

        if (alreadyExists)
        {
            await DisplayAlert(
                "Cupón duplicado",
                "Este cupón ya fue agregado.",
                "Aceptar");

            CouponCodeEntry.Text = string.Empty;
            CouponCodeEntry.Focus();
            return;
        }

        // Aquí posteriormente consultarás el cupón real:
        // var coupon =
        //     await _couponService.GetByCodeAsync(couponCode);

        Coupons.Add(new CouponItem
        {
            Number = Coupons.Count + 1,
            Code = couponCode,
            Amount = 150.00m,
            ScannedDate = DateTime.Now,
            Status = "VÁLIDO"
        });

        CouponCodeEntry.Text = string.Empty;

        RefreshSummary();

        CouponCodeEntry.Focus();
    }

    private async Task OpenChargeModalAsync()
    {
        if (Coupons.Count == 0)
        {
            await DisplayAlert(
                "Cobrar cupones",
                "No hay cupones agregados para cobrar.",
                "Aceptar");

            CouponCodeEntry.Focus();
            return;
        }

        RefreshSummary();

        ChargeModalOverlay.IsVisible = true;

        ChargeModalOverlay.Opacity = 0;
        ChargeModalCard.Scale = 0.92;

        await Task.WhenAll(
            ChargeModalOverlay.FadeTo(1, 160, Easing.CubicOut),
            ChargeModalCard.ScaleTo(1, 180, Easing.CubicOut));
    }

    private async Task ClearCouponsAsync()
    {
        if (Coupons.Count == 0)
        {
            CouponCodeEntry.Text = string.Empty;
            CouponCodeEntry.Focus();
            return;
        }

        bool confirmed = await DisplayAlert(
            "Limpiar cupones",
            "¿Desea eliminar todos los cupones escaneados?",
            "Limpiar",
            "Cancelar");

        if (!confirmed)
            return;

        Coupons.Clear();

        CouponCodeEntry.Text = string.Empty;

        RefreshSummary();

        CouponCodeEntry.Focus();
    }

    private void RemoveCoupon_Clicked(object sender, EventArgs e)
    {
        if (sender is not ImageButton imageButton)
            return;

        if (imageButton.CommandParameter is not CouponItem coupon)
            return;

        Coupons.Remove(coupon);

        ReorderCoupons();
        RefreshSummary();

        CouponCodeEntry.Focus();
    }

    private void ReorderCoupons()
    {
        for (int i = 0; i < Coupons.Count; i++)
        {
            Coupons[i].Number = i + 1;
        }
    }

    private void RefreshSummary()
    {
        OnPropertyChanged(nameof(CouponsCount));
        OnPropertyChanged(nameof(TotalAmount));
        OnPropertyChanged(nameof(LastCouponCode));
    }

    private async Task CloseChargeModalAsync()
    {
        await Task.WhenAll(
            ChargeModalOverlay.FadeTo(0, 130, Easing.CubicIn),
            ChargeModalCard.ScaleTo(0.94, 130, Easing.CubicIn));

        ChargeModalOverlay.IsVisible = false;

        CouponCodeEntry.Focus();
    }

    private async void CancelChargeModal_Clicked(
    object sender,
    EventArgs e)
    {
        await CloseChargeModalAsync();
    }

    private async void ConfirmCharge_Clicked(
     object sender,
     EventArgs e)
    {
        if (Coupons.Count == 0)
        {
            await CloseChargeModalAsync();
            return;
        }

        Button? confirmButton = sender as Button;

        if (confirmButton is not null)
        {
            confirmButton.IsEnabled = false;
            confirmButton.Text = "PROCESANDO...";
        }

        try
        {
            // Aquí posteriormente irá el cobro real:
            //
            // await _couponService.ChargeAsync(Coupons);
            //
            // Solo debes limpiar la colección cuando
            // el servidor confirme que el cobro fue exitoso.

            await Task.Delay(600);

            Coupons.Clear();

            RefreshSummary();

            CouponCodeEntry.Text = string.Empty;

            StatusMessage = "Cobro realizado correctamente";
            OnPropertyChanged(nameof(StatusMessage));

            await CloseChargeModalAsync();

            await DisplayAlert(
                "Cobro realizado",
                "Los cupones fueron cobrados correctamente.",
                "Aceptar");
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error al cobrar",
                $"No fue posible realizar el cobro: {ex.Message}",
                "Aceptar");
        }
        finally
        {
            if (confirmButton is not null)
            {
                confirmButton.IsEnabled = true;
                confirmButton.Text = "CONFIRMAR COBRO";
            }

            CouponCodeEntry.Focus();
        }
    }
}