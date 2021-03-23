using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AvaloniaWebBrowserControl.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        protected virtual void SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null)
        {
            if (Equals(member, val)) return;
            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        //The Dispose should be implemented on each class derived from BindableBase 
        //and release all resources.
        public virtual void Dispose() { }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
