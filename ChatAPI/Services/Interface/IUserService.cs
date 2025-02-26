using ChatAPI.Models;

namespace ChatAPI.Services.Interface
{
    public interface IUserService
    {
        Task<UserDetailDTO> CreateUser(RegisterUserDTO registerUserRequest);
        Task<List<User>> GetAllUsers();
        Task<UserDetailDTO> GetUserById(string id);
        Task<UserDetailDTO> LogInUser(LoginUserDTO loginUserRequest);

        Task<UserDetailDTO> HandleGithubCallBack(string code);
    }
}
