namespace DotvSocketServer.domain
{
    public class AuthRequest : Message
    {
        public string Token { get; set; }
    }
}