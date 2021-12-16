using System;

namespace DotvSocketServer.domain;

public class MessageResponse : Message
{
    public bool SendToRoom { get; set; }
    public bool SendToUser { get; set; }
    public String RoomId { get; set; }
    public bool IsGlobal { get; set; }

    public MessageResponse()
    {
        SendToUser = true;
        SendToRoom = false;
        IsGlobal = false;
    }

    public void Invalidate()
    {
        SendToUser = false;
        SendToRoom = false;
        IsGlobal = false;
    }

}