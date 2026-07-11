using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

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
}

public class ApiService : IApiService
{
    private readonly IConfiguration _configuration;
    private readonly ChatSettings _chatSettings;
    private readonly ILogger<ApiService> _logger;
    private readonly HttpClient _httpClient;

    public ApiService(IConfiguration configuration, ILogger<ApiService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _chatSettings = new ChatSettings();
        _configuration.GetSection("Chat").Bind(_chatSettings);

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri($"{_chatSettings.Server}:{_chatSettings.Port}")
        };
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "WPFChat");
        _httpClient.Timeout = TimeSpan.FromSeconds(5);
    }

    // GET messages
    public async Task<IList<Message>> GetMessagesAsync()
    {
        var response = await _httpClient.GetAsync($"{_chatSettings.Path}/list");
        response.EnsureSuccessStatusCode();

        var messages = await response.Content.ReadFromJsonAsync<List<Message>>();
        return messages ?? new List<Message>();
    }

    // GET message by id
    public async Task<Message?> GetMessageByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{id}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Message>();
    }

    public async Task ConnectAsync(string username)
    {
        var json = JsonSerializer.Serialize(username);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync($"{_chatSettings.Path}/connect", content);
        if (!response.IsSuccessStatusCode)
            throw new ApplicationException("Error connecting to chat.");
    }

    public async Task DisconnectAsync(string username)
    {
        var json = JsonSerializer.Serialize(username);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_chatSettings.Path}/disconnect", content);
        if (!response.IsSuccessStatusCode)
            throw new ApplicationException("Error connecting to chat.");
    }

    public async Task SendMessageAsync(Message message)
    {
        var json = JsonSerializer.Serialize(message);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_chatSettings.Path}/send", content);
        if (!response.IsSuccessStatusCode)
            throw new ApplicationException("Error connecting to chat.");
    }

    public async Task<IList<Message>> CheckNewMessagesAsync(int lastId)
    {
        var response = await _httpClient.GetAsync($"{_chatSettings.Path}/check/{lastId}");
        response.EnsureSuccessStatusCode();

        var messages = await response.Content.ReadFromJsonAsync<List<Message>>();
        return messages ?? new List<Message>();
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}