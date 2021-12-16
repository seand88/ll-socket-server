using DotvSocketServer.domain;

namespace DotvSocketServer.handler;

public class ChatHandler : MessageHandler
{
    public ChatHandler()
    {
        this.commands.Add(Message.MESSAGE_TYPE_CHAT, ProcessGlobalChatMessage);
        this.commands.Add(Message.MESSAGE_TYPE_HEARTBEAT, ProcessHeartBeat);
    }
    
    private Message ProcessGlobalChatMessage(string msgContent)
    {
        return new ChatResponse();
    }

    /**
     * Just used to keep the connection alive
     */
    private Message ProcessHeartBeat(string msgContent)
    {
        return new Message();
    }
}