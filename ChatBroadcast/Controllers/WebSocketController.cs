using System.Net.WebSockets;
using System.Text;
using Common;
using Common.Model;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

public class WebSocketController : ControllerBase
{
    static List<WebSocket> _sockets = new List<WebSocket>();
    [Route("/ws")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            _sockets.Add(webSocket);
            await HandleConnection(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
    
    private static async Task HandleConnection(WebSocket webSocket)
    {
        MessageHelper.ProcessMessage = ProcessMessage;
        var receiveTask = ReceiveMessage(webSocket);

        await receiveTask;
    }
    
    private static async Task ReceiveMessage(WebSocket webSocket)
    {
        await MessageHelper.ReceiveMessage(webSocket);
    }

    private static async Task ProcessMessage(string msg)
    {
        Message message = new Message(msg);
        switch (message.MessageType)
        {
            case MessageType.SendMessage: await SendMessageBroadcast(new Message(message));
                break;
            case MessageType.ReceiveMessage:
                break;
            default: Console.WriteLine("Tipo de mensaje no reconocido");
                break;
        }
    }
    
    private static async Task SendMessageBroadcast(Message message)
    {
        foreach (var socket in _sockets)
        {
            Console.WriteLine("Enviando mensaje a un cliente...");
            await MessageHelper.SendMessage(socket, new Message(message));
        }
    }
}