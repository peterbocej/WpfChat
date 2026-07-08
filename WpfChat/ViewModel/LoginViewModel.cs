using System.Windows;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

namespace WpfChat.ViewModel;

public interface ILoginViewModel : IBaseViewModel
{ 
    string UserName { get; }
}
public class LoginViewModel : BaseViewModel, ILoginViewModel
{

    public ICommand LoginClickCommand { get; set; }
    public LoginViewModel()
    {
        LoginClickCommand = new AsyncRelayCommand(LoginClick);
    }

    public string UserName
    {
        get { return App.UserName; }
        set
        {
            App.UserName = value;
            OnPropertyChanged(nameof(UserName));
        }
    }
    private async Task LoginClick()
    {
        if (!string.IsNullOrWhiteSpace(UserName))
        {
        Window.DialogResult = true;
        Window.Close();
        }
        else
            MessageBox.Show(Window, "No user name.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}
