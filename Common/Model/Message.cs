using System.Text.Json.Nodes;

namespace Common.Model;

public class Message
{ 
    public MessageType MessageType { get; set; }
    public JsonObject Data { get; set;  }
    
    public Message(MessageType messageType, JsonObject data)
    {
        MessageType = messageType;
        Data = data;
    }

    public Message(JsonObject messageType)
    {
        MessageType = Enum.Parse<MessageType>(messageType["MessageType"]!.ToString()!);
        Data = messageType["Data"]!.AsObject();
    }

    public Message(Message messageType)
    {
        MessageType = messageType.MessageType;
        Data = messageType.Data;
    }

    public Message(string message)
    {
        var jsonObject = JsonNode.Parse(message)!.AsObject();
        MessageType = Enum.Parse<MessageType>(jsonObject["MessageType"]!.ToString()!);
        Data = jsonObject["Data"]!.AsObject();
    }

    public void AddData(string key, JsonNode value)
    {
        Data[key] = value;
    }
    
    public JsonObject GetAsJsonObject()
    {
        var jsonObject = new JsonObject
        {
            ["MessageType"] = MessageType.ToString(),
        };
        // agregar clones de los datos
        var dataObject = new JsonObject();
        foreach (var kvp in Data)
        {
            dataObject[kvp.Key] = kvp.Value?.DeepClone();
        }
        jsonObject["Data"] = dataObject;
        return jsonObject;
    }
}