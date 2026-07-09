using System.Collections.ObjectModel;
using System.Windows.Threading;

using Microsoft.Extensions.Configuration;

using MySqlX.XDevAPI;

using WpfChat.Model;
using WpfChat.Repositories;

using WpfChatApp;

using static System.Net.Mime.MediaTypeNames;

namespace WpfChat.ViewModel;

public interface IMainViewModel : IBaseViewModel
{ }
public class MainWindowVM : BaseViewModel, IMainViewModel
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IChatClientService _chatClient;
    private readonly IConfigurationRoot _configuration;

    public MainWindowVM()
    {
        _messagesRepository = App.GetRequiredService<IMessagesRepository>();
        _chatClient = App.GetRequiredService<IChatClientService>(); 
        _configuration = App.GetRequiredService<IConfigurationRoot>();
        UserName = Properties.Settings.Default.UserName;
        Refresh();
        InitChat();
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
    internal async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(MessageText))
            return;
        // send to server
        await _chatClient.SendPublicMessageAsync(MessageText);
        // clear
        MessageText = string.Empty;
    }

    private void SaveMessage(Message msg)
    {
        // add to database
        _messagesRepository.Add(msg);
        _messagesRepository.SaveChanges();
    }

    // receive message
    internal void ReceiveMessage(string from, string text)
    {
        var msg = new Message
        {
            From = from,
            Body = text,
            Me = (byte)(from == UserName ? 1 : 0)
        };
        ReceiveMessage(msg);
    }
    internal void ReceiveMessage(Message message)
    {
        // add to grid
        Messages!.Insert(0, message);
        SelectedIndex = 0;
        SaveMessage(message);
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

    #region Chat

    private void InitChat()
    {
        _chatClient.SystemMessageReceived += (msg) => ReceiveMessage("[SYSTEM]", msg);
        _chatClient.PublicMessageReceived += (from, msg) => ReceiveMessage(from, msg);
        _chatClient.ErrorReceived += (msg) => ReceiveMessage("[ERROR]", msg);

        var chatSettings = _configuration.GetSection("Chat");
        var chatServer = chatSettings.GetValue<string>("Server");
        Task.Run(async () => await _chatClient.ConnectAsync($"{chatServer}/chat", UserName));
    }

    #endregion

}

