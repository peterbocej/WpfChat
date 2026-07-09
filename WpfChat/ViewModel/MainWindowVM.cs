using System.Collections.ObjectModel;

using Microsoft.Extensions.Configuration;

using WpfChat.Model;
using WpfChat.Repositories;

namespace WpfChat.ViewModel;

public interface IMainViewModel : IBaseViewModel
{ }
public class MainWindowVM : BaseViewModel, IMainViewModel
{
    private readonly IMessagesRepository _messagesRepository;

    public MainWindowVM()
    {
        _messagesRepository = App.GetRequiredService<IMessagesRepository>();
        UserName = Properties.Settings.Default.UserName;
        Refresh();
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
    public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();
    private int _selectedIndex;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            _selectedIndex = value;
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
    // sends new message
    internal void SendMessage()
    {
        if (string.IsNullOrWhiteSpace(MessageText))
            return;

        var msg = new Message
        {
            From = UserName,
            Body = MessageText!,
            Me = 1
        };
        // add to grid
        Messages!.Insert(0, msg);
        SelectedIndex = 0;
        // add to database
        _messagesRepository.Add(msg);
        _messagesRepository.SaveChanges();
        // clear
        MessageText = string.Empty;
    }
    // receive message
    internal void ReceiveMessage(Message message)
    {
        // add to grid
        Messages!.Insert(0, message);
        SelectedIndex = 0;
    }
    private void GetSavedMessages()
    {
        // clear messages
        if (Messages == null)
            Messages = new ObservableCollection<Message>();
        else
            Messages.Clear();
        // load messages to collection
        foreach (var msg in _messagesRepository.GetConversation())
        {
            if (msg.From == UserName)
                msg.Me = 1; // sets message color in grid
            else 
                msg.Me = 0;
            Messages.Add(msg);
        }
        SelectedIndex = 0;
    }
    #endregion
}

