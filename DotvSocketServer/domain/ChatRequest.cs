namespace DotvSocketServer.domain
{
    public class ChatRequest : Message
    {
        public string Token { get; set; }
        public string Message { get; set; }
        public long TimeStamp { get; set; }

        public ChatRequest()
        {
            this.Token = "";
            this.Message = "";
        }
        
    }
}