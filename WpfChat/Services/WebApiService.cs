using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using WpfChat.Domain.Model;
using WpfChat.Domain.Settings;

namespace WpfChat.Services;

public interface IWebApiService : IApiService
{ }
public class WebApiService : ApiService, IWebApiService
{
    private readonly HttpClient _httpClient;

    public WebApiService(IConfiguration configuration, ILogger<WebApiService> logger)
        : base(configuration, logger, "WebChat")
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri($"{ChatSettings.Server}:{ChatSettings.Port}")
        };
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "WPFChat");
        _httpClient.Timeout = TimeSpan.FromSeconds(5);
    }

    // GET messages
    public override async Task<IList<Message>> GetMessagesAsync()
    {
        var response = await _httpClient.GetAsync($"{ChatSettings.Path}/list");
        response.EnsureSuccessStatusCode();

        var messages = await response.Content.ReadFromJsonAsync<List<Message>>();
        return messages ?? new List<Message>();
    }

    // GET message by id
    public override async Task<Message?> GetMessageByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{id}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Message>();
    }

    public override async Task ConnectAsync(string username)
    {
        var json = JsonSerializer.Serialize(username);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync($"{ChatSettings.Path}/connect", content);
        if (!response.IsSuccessStatusCode)
            throw new ApplicationException("Error connecting to chat.");
    }

    public override async Task DisconnectAsync(string username)
    {
        var json = JsonSerializer.Serialize(username);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{ChatSettings.Path}/disconnect", content);
        if (!response.IsSuccessStatusCode)
            throw new ApplicationException("Error connecting to chat.");
    }

    public override async Task SendMessageAsync(Message message)
    {
        var json = JsonSerializer.Serialize(message);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{ChatSettings.Path}/send", content);
        if (!response.IsSuccessStatusCode)
            throw new ApplicationException("Error connecting to chat.");
    }

    public override async Task<IList<Message>> CheckNewMessagesAsync(int lastId)
    {
        var response = await _httpClient.GetAsync($"{ChatSettings.Path}/check/{lastId}");
        response.EnsureSuccessStatusCode();

        var messages = await response.Content.ReadFromJsonAsync<List<Message>>();
        return messages ?? new List<Message>();
    }

    public override void Dispose()
    {
        _httpClient.Dispose();
    }
}