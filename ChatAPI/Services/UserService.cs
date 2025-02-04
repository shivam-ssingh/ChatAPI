using ChatAPI.Data;
using ChatAPI.Extensions;
using ChatAPI.Models;
using ChatAPI.Options;
using ChatAPI.Services.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatAPI.Services
{
    public class UserService : IUserService
    {
        private readonly MongoDBService _mongoDBService;
        private readonly IPasswordHelper _passwordHelper;
        private const string CollectionName = "users";

        public UserService(MongoDBService mongoDBService, IPasswordHelper passwordHelper)
        {
            _mongoDBService = mongoDBService;
            _passwordHelper = passwordHelper;
        }
        public async Task<UserDetailDTO> CreateUser(RegisterUserDTO registerUserRequest)
        {
            try
            {
                var user = registerUserRequest.ConvertRegisterRequest();
                var checkIfEmailPresent = await _mongoDBService.GetDocumentById<User>(CollectionName, "Email", registerUserRequest.Email);
                if (checkIfEmailPresent != null)
                {
                    throw new Exception("Email Already Present");
                }
                //hashing technique before inserting in mongo
                user.PasswordHash = _passwordHelper.HashPasswordV2(registerUserRequest.Password);
                await _mongoDBService.CreateDocument(CollectionName, user);
                var savedUser = await _mongoDBService.GetDocumentById<User>(CollectionName, "Email", user.Email);
                return savedUser.ConvertUserToUserDetailDTO();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDetailDTO> LogInUser(LoginUserDTO loginUserRequest)
        {
            try
            {
                var userToLogin = await _mongoDBService.GetDocumentById<User>(CollectionName, "Email", loginUserRequest.Email);
                if (userToLogin == null)
                {
                    throw new Exception("Email Not Present");
                }
                //check hashed password 
                if (!_passwordHelper.VerifyPasswordV2(loginUserRequest.Password, userToLogin.PasswordHash))
                {
                    throw new Exception("Incorrect Password");
                }
                return userToLogin.ConvertUserToUserDetailDTO();
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
        public async Task<UserDetailDTO> GetUserById(string id)
        {
            var user =  await _mongoDBService.GetDocumentById<User>(CollectionName, "_id", id);
            return user.ConvertUserToUserDetailDTO();
        }
    }
}
