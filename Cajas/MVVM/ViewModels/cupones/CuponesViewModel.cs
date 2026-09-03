using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Cajas.MVVM.Models;
using Cajas.MVVM.Pages;
using Cajas.MVVM.ViewModels;
using Cajas.Services;
using Cajas.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using radiotaxi.Model;
using radiotaxi.Model.v2.finance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZXing;
using ZXing.QrCode.Internal;

namespace Cajas.MVVM.ViewModels.cupones
{
    public partial class CuponesViewModel : BaseViewModel
    {// Control de scaner:
        private readonly ISessionService _sessionService;
        private readonly IPageFactory _pageFactory;
        private bool _scannerActivo;
        public event EventHandler? FocusScannerRequested;
        private int userid = 0;
        // Modelo cupon para busquedas etc.
        private radiotaxi.Model.Cupon _cupon = new radiotaxi.Model.Cupon();

        // Inforamcion del operador o socio que esta realizando la transaccion.
        [ObservableProperty]
        private string _operatorName = "";

        [ObservableProperty]
        private string _message = "";

        [ObservableProperty]
        private string _operatorCredential = "";
        [ObservableProperty]
        private bool _status = true;

        [ObservableProperty]
        public int _couponsCount = 0;

        // Buscador de cupones
        [ObservableProperty]
        private string _couponCode = "";

        [ObservableProperty]
        private ObservableCollection<Cupon> _cupons = new();
        // Aqui son las variables encesarias para hacer el render de la imagen.
        private readonly iAwsS3 _AwsS3 = new AwsS3();
        [ObservableProperty]
        ImageSource _imgs = null;

        [ObservableProperty]
        private bool isLoadingImage;

        [ObservableProperty]
        private string _operatorStatus = "ACTIVO";
        [ObservableProperty]
        public decimal _totalAmount = 0;
        [ObservableProperty]
        public string _currentUser = "";
        [ObservableProperty]
        public string _registerNumber = "1";
        [ObservableProperty]
        public string _statusMessage = "Listo para escanear el siguiente cupón";
        [ObservableProperty]
        public string _lastCouponCode = "";

        [ObservableProperty]
        private bool showChargeModal;



        // Variables de german para lanzar los canvas.
        [ObservableProperty]
        private bool showPaymentsDrawer;
        // Variables de german para lanzar los canvas.
        [ObservableProperty]
        private bool showPaymentDetailDrawer;
        // Variable que contiene la lista de pagos.
        [ObservableProperty]
        private ObservableCollection<CouponPaymentItemViewModel> _payments = new();

        private readonly IQrScannerService _scannerService;
        private readonly ITicketPrinterService _ticketPrinterService;

        private bool _scannerSubscribed;
        [ObservableProperty]
        private string codigoQr = string.Empty;
        [ObservableProperty]
        private string mensaje = "Esperando código QR...";
        [ObservableProperty]
        private bool procesandoQr;

        // ALERTAS

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


        // Obtener Hoteles 
        [ObservableProperty]
        private ObservableCollection<Hotel> hotels = new();
        [ObservableProperty]
        private Hotel? selectedHotel;

        public override Task InitializeAsync(object navigationData)
        {
            if (!_sessionService.IsLoggedIn)
            {
                var login = _pageFactory.Create<LoginPage>();

                Application.Current!.MainPage =
                    new NavigationPage(login);

                return Task.CompletedTask;
            }

            if (navigationData != null)
            {
                // Procesar navigationData
            }

            return base.InitializeAsync(navigationData);
        }

