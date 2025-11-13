using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Nodes;
using Common.Model;

namespace Common;

public class MessageHelper
{
    public static Func<string, Task> ProcessMessage { get; set; } = async (msg) =>
    {
        Console.WriteLine("Función de procesamiento de mensajes no asignada.");
        await Task.CompletedTask;
    };
    
    public static async Task SendMessage(WebSocket socket, Message e)
    {
        byte[] messageBuffer = Encoding.UTF8.GetBytes(e.GetAsJsonObject().ToJsonString());
        var messageSegment = new ArraySegment<byte>(messageBuffer);
        if (socket.State != WebSocketState.Open) return;
        try{
            await socket.SendAsync(
                messageSegment, // mensaje a enviar
                WebSocketMessageType.Text,
                true, 
                CancellationToken.None 
            );
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine($"Error al enviar mensaje: {ex.Message}");
        }
    }
    
    public static async Task ReceiveMessage(WebSocket webSocket)
    {
        var bufferArr = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(bufferArr), CancellationToken.None);
            if (receiveResult.CloseStatus.HasValue)
                break;

            string message = Encoding.UTF8.GetString(bufferArr, 0, receiveResult.Count);
            Console.WriteLine($"Recibido: {message}");
            await ProcessMessage(message);
        }
    }
}