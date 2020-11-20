namespace DotvSocketServer.domain
{
    public class Message
    {
        public static int MESSAGE_TYPE_AUTH = 0;
        public static int MESSAGE_TYPE_CHAT = 1;
        public int MessageType { get; set; }
    }
}