        public CuponesViewModel(
            IQrScannerService scannerService,
            ITicketPrinterService ticketPrinterService, 
            ISessionService sessionService,
            IPageFactory pageFactory
        )
        {
            System.Diagnostics.Debug.WriteLine(
            "CuponesViewModel creado");
            _pageFactory = pageFactory;
            _sessionService = sessionService;
            _scannerService = scannerService;
            _ticketPrinterService = ticketPrinterService;

            // =====================================================
            //  MI MODIFICACION MI INTENDO DE MVVM PARA LO DEL SLIDE
            // Cargamos información DUMMY solamente para probar el front.
            // =====================================================
            //;
            try
            {
                // CARGAMOS LA INFORMACION DE LOS HOTELES PARA ENVIARLOS AL SELECT DE PROVEEDORES/CLIENTES
                Hotel hotel = new Hotel();
                hotel.get();
                List<Hotel> _hotels = hotel.get();
                this.Hotels = new ObservableCollection<Hotel>(_hotels);
                Console.Write("");
            }
            catch (Exception ex)
            {

            }

        }

        private void getImage(radiotaxi.Model.Operator _operator)
        {
            UriImageSource Img = null;
            try
            {
                Thread.Sleep(50);

                

                string uri = _AwsS3.getImage(
                    _operator.bucket,
                    _operator.key);

                if (string.IsNullOrWhiteSpace(uri))
                {
                    this.Imgs = null;
                    return;
                }

                this.Imgs = new UriImageSource
                {
                    Uri = new Uri(uri, UriKind.Absolute),
                    CachingEnabled = true,
                    CacheValidity = TimeSpan.FromHours(1)
                };
                Thread.Sleep(50);
            }
            catch (Exception ex)
            {
                this.Imgs = null;

                System.Diagnostics.Debug.WriteLine(
                    $"Error cargando imagen: {ex}");
            }
            finally
            {
            }
            Thread.Sleep(50);
        }

        private List<radiotaxi.Model.card> getCards(string gafet)
        {
            // Obtenemos todos los tarjetons del operador.
            radiotaxi.Model.card _card = new radiotaxi.Model.card();
            return _card.get(gafet);
        }

        private List<radiotaxi.Model.HistAsistencia> getReports(radiotaxi.Model.Operator _operator)
        {
            // Obtenemos los reportes 
            List<radiotaxi.Model.HistAsistencia> _reportes = _operator.reportes();
            List<radiotaxi.Model.HistAsistencia> reportes = new List<radiotaxi.Model.HistAsistencia>();
            string _color = this.getRandColor();
            string oldticket = "";
            foreach (var reporte in _reportes)
            {
                if (oldticket != reporte.Ticket)
                {
                    _color = this.getRandColor();
                }
                reporte.color = _color;
                reportes.Add(reporte);
                oldticket = reporte.Ticket;
            }
            return reportes;
        }

        private List<radiotaxi.Model.MENSAJE> getMessage(string gafet)
        {
            try
            {
                // Obtenemos los mensajes Bloqueos
                radiotaxi.Model.MENSAJE mENSAJE = new radiotaxi.Model.MENSAJE();
                return mENSAJE.getGafet(gafet);
            }
            catch (Exception ex)
            {
                return new List<radiotaxi.Model.MENSAJE>();
            }
        }

        private List<desbloqueo> getBlock(string gafet)
        {
            try
            {
                // Obtenemos los mensajes Bloqueos
                radiotaxi.Model.MENSAJE mENSAJE = new radiotaxi.Model.MENSAJE();
                return mENSAJE.Desbloqueos(gafet);
            }
            catch (Exception ex)
            {
                return new List<desbloqueo>();
            }
        }

