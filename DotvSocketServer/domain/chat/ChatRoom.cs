using System.Collections.Generic;

namespace DotvSocketServer.domain.chat;

public class ChatRoom
{
    private List<User> usersInRoom;

    public ChatRoom()
    {
        usersInRoom = new List<User>();
    }
}