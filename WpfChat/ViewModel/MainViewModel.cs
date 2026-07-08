using System.Windows;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Configuration;

using WpfChat.Windows;

namespace WpfChat.ViewModel;

public interface IMainViewModel : IBaseViewModel
{ }
public class MainViewModel : BaseViewModel, IMainViewModel
{
    private readonly IConfigurationRoot _config;

    public MainViewModel()
    {
        _config = App.GetRequiredService<IConfigurationRoot>();
    }

    public string Title { get; set; } = "Chat";

    public string UserName { get; private set; } = default!;

}