        private financesOperator getFinances(radiotaxi.Model.Operator _operator)
        {
            try
            {
                // Obtener faltas
                return _operator.financesOperator();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<pay> getPays(radiotaxi.Model.Operator _operator)
        {
            try
            {
                // Obtener faltas
                return _operator.payCards();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<Convenio> getAgreements(radiotaxi.Model.Operator _operator)
        {
            try
            {
                List<Convenio> agreements = _operator.agreements();
                return agreements;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<radiotaxi.Model.C_permissionsDTO> permision(string gafet)
        {
            try
            {
                radiotaxi.Model.C_permissions permissions = new radiotaxi.Model.C_permissions();
                return permissions.get(gafet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [RelayCommand]
        private async Task SearchOperatorAsync()
        {
            try
            {

                this.IsLoadingImage = true;

                OperatorName = "";
                OperatorStatus = "";
                Imgs = null;
                userid = 0;

                string reference =
                    OperatorCredential?
                        .Replace(" ", "")
                        .Trim()
                        .ToUpperInvariant()
                    ?? string.Empty;

                if (string.IsNullOrWhiteSpace(reference))
                {
                    await ShowAlertAsync(
                        "Identificación requerida",
                        "Ingrese el ID o número de identificación del operador o socio.",
                        AlertType.Warning);

                    return;
                }


                if (reference.Contains("OP"))
                {
                    radiotaxi.Model.Operator operatorData =
                        new radiotaxi.Model.Operator().get(reference);

                    if (operatorData is not null)
                    {
                        userid = operatorData.userId;

                        OperatorName = string.Join(
                            " ",
                            new[]
                            {
                    operatorData.firstName,
                    operatorData.lastNameF,
                    operatorData.lastNameM
                            }
                            .Where(x => !string.IsNullOrWhiteSpace(x)));

                        OperatorStatus =
                            operatorData.active == true
                                ? "ACTIVO"
                                : "INACTIVO";

                        await Task.Run(() =>
                            getImage(operatorData));
                    }
                }
                else
                {
                    radiotaxi.Model.Partner partnerData =
                        new radiotaxi.Model.Partner().get(reference);

                    if (partnerData is not null)
                    {
                        userid = partnerData.userId;

                        OperatorName = string.Join(
                            " ",
                            new[]
                            {
                    partnerData.firstName,
                    partnerData.lastNameF,
                    partnerData.lastNameM
                            }
                            .Where(x => !string.IsNullOrWhiteSpace(x)));

                        OperatorStatus =
                            partnerData.active == true
                                ? "ACTIVO"
                                : "INACTIVO";
                    }
                }

                if (userid == 0)
                {
                    OperatorName = "";
                    OperatorStatus = "";
                    Imgs = null;

                    await ShowAlertAsync(
                        "Operador/Socio no encontrado",
                        "No se encontró ningún operador o socio con esa identificación.",
                        AlertType.Warning);
                }
            }
            catch (Exception ex)
            {
                OperatorName = "";
                OperatorStatus = "";
                Imgs = null;
                userid = 0;

                await ShowAlertAsync(
                    "Error de búsqueda",
                    $"No fue posible consultar al operador: {ex.Message}",
                    AlertType.Error);
            }
            finally{
                FocusScannerRequested?.Invoke(
                            this,
                            EventArgs.Empty);

                this.IsLoadingImage = false;
            }
        }

        [RelayCommand]
        public async Task AddCouponAsync()
        {
            try
            {
                // AQUI INICIA LA BUSQUEDA DESDE EL FORMULARIO.
                if (selectedHotel != null)
                {
                    // ENVIAMOS COMO PARAMETRO EL HOTEL SELECCIONADO PARA BUSCAR EL CUPON.
                    this.searchCupon(this.CouponCode, selectedHotel.HotelID);
                }
                else
                {
                    this.searchCupon(this.CouponCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // Obtenemos la informacion del valor.

            // validamos el cupon en la base de datos si esta bloqueado o cancelado.
        }

        [RelayCommand]
        private async Task DeleteCuponAsync(Cupon cupon)
        {
            if (cupon == null)
                return;
            Cupons.Remove(cupon);
            this.calculateamount();
        }

        // ESTE METODO LIMPIA TODO.
        [RelayCommand]
        private async Task CleanAsync() {
            this.clean();
        }

        [RelayCommand]
        private async Task PayCuponAsync()
        {
            try
            {
                Cupon _cupon = new Cupon();
                Console.WriteLine("Procedemos al pago.");
                if (userid != 0 && this.Cupons.Count() > 0 && this.TotalAmount > 0) // Si hay algo que pagar... entonces pagalo pues.
                {
                    int? payment_id = _cupon.pay(this.app.USERLOG.firstName + " " + this.app.USERLOG.lastName, Cupons.ToList(), userid, TotalAmount);
                    if (payment_id != null)
                    {
                        await ShowAlertAsync(
                            "Pago realizado",
                            "Los cupones fueron cobrados correctamente.",
                            AlertType.Success);

                        ShowChargeModal = false;
                        // IMPRIME EL TICKET 
                        PrintTicketResult result =  await this.printticket((int)payment_id, "Original");
                        // Limapia todos los valores de los formulario.
                        this.clean();
                        if (result != null && result.Success != true)
                        {
                            // ALERTA::!!! No se pudo cobrar nada...
                            await ShowAlertAsync(
                                "No se pudo imprimir el ticket.",
                                "Ocurrió un problema al imprimir el ticket de pago.",
                                AlertType.Error);
                        }
                    }
                    else
                    {
                        // ALERTA::!!! No se pudo cobrar nada...
                        await ShowAlertAsync(
                            "No fue posible cobrar",
                            "Ocurrió un problema al procesar el pago de los cupones.",
                            AlertType.Error);
                    }
                }
                else
                {
                    if (userid == 0)
                    {
                        // ALERTA:: Enviar un alert de que no existe operador / socio seleccionado
                        await ShowAlertAsync(
                            "Operador requerido",
                            "Seleccione un operador o socio antes de realizar el pago.",
                            AlertType.Warning);

                    }
                    else if (this.Cupons.Count() <= 0)
                    {
                        // ALERTA:: Enviar un alert que no se han escaneado cupones aun...
                        await ShowAlertAsync(
                            "Sin cupones",
                            "No se han escaneado cupones para cobrar.",
                            AlertType.Warning);
                    }
                    else if (this.TotalAmount <= 0)
                    {
                        // ALERTA:: Enviar un alert que no existe monto que pagar...
                        // ALERTA:: Caso: cupones cancelados o pagados escaneados.
                        await ShowAlertAsync(
                        "Monto no válido",
                        "No existe un monto disponible para realizar el pago.",
                        AlertType.Warning);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [RelayCommand]
        private void CloseChargeModal()
        {
            ShowChargeModal = false;
        }

        public void StartScanner()
        {
            if (_scannerActivo)
                return;
            System.Diagnostics.Debug.WriteLine(
                "CuponesViewModel.StartScanner");
            _scannerService.CodeScanned += OnCodeScanned;
            _scannerService.FocuScanned += OnFocuScanner;
            _scannerService.StartListening();
            _scannerActivo = true;
        }

        public void StopScanner()
        {
            if (!_scannerActivo)
                return;
            _scannerService.CodeScanned -= OnCodeScanned;
            _scannerService.FocuScanned -= OnFocuScanner;
            _scannerService.StopListening();
            _scannerActivo = false;
        }

        private void searchCupon(string code, int? hotelId = null, int? cuponType = null)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (code != "")
                    {

                        if (Shell.Current is not null)
                        {
                            Shell.Current.FlyoutIsPresented = false;
                        }
                        code = code.ToUpper().Replace("K", "");
                        this.CouponCode = code;
                        SerchCuponsDTO parameters = new SerchCuponsDTO
                        {
                            hotelid = hotelId ==null? 0 : (int)hotelId,
                            folio = Convert.ToInt32(this.CouponCode),
                            cupontype = cuponType == null ? 0 : (int)cuponType,
                            hc = hotelId == null ? 0 : (int)hotelId
                        };
                        List<Cupon> Rcupons = _cupon.search(parameters);
                        System.Diagnostics.Debug.WriteLine(
                            $"Código recibido: {code}");
                        if (Rcupons != null && Rcupons.Count() > 0 && !Cupons.Any(c => c.CuponsID == Rcupons.FirstOrDefault().CuponsID))
                        {

                            string json = JsonConvert.SerializeObject(
                                Rcupons.First(),
                                Formatting.Indented,
                                new JsonSerializerSettings
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                });

                            // Agregamos esto a la lista.
                            this.Cupons.Add(Rcupons.FirstOrDefault());
                            this.calculateamount();
                        }
                        else
                        {
                            bool alreadyAdded =
                                Rcupons is not null &&
                                Rcupons.Count > 0 &&
                                Cupons.Any(c =>
                                    c.CuponsID ==
                                    Rcupons.First().CuponsID);

                            if (alreadyAdded)
                            {
                                await ShowAlertAsync(
                                    "Cupón duplicado",
                                    "Este cupón ya fue agregado a la lista.",
                                    AlertType.Warning);
                            }
                            else
                            {
                                await ShowAlertAsync(
                                    "Cupón no encontrado",
                                    "El cupón capturado o escaneado no existe.",
                                    AlertType.Error);
                            }

                            CouponCode = "";
                            SelectedHotel = null;
                        }
                    }
                    else
                    {
                        await ShowAlertAsync(
                            "Es necesario ingresar un código",
                            "Por favor ingrese un código de cupón.",
                            AlertType.Warning);
                    }
                });
                // Aquí puedes manejar el código escaneado, por ejemplo, actualizar la propiedad CodigoQr... buscar el cupon.
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        private void OnCodeScanned(
            object? sender,
            string code)
        {
            this.searchCupon(code);
        }

        private void calculateamount()
        {
            decimal tempAmount = 0;
            int count = 0;
            this.CouponsCount = this.Cupons.Count();
            foreach (Cupon _cupon in this.Cupons)
            {
                if (_cupon.active == true &&
                    _cupon.Paid == false &&
                    DateTime.Now < _cupon.CreateDate.AddYears(1)
                    ) // Si esta activo se suma para el pago, de lo contrario no.
                {
                    count += 1;
                    tempAmount += _cupon.Couponrate.Valor;
                }
            }
            this.TotalAmount = tempAmount;
            this.CouponsCount = count;
        }

        private void OnFocuScanner(object? sender,
            string code)
        {
            FocusScannerRequested?.Invoke(this, EventArgs.Empty);
        }

        // BEGIN ::

        private void clean() {
            ///////////////////////////////////////////////////////////

            Cupons = new ObservableCollection<Cupon>();
            OperatorCredential = "";
            OperatorName = "";
            OperatorStatus = "";
            userid = 0;
            CouponCode = "";
            SelectedHotel = null;
            Mensaje = "";
            Message = "";
            CouponsCount = 0;
            TotalAmount = 0;
            Imgs = null;
        }

        private async Task<PrintTicketResult> printticket(int id, string title){
            try
            {
                CouponPayment Payment = new CouponPayment();
                List<CouponPayment> Payments = Payment.getHistory(userid, id);
                // OBTENEMOS TODA LA INFORMACION DEL TICKET.

                if (Payments.Count() > 0)
                {
                    Payment = Payments.First();
                    ///////////////////////////////////////////////////////////
                    // El pago ya fue confirmado y la transacción hizo Commit.
                    var ticket = new CouponPaymentTicket
                    {
                        PaymentId = Payment.CouponPaymentId,
                        PaymentDate = Payment.CreatedDate,
                        CashierName = Payment.CreatedBy,
                        OperatorNumber = Payment.user.partnerReference,
                        OperatorName = Payment.user.firstName + " " + Payment.user.lastNameF + " " + Payment.user.lastNameM,
                        TotalAmount = Payment.TotalAmount,
                        title = title,

                        Coupons = Payment.Cupons
                            .Select(x => new CouponTicketItem
                            {
                                CouponId = x.CuponsID,
                                Code = $"{x.Couponrate.Hotel.shortname}-{x.CuponsID}-{x.Couponrate.Cupontype.Name}",
                                Amount = x.Couponrate?.Valor ?? 0
                            })
                            .ToList()
                    };

                    StatusMessage = "Imprimiendo comprobante...";
                    PrintTicketResult printResult =
                        await _ticketPrinterService
                            .PrintCouponPaymentAsync(ticket);
                    return printResult;
                }
                return null;
            }
            catch (Exception ex)
            {
                await ShowAlertAsync(
                "Impresion de ticket fallo",
                ex.Message,
                AlertType.Info);
                return null;
            }
        }

        [RelayCommand]
        private async Task ReprintPaymentAsync(CouponPaymentItemViewModel Obj)
        {
            if (Obj is null)
            {
                return;
            }
            PrintTicketResult result = await this.printticket(Obj.Payment.CouponPaymentId, "Reimpresion");
            // IMPORTANTE:
            // En esta etapa NO se imprime ningún ticket real.
            // Este mensaje únicamente permite validar el botón del front.
            if (result != null && result.Success == true)
            {
                await ShowAlertAsync(
                    "Reimpresión",
                    $"Se esta reimprimiendo el ticket de pago de cupones con el folio #{Obj.Payment.CouponPaymentId}.",
                    AlertType.Info);
            }
        }
        // END ::


        private PaymentHistoryItem LoadHistory()
        {
            // VALIDAR OPERADOR.
            if (userid != 0)
            {
                // Obtenemos el historico de los pagos... con sus respectivos cupones.
                CouponPayment Payment = new CouponPayment();
                List<CouponPayment> Payments = Payment.getHistory(userid);
                ObservableCollection<CouponPayment> observable = new ObservableCollection<CouponPayment>(Payments);
                PaymentHistoryItem _paymentinfo = new PaymentHistoryItem()
                {
                    OperatorName = OperatorCredential +  " / " + OperatorName,
                    CouponsCount = observable.Count(),
                    TotalAmount = observable.Sum(p => p.TotalAmount),
                    PaymentDate = new DateTime(2026, 8, 7, 14, 32, 0),
                    // ESTO NO SE DEFINE COMO CUPONES, SE DEBE DEFINIR COMO cuponpayments adentro de cuponpayments esta la lista de cupones.
                    payments = observable
                };
                return _paymentinfo;
            }
            return null;
        }


        [RelayCommand]
        private void TogglePayment(CouponPaymentItemViewModel item)
        {
            if (item == null)
                return;

            item.IsExpanded = !item.IsExpanded;
        }


        [RelayCommand]
        private async Task ShowHistory() {
            try
            {
                PaymentHistoryItem response =  this.LoadHistory();
                if (response != null && response.payments != null && response.payments.Count > 0)
                {

                    foreach (CouponPayment item in response.payments)
                    {
                        Payments.Add(new CouponPaymentItemViewModel()
                        {
                            Payment = item,
                            IsExpanded = false
                        });
                    }

                    // Renderziar el canvas.
                    this.ShowPaymentsDrawer = true;
                }
                else {
                    // ALERTA:: No hay historial de pagos para este operador.
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [RelayCommand]
        private async Task HideHistory()
        {
            try
            {
                // Ocultar el canvas de historial de pagos.
                // Renderziar el canvas.
                this.ShowPaymentsDrawer = false;
                Payments = new ObservableCollection<CouponPaymentItemViewModel>();
            }
            catch (Exception)
            {
                throw;
            }
        }

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

        [RelayCommand]
        private async Task PrePayCuponAsync()
        {
            try
            {
                if (Cupons.Count > 0)
                {
                    ShowChargeModal = true;
                    return;
                }

                await ShowAlertAsync(
                    "Sin cupones",
                    "No se han escaneado cupones para cobrar.",
                    AlertType.Warning);
            }
            catch (Exception ex)
            {
                await ShowAlertAsync(
                    "Error",
                    $"No fue posible preparar el pago: {ex.Message}",
                    AlertType.Error);
            }
        }

        public enum AlertType
        {
            Info,
            Success,
            Warning,
            Error
        }
    }
}