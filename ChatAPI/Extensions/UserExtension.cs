using ChatAPI.Models;

namespace ChatAPI.Extensions
{
    public static class UserExtension
    {
        public static User ConvertRegisterRequest(this RegisterUserDTO registerUserRequest)
        {
            return new User() 
            { 
                Email = registerUserRequest.Email, 
                Password = registerUserRequest.Password,
                FirstName = registerUserRequest.FirstName, 
                LastName = registerUserRequest.LastName 
            };
        }

        public static User ConvertLoginRequest(this LoginUserDTO loginUserRequest)
        {
            return new User()
            {
                Email = loginUserRequest.Email,
                Password = loginUserRequest.Password,
            };
        }
    }
}
