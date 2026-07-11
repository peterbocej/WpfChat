using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using WpfChat.Domain.Model;
using WpfChat.Domain.Settings;

namespace WpfChat.Services;

public interface IApiService : IDisposable
{
    Task<Message?> GetMessageByIdAsync(int id);
    Task<IList<Message>> GetMessagesAsync();
    Task ConnectAsync(string username);
    Task DisconnectAsync(string username);
    Task SendMessageAsync(Message message);
    Task<IList<Message>> CheckNewMessagesAsync(int lastId);
    ChatSettings ChatSettings { get; }
}
public abstract class ApiService : IApiService
{
    protected readonly ILogger _logger;
    public ApiService(ChatSettings chatSettings, ILogger logger)
    {
        ChatSettings = chatSettings;
        _logger = logger;
    }

    public abstract Task<Message?> GetMessageByIdAsync(int id);
    public abstract Task<IList<Message>> GetMessagesAsync();
    public abstract Task ConnectAsync(string username);
    public abstract Task DisconnectAsync(string username);
    public abstract Task SendMessageAsync(Message message);
    public abstract Task<IList<Message>> CheckNewMessagesAsync(int lastId);
    public virtual ChatSettings ChatSettings { get; }
    public abstract void Dispose();
}
