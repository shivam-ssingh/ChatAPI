using ChatAPI.Models;

namespace ChatAPI.Data.Interface
{
    public interface IUserService
    {
        Task<bool> CreateUser(User user);
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(string id);
        Task<User> LogInUser(User user);
    }
}
