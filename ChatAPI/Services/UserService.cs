using ChatAPI.Data;
using ChatAPI.Extensions;
using ChatAPI.Models;
using ChatAPI.Options;
using ChatAPI.Services.Interface;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ChatAPI.Services
{
    public class UserService : IUserService
    {
        private readonly MongoDBService _mongoDBService;
        private readonly IPasswordHelper _passwordHelper;
        private const string CollectionName = "users";
        private readonly GithubAuthOptions _githubAuthOptions;
        private readonly HttpClient _httpClient;

        public UserService(MongoDBService mongoDBService, IPasswordHelper passwordHelper,
                           IOptions<GithubAuthOptions> githubAuthOptions, IHttpClientFactory httpClientFactory)
        {
            _mongoDBService = mongoDBService;
            _passwordHelper = passwordHelper;
            _githubAuthOptions = githubAuthOptions.Value;
            _httpClient = httpClientFactory.CreateClient();
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

        public async Task<UserDetailDTO> HandleGithubCallBack(string code)
        {
            try
            {
                var clientId = _githubAuthOptions.ClientId;
                var clientSecret = _githubAuthOptions.ClientSecret;

                var tokenResponse = await _httpClient.PostAsync("https://github.com/login/oauth/access_token",
                    new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                {"client_id", clientId},
                {"client_secret", clientSecret},
                {"code", code}
                    }));

                tokenResponse.EnsureSuccessStatusCode();
                var content = await tokenResponse.Content.ReadAsStringAsync();
                var accessToken = content.Split('&')[0].Split('=')[1];

                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.UserAgent.ParseAdd("ChatApp");

                var userResponse = await _httpClient.SendAsync(request);
                if (userResponse.IsSuccessStatusCode)
                {
                    var userContent = await userResponse.Content.ReadAsStringAsync();
                    var githubResponse = JsonSerializer.Deserialize<GithubResponse>(userContent);
                    if (githubResponse.email is null)
                    {
                        throw new Exception("GitHub did not return an email.");
                    }
                    var newUser = new User()
                    {
                        Email = githubResponse.email,
                        UserName = githubResponse.login,
                        FirstName = githubResponse.name
                    };

                    var userWithEmail = await _mongoDBService.GetDocumentById<User>(CollectionName, "Email", newUser.Email);
                    if (userWithEmail == null)
                    {
                        await _mongoDBService.CreateDocument(CollectionName, newUser);
                        var savedUser = await _mongoDBService.GetDocumentById<User>(CollectionName, "Email", newUser.Email);
                        return  savedUser.ConvertUserToUserDetailDTO();
                    }
                    else
                    {
                        return userWithEmail.ConvertUserToUserDetailDTO();
                    }
                }
                throw new Exception();

                //// TODO: Decide if we want to have email as mandatory since github email may be empty
                //if (email == null)
                //    throw new Exception("GitHub did not return an email.");
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                throw;
            }
            
        }

    }
}
