using System.Timers;
using System.Timers;
using Timer = System.Timers.Timer;
namespace Cajas.MVVM.Pages;

public partial class OnboardingPage : ContentPage
{
    private Timer _timer;
    private int _currentIndex;

    public OnboardingPage()
    {
        InitializeComponent();
        StartAutoPlay();
    }

    private void StartAutoPlay()
    {
        _currentIndex = 0;
        _timer = new Timer(6000); // Set interval to 3000 ms (3 seconds)
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    private void OnTimedEvent(object sender, ElapsedEventArgs e)
    {
        // Switch to the next item
        MainThread.BeginInvokeOnMainThread(() =>
        {
            _currentIndex = (_currentIndex + 1) % OnboardingCarousel.ItemsSource.Cast<string>().Count();
            OnboardingCarousel.Position = _currentIndex;
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _timer?.Stop();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _timer?.Start();
    }
}