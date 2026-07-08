using System.Collections.ObjectModel;

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
        Users = new ObservableCollection<string>();
    }

    public string Title { get; set; } = "Chat";
    private string _username = string.Empty;
    public string UserName 
    { 
        get
        {
            return _username;
        }
        set
        {
            _username = value;
            OnPropertyChanged(nameof(UserName));
            SetTitle();
        }
    }
    private string _friendName = string.Empty;
    public string FriendName 
    { 
        get
        {
            return _friendName;
        }
        set
        {
            _friendName = value;
            OnPropertyChanged(nameof(FriendName));
            SetTitle();
        }
    }
    public ICollection<string> Users { get; set; }

    private void SetTitle()
    {
        if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(FriendName))
            Title = "Chat";
        else
        {
            Title = $"Chat ({UserName} <=> {FriendName})";
            OnPropertyChanged(nameof(Title));
        }
    }

    internal void Connect()
    {
        SetTitle();
    }
}
