namespace LetsChat.Response
{
    public class AuthResponseManager
    {
        public string Message { get; set; }
        public bool success { get; set; }
        public IEnumerable<string> Errors { get; set; } 

        public DateTime? Expires { get; set; }
    }
}
