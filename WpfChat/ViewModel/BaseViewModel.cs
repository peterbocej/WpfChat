using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WpfChat.ViewModel;
public interface IBaseViewModel : IDisposable, INotifyPropertyChanged
{ }
public class BaseViewModel : IBaseViewModel
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null) 
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Dispose()
    {
    }
}
