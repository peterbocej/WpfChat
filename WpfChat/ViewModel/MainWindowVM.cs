using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using Microsoft.Extensions.Configuration;

using MySqlX.XDevAPI;

using Serilog;

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
    private IChatClientService _chatClient = null!;
    private readonly IConfigurationRoot _configuration;

    public MainWindowVM()
    {
        _messagesRepository = App.GetRequiredService<IMessagesRepository>();
        _configuration = App.GetRequiredService<IConfigurationRoot>();
        UserName = Properties.Settings.Default.UserName;
        Connect();
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
    private bool _chatEnabled = false;
    public bool ChatEnabled
    {
        get => _chatEnabled;
        set
        {
            _chatEnabled = value;
            OnPropertyChanged(nameof(ChatEnabled));
        }
    }
    public bool ChatDisabled { get => !ChatEnabled; }
    private Cursor _cursor = Cursors.Arrow;
    public Cursor Cursor
    {
        get => _cursor;
        set
        {
            _cursor = value;
            OnPropertyChanged(nameof(Cursor));
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

    private void SaveMessage(Message msg)
    {
        // add to database
        _messagesRepository.Add(msg);
        _messagesRepository.SaveChanges();
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
    internal void Connect()
    {
        try
        {
            Cursor = Cursors.Wait;
            SetTitle();
            GetSavedMessages();

            _chatClient = App.GetRequiredService<IChatClientService>();
            _chatClient.PublicMessageReceived += (from, msg) => ReceiveMessage(from, msg);
            _chatClient.SystemMessageReceived += (msg) => ReceiveMessage("[SYSTEM]", msg);
            _chatClient.ErrorReceived += (msg) => ReceiveMessage("[ERROR]", msg);

            var chatSettings = _configuration.GetSection("Chat");
            var chatServer = chatSettings.GetValue<string>("Server");
            _chatClient.ConnectAsync($"{chatServer}/chat", UserName)
                .GetAwaiter()
                .GetResult();

            ChatEnabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(Window, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Log.Error(ex.Message);
        }
        finally
        {
            Cursor = Cursors.Arrow;
        }
    }
    internal async Task Disconnect()
    {
        try
        {
            Cursor = Cursors.Wait;
            await _chatClient.DisconnectAsync();
            _chatClient.Dispose();
            ChatEnabled = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show(Window, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Log.Error(ex.Message);
        }
        finally
        {
            Cursor = Cursors.Arrow;
        }
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
        try
        {
            Cursor = Cursors.Wait;
            // add to grid
            Messages!.Insert(0, message);
            SelectedIndex = 0;
            SaveMessage(message);
        }
        catch (Exception ex)
        {
            MessageBox.Show(Window, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Log.Error(ex.Message);
        }
        finally
        {
            Cursor = Cursors.Arrow;
        }
    }

    #endregion

}

