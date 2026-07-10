using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.Extensions.Configuration;

using WpfChat.Domain.Model;
using WpfChat.Domain.Settings;

namespace WpfChat.Services;

public interface IApiService
{
    Task<IList<Message>> GetMessagesAsync();
    Task<Message?> GetMessageAsync(string id);
    Task SendMessageAsync(Message message);
    Task<IList<Message>> CheckMessagesAsync(int lastId);
    Task Connect(string username);
    Task Disconnect(string username);
}
public class ApiService : IApiService
{
    private readonly IConfiguration _configuration;
    private readonly string _apiUrl;
    private readonly ChatSettings _chatSettings = new ChatSettings();
    public ApiService(IConfiguration configuration)
    {
        _configuration = configuration;
        configuration.GetSection("Chat").Bind(_chatSettings);
        _apiUrl = $"{_chatSettings.Server}:{_chatSettings.Port}{_chatSettings.Path}";
    }
    private HttpClient GetHttpClient()
    {
        return new HttpClient();
    }
    public async Task<IList<Message>> GetListMessagesAsync()
    {
        using (HttpClient client = GetHttpClient())
        {
            var response = await client.GetAsync($"{_apiUrl}/list");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<Message>>(json);
            return list ?? new List<Message>();
        }
    }

    public async Task<Message?> GetMessageAsync(string id)
    {
        using (HttpClient client = GetHttpClient())
        {
            var response = await client.GetAsync($"{_apiUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var item = JsonSerializer.Deserialize<Message>(json);
            return item ?? null;
        }
    }

    public async Task SendMessageAsync(Message message)
    {
        using(HttpClient client = GetHttpClient())
        {
            var content = JsonContent.Create(message);
            var response = await client.PostAsync($"{_apiUrl}/send", content);
            response.EnsureSuccessStatusCode();
        }
    }

    public Task<IList<Message>> GetMessagesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IList<Message>> CheckMessagesAsync(int lastId)
    {
        using (HttpClient client = GetHttpClient())
        {
            var response = await client.GetAsync($"{_apiUrl}/check/{lastId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<Message>>(json);
            return list ?? new List<Message>();
        }
    }

    public async Task Connect(string username)
    {
        using (HttpClient client = GetHttpClient())
        {
            var content = JsonContent.Create(username);
            var response = await client.PostAsync($"{_apiUrl}/connect", content);
            response.EnsureSuccessStatusCode();
        }
    }

    public async Task Disconnect(string username)
    {
        using (HttpClient client = GetHttpClient())
        {
            var content = JsonContent.Create(username);
            var response = await client.PostAsync($"{_apiUrl}/disconnect", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
