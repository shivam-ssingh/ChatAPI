using MongoDB.Bson.Serialization.Attributes;

namespace ChatAPI.Models
{
    public class UserDetailDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
    }
}
