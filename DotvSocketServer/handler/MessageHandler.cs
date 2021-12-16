using System;
using System.Collections.Generic;
using DotvSocketServer.domain;
namespace DotvSocketServer.handler;

public class MessageHandler
{
    protected Dictionary<int, Func<string, MessageResponse>> commands { get; }
        
    public MessageHandler()
    {
        commands = new Dictionary<int, Func<string, MessageResponse>>();
    }
    
    public MessageResponse processMessage(Message message, String content)
    {
        Func<string, MessageResponse> function = commands.GetValueOrDefault(message.MessageType, processBadMessage);
        MessageResponse response = function.Invoke(content);
        return response;
    }

    public MessageResponse processBadMessage(String message)
    {
        //TODO: Log the bad message,
        
        //invalidated message wont have a response
        MessageResponse response = new MessageResponse();
        response.Invalidate();
        return response;
    }

    public bool HasMessageType(int msgType)
    {
        if (commands.ContainsKey(msgType))
            return true;
        
        return false;
    }
}