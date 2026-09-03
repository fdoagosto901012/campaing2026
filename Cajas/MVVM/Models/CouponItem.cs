using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cajas.MVVM.Models
{
    public class CouponItem : INotifyPropertyChanged
    {
        private int _number;
        private string _code = string.Empty;
        private decimal _amount;
        private DateTime _scannedDate;
        private string _status = string.Empty;

        public int Number
        {
            get => _number;
            set
            {
                if (_number == value)
                    return;

                _number = value;
                OnPropertyChanged();
            }
        }

        public string Code
        {
            get => _code;
            set
            {
                if (_code == value)
                    return;

                _code = value;
                OnPropertyChanged();
            }
        }

        public decimal Amount
        {
            get => _amount;
            set
            {
                if (_amount == value)
                    return;

                _amount = value;
                OnPropertyChanged();
            }
        }

        public DateTime ScannedDate
        {
            get => _scannedDate;
            set
            {
                if (_scannedDate == value)
                    return;

                _scannedDate = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                if (_status == value)
                    return;

                _status = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}