namespace DotvSocketServer.domain
{
    public class AuthResponse : MessageResponse
    {
        public AuthResponse()
        {
            MessageType = MESSAGE_TYPE_AUTH;
        }
        
        public bool Valid { get; set; }
    }
}