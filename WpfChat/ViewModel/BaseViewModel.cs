using System.Windows;

using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfChat.ViewModel;

public interface IBaseViewModel : IDisposable
{ }
public class BaseViewModel : ObservableObject, IBaseViewModel
{
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
