using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using Serilog;

using WpfChat.Domain.Model;
using WpfChat.Services;

namespace WpfChat.ViewModel;

public interface IMainViewModel : IBaseViewModel
{ }
public class MainWindowVM : BaseViewModel, IMainViewModel
{
    private readonly IApiService _apiService;

    public MainWindowVM()
    {
        _apiService = App.GetRequiredService<IApiService>();
        UserName = Properties.Settings.Default.UserName;

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
        var connected = ChatEnabled ? "Online" : "Offline";
        if (string.IsNullOrWhiteSpace(UserName))
            Title = $"Chat - {connected}";
        else
            Title = $"Chat ({UserName}) - {connected}";
    }

    private async Task GetSavedMessages()
    {
        try
        {
            // clear messages
            if (Messages == null)
                Messages = new ObservableCollection<Message>();
            else
                Messages.Clear();
            // load messages to collection
            foreach (var msg in await _apiService.GetMessagesAsync())
            {
                if (msg.From == UserName)
                    msg.Me = 1; // sets message color in grid
                else
                    msg.Me = 0;
                Messages.Add(msg);
            }
            SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show(Window, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        _apiService.Dispose();
    }

    #endregion

    #region Chat
    internal async Task Connect()
    {
        try
        {
            Cursor = Cursors.Wait;
            await _apiService.ConnectAsync(UserName);
            await GetSavedMessages();
            ChatEnabled = true;
            SetTitle();
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
            await _apiService.DisconnectAsync(UserName);
            await ReceiveMessages();
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
        var message = new Message
        {
            From = UserName,
            Body = MessageText
        };
        // send to server
        await _apiService.SendMessageAsync(message);
        await ReceiveMessages();
        // clear
        MessageText = string.Empty;
    }
    // receive message
    internal async Task ReceiveMessages()
    {
        try
        {
            Cursor = Cursors.Wait;
            var messages = await _apiService.CheckNewMessagesAsync(Messages.Last().MessageId);
            foreach (var message in messages)
                ReceiveMessage(message);
        }
        catch (Exception ex)
        {
            MessageBox.Show(Window, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void ReceiveMessage(Message message)
    {
        try
        {
            // add to grid
            Messages!.Insert(0, message);
            SelectedIndex = 0;
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

    internal async Task Refresh()
    {
        try
        {
            Cursor = Cursors.Wait;
            await GetSavedMessages();
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

