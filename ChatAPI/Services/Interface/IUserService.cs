using ChatAPI.Models;

namespace ChatAPI.Services.Interface
{
    public interface IUserService
    {
        Task<User> CreateUser(User user);
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(string id);
        Task<User> LogInUser(User user);
    }
}
