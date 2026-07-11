using System;
using System.Collections.Generic;
using System.Text;

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
    protected readonly IConfiguration _configuration;
    protected readonly ILogger _logger;
    public ApiService(IConfiguration configuration, ILogger logger, string chatConfigSection)
    {
        _configuration = configuration;
        _logger = logger;
        ChatSettings = new ChatSettings();
        _configuration.GetSection(chatConfigSection).Bind(ChatSettings);
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
