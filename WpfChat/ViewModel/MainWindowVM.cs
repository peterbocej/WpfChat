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
                    Body = "Start chat"
                }]);
        }
        foreach (var msg in Messages.Where(m => m.From == UserName).ToList())
            msg.Me = 1;
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
    public ICollection<Message>? Messages { get; set; }

    private void SetTitle()
    {
        if (string.IsNullOrWhiteSpace(UserName))
            Title = "Chat";
        else
        {
            Title = $"Chat ({UserName})";
        }
        OnPropertyChanged(nameof(Title));
    }

    internal void Connect()
    {
        SetTitle();
    }
}
