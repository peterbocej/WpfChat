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
        GetSavedMessages();
    }
    #region Properties

    private string _title = "Chat";
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
        }
    }
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
    private Message? _selectedMessage;

    public Message? SelectedMessage
    {
        get => _selectedMessage;
        set
        {
            _selectedMessage = value;
            OnPropertyChanged(nameof(SelectedMessage));
        }
    }
    private string? _messageText;

    public string? MessageText
    {
        get => _messageText;
        set
        {
            _messageText = value;
            OnPropertyChanged(nameof(MessageText));
        }
    }
    #endregion
    
    #region Functions

    private void SetTitle()
    {
        if (string.IsNullOrWhiteSpace(UserName))
            Title = "Chat";
        else
            Title = $"Chat ({UserName})";
    }

    internal void Refresh()
    {
        SetTitle();
        GetSavedMessages();
    }

    internal void SendMessage()
    {
        var msg = new Message
        {
            From = UserName,
            Body = MessageText!,
            Me = 1
        };
        // add to database
        _messagesRepository.Add(msg);
        _messagesRepository.SaveChanges();
        // add to grid
        Messages!.Add(msg);
        SelectedMessage = msg;
        // clear
        MessageText = string.Empty;

    }
    private void GetSavedMessages()
    {
        if (Messages == null)
        {
            Messages = new ObservableCollection<Message>();
            foreach (var msg in _messagesRepository.GetConversation())
            {
                if (msg.From == UserName)
                    msg.Me = 1;
                Messages.Add(msg);
            }
            SelectedMessage = Messages.Last();
        }
    }
    #endregion
}

