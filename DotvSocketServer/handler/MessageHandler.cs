using System;
using System.Collections.Generic;
using DotvSocketServer.domain;
namespace DotvSocketServer.handler;

public class MessageHandler
{
    protected Dictionary<int, Func<string, Message>> commands { get; }
        
    public MessageHandler()
    {
        commands = new Dictionary<int, Func<string, Message>>();
    }
    
    public Message processMessage(Message message, String content)
    {
        Func<string, Message> function = commands.GetValueOrDefault(message.MessageType, processBadMessage);
        Message response = function.Invoke(content);
        return response;
    }

    public Message processBadMessage(String message)
    {
        //happens if message type not found, should log bad messages for security
        return new Message();
    }

    public bool HasMessageType(int msgType)
    {
        if (commands.ContainsKey(msgType))
            return true;
        
        return false;
    }
}