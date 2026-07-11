using Microsoft.Extensions.Logging;

using WpfChat.Domain.Model;
using WpfChat.Domain.Settings;

namespace WpfChat.Services;
public interface IWinSockApiService : IApiService
{ }
public class WinSockApiService : ApiService, IWinSockApiService
{
    public WinSockApiService(ChatSettings chatSettings, ILogger logger) : base(chatSettings, logger)
    {
    }

    public override async Task<IList<Message>> CheckNewMessagesAsync(int lastId)
    {
        throw new NotImplementedException();
    }

    public override async Task ConnectAsync(string username)
    {
        throw new NotImplementedException();
    }

    public override async Task DisconnectAsync(string username)
    {
        throw new NotImplementedException();
    }

    public override async void Dispose()
    {
        throw new NotImplementedException();
    }

    public override async Task<Message?> GetMessageByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public override async Task<IList<Message>> GetMessagesAsync()
    {
        throw new NotImplementedException();
    }

    public override async Task SendMessageAsync(Message message)
    {
        throw new NotImplementedException();
    }
}
