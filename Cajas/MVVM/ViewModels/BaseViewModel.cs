using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Cajas.Client;

namespace Cajas.MVVM.ViewModels
{
    public class BaseViewModel : ObservableObject
    {

        string privateKey = "sk_e319cc73f5b94667a0d50194102c4af5";
        string mechandID = "mxq5mero3jq4xj38q1fd";

        protected App app { get; set; }
        public BaseViewModel()
        {
            app = App.CurrentApp;
        }

        protected Application CurrentApplication
        {
            get { return Application.Current; }
        }
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string? propertyName = null, Action? onChanged = default)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }
            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected string getRandColor()
        {
            Random rnd = new Random();
            string hexOutput = String.Format("{0:X}", rnd.Next(0, 0xFFFFFF));
            while (hexOutput.Length < 6)
                hexOutput = "0" + hexOutput;
            return "#" + hexOutput;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged = null;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

        #endregion
    }
}
