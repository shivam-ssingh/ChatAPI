using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatAPI.Models
{
    public class User 
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("Email")]
        public string? Email { get; set; }
        [BsonElement("FirstName")]
        public string? FirstName { get; set; }
        [BsonElement("LastName")]
        public string? LastName { get; set; }
        [BsonElement("PasswordHash")]
        public string? PasswordHash { get; set; }
        [BsonElement("UserName")]
        public string? UserName { get; set; }
    }
}
