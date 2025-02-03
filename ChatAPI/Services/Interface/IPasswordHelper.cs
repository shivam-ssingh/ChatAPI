using ChatAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatAPI.Services.Interface
{
    public interface IPasswordHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string password);
    }
}
