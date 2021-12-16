using DotvSocketServer.domain;

namespace DotvSocketServer.handler;

public class AuthHandler : MessageHandler
{
    public AuthHandler()
    {
        this.commands.Add(Message.MESSAGE_TYPE_AUTH, processAuth);
    }

    public MessageResponse processAuth(string msg)
    {
        AuthResponse response = new AuthResponse();
        response.Valid = true;
        //TODO: actually make sure the user is valid by checking the token
        return response;
    }
    
}