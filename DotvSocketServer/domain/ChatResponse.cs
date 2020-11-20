namespace DotvSocketServer.domain
{
    public class ChatResponse : Message
    {
        public string Message { get; set; }
        public string CharacterName { get; set; }
        public string TimeStamp { get; set; }
        public bool Valid { get; set; }

        public ChatResponse()
        {
            MessageType = MESSAGE_TYPE_CHAT;
        }
    }
}