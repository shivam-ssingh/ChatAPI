using ChatAPI.Data;
using ChatAPI.Models;
using ChatAPI.Services.Interface;
using MongoDB.Driver;

namespace ChatAPI.Services
{
    public class UserService : IUserService
    {
        private readonly MongoDBService _mongoDBService;
        private const string CollectionName = "users";

        public UserService(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }
        public async Task<User> CreateUser(User user)
        {
            try
            {
                var checkIfEmailPresent = await _mongoDBService.GetDocumentById<User>(CollectionName, "Email", user.Email);
                if (checkIfEmailPresent != null)
                {
                    throw new Exception("Email Already Present");
                }
                //hashing technique before inserting in mongo
                await _mongoDBService.CreateDocument(CollectionName, user);
                return await _mongoDBService.GetDocumentById<User>(CollectionName, "Email", user.Email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> LogInUser(User user)
        {
            try
            {
                var userLoggedIn = await _mongoDBService.GetDocumentById<User>(CollectionName, "Email", user.Email);
                if (userLoggedIn == null)
                {
                    throw new Exception("Email Not Present");
                }
                //check hashed password 
                return userLoggedIn;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _mongoDBService.GetAllDocument<User>(CollectionName);
        }
        public async Task<User> GetUserById(string id)
        {
            return await _mongoDBService.GetDocumentById<User>(CollectionName, "_id", id);
        }
    }
}
