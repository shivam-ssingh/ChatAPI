using MongoDB.Bson.Serialization.Attributes;

namespace ChatAPI.Models
{
    public class UserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
