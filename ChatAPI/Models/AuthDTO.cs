namespace ChatAPI.Models
{
    public class AuthDTO 
    {
        public string AuthToken { get; set; }
        public UserDetailDTO UserDetails { get; set; }
    }
}
