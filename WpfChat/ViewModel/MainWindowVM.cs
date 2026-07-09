using System.Collections.ObjectModel;

using Microsoft.Extensions.Configuration;

using WpfChat.Model;
using WpfChat.Repositories;

namespace WpfChat.ViewModel;

public interface IMainViewModel : IBaseViewModel
{ }
public class MainWindowVM : BaseViewModel, IMainViewModel
{
    private readonly IConfigurationRoot _config;
    private readonly IMessagesRepository _messagesRepository;

    public MainWindowVM()
    {
        _config = App.GetRequiredService<IConfigurationRoot>();
        _messagesRepository = App.GetRequiredService<IMessagesRepository>();
        UserName = Properties.Settings.Default.UserName;
        Task.Run(async () => await GetSavedMessages());
    }

    private async Task GetSavedMessages()
    {
        if (Messages == null)
        {
            Messages = new ObservableCollection<Message>();
            foreach (var msg in await _messagesRepository.GetConversationAsync())
            {
                if (msg.From == UserName)
                    msg.Me = 1;
                Messages.Add(msg);
            }
            SelectedMessage = Messages.Last();
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
    public ICollection<Message>? Messages { get; set; }
    public Message? SelectedMessage { get; set; }

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

    internal void Refresh()
    {
        SetTitle();
    }
}
