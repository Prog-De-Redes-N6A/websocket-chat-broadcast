using System.Net.WebSockets;
using Common;
using Common.Model;

namespace ClientWS.Services;

public class WebSocketService : IAsyncDisposable
{
    private ClientWebSocket _webSocket = new();

    public async Task ConnectAsync(string uri)
    {
        if (_webSocket.State == WebSocketState.Open)
            return;
        
        try
        {
            await _webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
            Console.WriteLine("Conectado al servidor WebSocket.");
        } catch (Exception ex)
        {
            Console.WriteLine($"Error al conectar al servidor WebSocket: {ex.Message}");
        }
    }
    
    public async Task SendMessageAsync(Message message)
    {
       await MessageHelper.SendMessage(_webSocket, message);
    }
    
    public async Task ReceiveMessageAsync()
    {
        await MessageHelper.ReceiveMessage(_webSocket);
    }

    public void SetProcessMessage(Func<string, Task> processMessage)
    {
        MessageHelper.ProcessMessage = processMessage;
    }
    
    public async ValueTask DisposeAsync()
    {
        if (_webSocket.State == WebSocketState.Open)
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Cerrando", CancellationToken.None);
        }
        _webSocket.Dispose();
        _webSocket = null;
    }
}