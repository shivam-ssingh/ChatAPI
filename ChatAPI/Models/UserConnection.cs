namespace ChatAPI.Models
{
    public class UserConnection
    {
        public string UserName { get; set; }
        public string ChatRoom { get; set; }
        public string PublicKey { get; set; }

        public UserConnection()
        {
            UserName = string.Empty;
            ChatRoom = string.Empty;
            PublicKey = string.Empty;
        }
    }
}
