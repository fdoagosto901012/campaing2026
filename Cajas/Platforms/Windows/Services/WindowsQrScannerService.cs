#if WINDOWS

using Cajas.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using System.Text;
using System.Text.RegularExpressions;
using Windows.System;
using ZXing.QrCode.Internal;
using Application = Microsoft.Maui.Controls.Application;

namespace radiotaxi.Platforms.Services.Windows;

public sealed class WindowsQrScannerService : IQrScannerService
{
    private readonly StringBuilder _buffer = new();
    private FrameworkElement? _rootElement;
    private DateTime _lastKeyTime = DateTime.MinValue;
    private bool _isListening;
    public event EventHandler<string>? CodeScanned;
    public event EventHandler<string>? FocuScanned;

    private bool _scannerSequence;
    private const int ScannerIntervalMilliseconds = 80;
    private bool _focusRequested = false;

    private readonly KeyEventHandler _keyDownHandler;

    public WindowsQrScannerService()
    {
        _keyDownHandler = OnKeyDown;
        System.Diagnostics.Debug.WriteLine(
            "WindowsQrScannerService creado");
        _focusRequested = false;
    }

    public void StartListening()
    {
        System.Diagnostics.Debug.WriteLine(
        "WindowsQrScannerService.StartListening");

        if (_isListening)
            return;

        var window = Application.Current?.Windows.FirstOrDefault();

        if (window?.Handler?.PlatformView
            is not Microsoft.UI.Xaml.Window nativeWindow)
        {
            return;
        }

        _rootElement = nativeWindow.Content as FrameworkElement;

        if (_rootElement is null)
            return;

        /*
         * true significa que recibiremos el evento incluso si otro
         * control ya lo marcó como procesado.
         */
        _rootElement.AddHandler(
            UIElement.PreviewKeyDownEvent,
            _keyDownHandler,
        true);

        _buffer.Clear();
        _isListening = true;
    }

    public void StopListening()
    {
        if (!_isListening || _rootElement is null)
            return;

        _rootElement.RemoveHandler(
        UIElement.PreviewKeyDownEvent,
        _keyDownHandler);

        _buffer.Clear();
        _rootElement = null;
        _isListening = false;
    }

    private void OnKeyDown(
        object sender,
        KeyRoutedEventArgs e)
    {
        var now = DateTime.UtcNow;
        var elapsed = now - _lastKeyTime;

        if (_lastKeyTime != DateTime.MinValue &&
            elapsed.TotalMilliseconds <= ScannerIntervalMilliseconds)
        {
            _scannerSequence = true;
        }
        else
        {
            _scannerSequence = false;
            _buffer.Clear();
        }

        _lastKeyTime = now;

        if (e.Key == VirtualKey.Enter)
        {
            var _code = (_buffer.ToString().Trim()).ToUpper();
            var isCodeValid =
                !string.IsNullOrWhiteSpace(_code) &&
                Regex.IsMatch(
                    _code,
                    @"^K\d+$",
                    RegexOptions.IgnoreCase);

            if (isCodeValid)
            {
                FocusEntry();
                // El Enter no llega a la interfaz.
                e.Handled = true;
                CompleteScan();
                _scannerSequence = false;
                return;
            }

            // Código inválido o Enter vacío:
            // el Enter sigue hacia la interfaz.
            ResetBuffer();
            return;
        }

        var character = ConvertKeyToCharacter(e.Key);
        if (!character.HasValue)
            return;

        _buffer.Append(character.Value);

        var codepattern = _buffer
        .ToString()
        .Trim()
        .ToUpperInvariant();

        // K seguido de uno o más números.. para evitar el focus en otra parte del programa.. si es un patron.
        bool isCuponPath =
            Regex.IsMatch(codepattern, @"^K\d+$");

        if (isCuponPath && !_focusRequested)
        {
            FocusEntry();
            _focusRequested = true;
        }

        if (_scannerSequence)
        {
            e.Handled = true;
        }
    }

    private void CompleteScan()
    {
        var code = _buffer
            .ToString()
            .Trim();
        _buffer.Clear();
        if (string.IsNullOrWhiteSpace(code))
            return;
        CodeScanned?.Invoke(this, code);
    }

    private void ResetBuffer()
    {
        _scannerSequence = false;
        _focusRequested = false;
        _buffer.Clear();
        _lastKeyTime = DateTime.MinValue;
    }

    private void FocusEntry() {
        FocuScanned?.Invoke(this, "");
    }

    private static char? ConvertKeyToCharacter(
        VirtualKey key)
    {
        if (key >= VirtualKey.Number0 &&
            key <= VirtualKey.Number9)
        {
            return (char)(
                '0' +
                (key - VirtualKey.Number0));
        }

        if (key >= VirtualKey.NumberPad0 &&
            key <= VirtualKey.NumberPad9)
        {
            return (char)(
                '0' +
                (key - VirtualKey.NumberPad0));
        }

        if (key >= VirtualKey.A &&
            key <= VirtualKey.Z)
        {
            return (char)(
                'A' +
                (key - VirtualKey.A));
        }

        return key switch
        {
            VirtualKey.Subtract => '-',
            VirtualKey.Space => ' ',
            VirtualKey.Decimal => '.',
            _ => null
        };
    }
}

#endif