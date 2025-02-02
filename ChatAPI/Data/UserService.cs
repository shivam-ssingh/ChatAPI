using ChatAPI.Data.Interface;
using ChatAPI.Models;
using MongoDB.Driver;

namespace ChatAPI.Data
{
    public class UserService : IUserService
    {
        private readonly MongoDBService _mongoDBService;
        private const string CollectionName = "users";

        public UserService(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }
        public async Task<bool> CreateUser(User user)
        {
            await _mongoDBService.CreateDocument(CollectionName, user);
            return true;
        }

        public async Task<User> LogInUser(User user)
        {
            var userLoggedIn = await _mongoDBService.GetDocumentById<User>(CollectionName, "Email", user.Email);
            if (userLoggedIn != null) 
            {
                return userLoggedIn;
            }
            return new User();
        }

        public async Task<List<User>> GetAllUsers()
        {
           return await _mongoDBService.GetAllDocument<User>(CollectionName);
        }
        public async Task<User> GetUserById(string id)
        {
           return await _mongoDBService.GetDocumentById<User>(CollectionName, "_id" , id);
        }
    }
}
