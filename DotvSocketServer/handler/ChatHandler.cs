using DotvSocketServer.domain;

namespace DotvSocketServer.handler;

public class ChatHandler : MessageHandler
{
    public ChatHandler()
    {
        this.commands.Add(Message.MESSAGE_TYPE_CHAT, ProcessGlobalChatMessage);
        this.commands.Add(Message.MESSAGE_TYPE_HEARTBEAT, ProcessHeartBeat);
    }
    
    private MessageResponse ProcessGlobalChatMessage(string msgContent)
    {
        return new ChatResponse();
    }

    /**
     * Just used to keep the connection alive
     */
    private MessageResponse ProcessHeartBeat(string msgContent)
    {
        return new MessageResponse();
    }
}