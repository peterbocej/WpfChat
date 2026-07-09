using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

using Microsoft.Extensions.Configuration;

namespace WpfChatApp
{
    public interface IChatClientService
    {
        bool IsConnected { get; }

        event Action<string>? ErrorReceived;
        event Action<string, string>? PrivateMessageReceived;
        event Action<string, string>? PublicMessageReceived;
        event Action<string>? SystemMessageReceived;

        Task ConnectAsync(string uri, string username);
        Task DisconnectAsync();
        void Dispose();
        Task SendPublicMessageAsync(string message);
    }

    public class ChatClientService : IChatClientService
    {
        private readonly ClientWebSocket _ws = new();
        private readonly Dispatcher _dispatcher;
        private readonly IConfigurationRoot _configuration;
        private readonly CancellationTokenSource _cts = new();

        public bool IsConnected => _ws.State == WebSocketState.Open;

        // Events for UI binding
        public event Action<string>? SystemMessageReceived;
        public event Action<string, string>? PublicMessageReceived;
        public event Action<string, string>? PrivateMessageReceived;
        public event Action<string>? ErrorReceived;

        public ChatClientService(IConfigurationRoot configuration)
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _configuration = configuration;
        }

        public async Task ConnectAsync(string uri, string username)
        {
            if (IsConnected) return;

            await _ws.ConnectAsync(new Uri(uri), CancellationToken.None);

            // Send username as first message
            await SendTextAsync(username);

            // Start receiving messages
            _ = Task.Run(ReceiveLoop);
        }

        public async Task SendPublicMessageAsync(string message)
        {
            if (!IsConnected) return;
            await SendTextAsync(message);
        }

        private async Task SendTextAsync(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            await _ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task ReceiveLoop()
        {
            var buffer = new byte[1024 * 4];
            try
            {
                while (_ws.State == WebSocketState.Open)
                {
                    var result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), _cts.Token);
                    if (result.MessageType == WebSocketMessageType.Close) break;

                    var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    HandleIncomingMessage(json);
                }
            }
            catch (Exception ex)
            {
                RaiseOnUI(() => ErrorReceived?.Invoke($"Connection error: {ex.Message}"));
            }
        }

        private void HandleIncomingMessage(string json)
        {
            try
            {
                var doc = JsonDocument.Parse(json);
                var type = doc.RootElement.GetProperty("type").GetString();

                switch (type)
                {
                    case "system":
                        RaiseOnUI(() => SystemMessageReceived?.Invoke(doc.RootElement.GetProperty("message").GetString() ?? ""));
                        break;
                    case "chat":
                        RaiseOnUI(() => PublicMessageReceived?.Invoke(
                            doc.RootElement.GetProperty("from").GetString() ?? "",
                            doc.RootElement.GetProperty("message").GetString() ?? ""));
                        break;
                    case "private":
                        if (doc.RootElement.TryGetProperty("from", out var from))
                        {
                            RaiseOnUI(() => PrivateMessageReceived?.Invoke(
                                from.GetString() ?? "",
                                doc.RootElement.GetProperty("message").GetString() ?? ""));
                        }
                        else
                        {
                            RaiseOnUI(() => PrivateMessageReceived?.Invoke(
                                $"(to {doc.RootElement.GetProperty("to").GetString()})",
                                doc.RootElement.GetProperty("message").GetString() ?? ""));
                        }
                        break;
                    case "error":
                        RaiseOnUI(() => ErrorReceived?.Invoke(doc.RootElement.GetProperty("message").GetString() ?? ""));
                        break;
                }
            }
            catch
            {
                RaiseOnUI(() => ErrorReceived?.Invoke($"Invalid message format: {json}"));
            }
        }

        private void RaiseOnUI(Action action)
        {
            _dispatcher.Invoke(action);
        }

        public async Task DisconnectAsync()
        {
            if (IsConnected)
            {
                _cts.Cancel();
                await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closing", CancellationToken.None);
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _ws.Dispose();
        }
    }
}
