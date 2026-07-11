using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

using Serilog;

using WpfChat.Domain.Model;
using WpfChat.Domain.Settings;
using WpfChat.Services;

namespace WpfChat.ViewModel;

public interface IMainViewModel : IBaseViewModel
{ }
public partial class MainWindowVM : BaseViewModel, IMainViewModel
{
    private readonly IApiService _apiService;
    private readonly ChatSettings _chatSettings;
    private CancellationTokenSource? _cancellationTokenSource;

    public MainWindowVM()
    {
        _chatSettings = App.GetRequiredService<ChatSettings>();
        _apiService = App.GetApiService(_chatSettings.ApiServiceType)
            ?? throw new ApplicationException("Api service not fount");
        UserName = Properties.Settings.Default.UserName;
    }

    #region Commands

    [RelayCommand]
    private async Task ConnectAsync()
    {
        try
        {
            Cursor = Cursors.Wait;
            StatusText = "Connecting...";
            await _apiService.ConnectAsync(UserName);
            await GetSavedMessages();
            ChatEnabled = true;
            SetTitle();
            await StartTimerAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(Window, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Log.Error(ex.Message);
        }
        Cursor = Cursors.Arrow;
        StatusText = "Ready";
    }
    [RelayCommand]
    public async Task DisconnectAsync()
    {
        try
        {
            Cursor = Cursors.Wait;
            StatusText = "Disconnecting...";
            await _apiService.DisconnectAsync(UserName);
            await ReceiveMessages();
            ChatEnabled = false;
            StopTimer();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Log.Error(ex.Message);
        }
        Cursor = Cursors.Arrow;
        StatusText = "Ready";
    }
    [RelayCommand]
    private async Task SendAsync()
    {
        if (string.IsNullOrWhiteSpace(MessageText))
            return;
        var message = new Message
        {
            From = UserName,
            Body = MessageText
        };
        try
        {
            StatusText = "Sending...";
            // send to server
            await _apiService.SendMessageAsync(message);
            // clear
            MessageText = string.Empty;
            MessageFocus = true;
            await ReceiveMessages();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Log.Error(ex.Message);
        }
        Cursor = Cursors.Arrow;
        StatusText = "Ready";
    }
    [RelayCommand]
    private async Task RefreshAsync()
    {
        try
        {
            Cursor = Cursors.Wait;
            StatusText = "Refreshing...";
            await GetSavedMessages();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Log.Error(ex.Message);
        }
        Cursor = Cursors.Arrow;
        StatusText = "Ready";
    }
    [RelayCommand]
    private async Task ClosedAsync()
    {
        Dispose();
    }
    [RelayCommand]
    private async Task LoadedAsync()
    {
        UserNameFocus = true;
    }
    #endregion

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
            OnPropertyChanged(nameof(ChatDisabled));
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
    private bool _userNameFocus;
    public bool UserNameFocus
    {
        get { return _userNameFocus; }
        set
        {
            _userNameFocus = value;
            OnPropertyChanged(nameof(UserNameFocus));
        }
    }
    private bool _messageFocus;
    public bool MessageFocus
    {
        get { return _messageFocus; }
        set
        {
            _messageFocus = value;
            OnPropertyChanged(nameof(MessageFocus));
        }
    }

    private string _statusText = "Ready";
    public string StatusText
    {
        get => _statusText;
        set
        {
            _statusText = value;
            OnPropertyChanged(nameof(StatusText));
        }
    }

    #endregion

    #region Functions

    private async Task StartTimerAsync()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(_apiService.ChatSettings.RefreshIntervalSec));
        try
        {
            while (await timer.WaitForNextTickAsync(_cancellationTokenSource.Token))
            {
                await ReceiveMessages();
            }
        }
        catch (OperationCanceledException)
        {
            // end timer
        }
    }
    private void StopTimer()
    {
        _cancellationTokenSource?.Cancel();
    }
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
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public override async void Dispose()
    {
        base.Dispose();
        Properties.Settings.Default.Save();
        if (_cancellationTokenSource != null)
            _cancellationTokenSource.Cancel();
        if (_apiService.State == ApiState.Connected)
            await _apiService.DisconnectAsync(UserName);
        _apiService.Dispose();
    }

    #endregion

    #region Chat
    private async Task ReceiveMessages()
    {
        try
        {
            Cursor = Cursors.AppStarting;
            StatusText = "Communicating...";
            var messages = await _apiService.CheckNewMessagesAsync(Messages.Max(m => m.MessageId));
            foreach (var message in messages)
                ReceiveMessage(message);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        Cursor = Cursors.Arrow;
        StatusText = "Ready";
    }
    private void ReceiveMessage(Message message)
    {
        try
        {
            message.Me = (byte)(message.From == UserName ? 1 : 0);
            // add to grid
            Messages!.Insert(0, message);
            SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Log.Error(ex.Message);
        }
        finally
        {
            Cursor = Cursors.Arrow;
        }
    }

    #endregion

}

