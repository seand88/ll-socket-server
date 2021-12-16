using System;

namespace DotvSocketServer.domain;

public class MessageResponse : Message
{
    public bool SendToRoom { get; }
    public bool SendToUser { get; }
    public String RoomId { get; set; }
    public bool IsGlobal { get; set; }

    public MessageResponse()
    {
        SendToUser = true;
        SendToRoom = false;
        IsGlobal = false;
    }

}