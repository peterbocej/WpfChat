using System.Collections.ObjectModel;

using Microsoft.Extensions.Configuration;

using WpfChat.Model;

namespace WpfChat.ViewModel;

public interface IMainViewModel : IBaseViewModel
{ }
public class MainWindowVM : BaseViewModel, IMainViewModel
{
    private readonly IConfigurationRoot _config;

    public MainWindowVM()
    {
        _config = App.GetRequiredService<IConfigurationRoot>();
        UserName = Properties.Settings.Default.UserName;
        FriendName = Properties.Settings.Default.FriendName;
        Task.Run(async () => await GetSavedMessages());
    }

    private async Task GetSavedMessages()
    {
        if (Messages == null)
        {
            Messages = new ObservableCollection<Message>(
                [
                    new Message()
                {
                    MessageId = 1,
                    From = UserName,
                    To = FriendName,
                    Body = $"Start chat at {DateTime.Now}"
                }]);
            foreach (var msg in Messages.Where(m => m.From == UserName).ToList())
                msg.Me = 1;
        }
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
            Properties.Settings.Default.UserName = _username;
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
            Properties.Settings.Default.FriendName = _friendName;
        }
    }
    public ICollection<Message>? Messages { get; set; }

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
