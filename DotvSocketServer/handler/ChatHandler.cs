using DotvSocketServer.domain;
namespace DotvSocketServer.handler;

public class ChatHandler : MessageHandler
{
    public ChatHandler()
    {
        commands.Add(Message.MESSAGE_TYPE_CHAT, ProcessGlobalChatMessage);
        commands.Add(Message.MESSAGE_TYPE_HEARTBEAT, ProcessHeartBeat);
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
        //invalidated message wont have a response
        MessageResponse response = new MessageResponse();
        response.Invalidate();
        return response;
    }
}