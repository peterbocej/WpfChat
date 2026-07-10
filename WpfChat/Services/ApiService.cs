using Microsoft.Extensions.Configuration;

using WpfChat.Domain.Model;

namespace WpfChat.Services;

public interface IApiService
{
    Task<IList<Message>> GetSavedMessagesAsync();
}
public class ApiService : IApiService
{
    private readonly IConfiguration _configuration;
    private readonly string _apiUrl;
    public ApiService(IConfiguration configuration)
    {
        _configuration = configuration;
        var chatSettings = new ChatSettings();
        configuration.GetSection("Chat").Bind(chatSettings);
        _apiUrl = $"{chatSettings.Server}:{chatSettings.Port}{chatSettings.Path}";
    }

    public async Task<IList<Message>> GetSavedMessagesAsync()
    {
        throw new NotImplementedException();
    }
    private class ChatSettings
    {
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Path { get; set; } = string.Empty;
    }
}
