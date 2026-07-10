using System.ComponentModel;
using System.Windows;

namespace WpfChat.ViewModel;

public interface IBaseViewModel : IDisposable, INotifyPropertyChanged
{ }
public class BaseViewModel : IBaseViewModel
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private Window _window = default!;
    protected virtual Window Window
    {
        get
        {
            if (_window == null)
                // get current window
                foreach (Window win in Application.Current.Windows)
                    if (win.DataContext == this)
                    {
                        _window = win;
                        break;
                    }
            return _window ?? throw new ApplicationException("Error identifying window.");
        }
    }
    public virtual void Dispose()
    {
    }
}
