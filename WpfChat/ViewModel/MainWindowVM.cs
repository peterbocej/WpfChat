using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Configuration;

namespace WpfChat.ViewModel;

public interface IMainViewModel : IBaseViewModel
{ }
public class MainWindowVM : BaseViewModel, IMainViewModel
{
    private readonly IConfigurationRoot _config;

    public MainWindowVM()
    {
        _config = App.GetRequiredService<IConfigurationRoot>();
    }

    public string Title { get; set; } = "Chat";
    
    public string UserName { get; set; } = default!;
    
    public void Login()
    {
        if (string.IsNullOrWhiteSpace(UserName))
            return;
        Title = $"Chat ({UserName})";
        OnPropertyChanged(nameof(Title));
    }
}
