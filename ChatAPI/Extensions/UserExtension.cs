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
                FirstName = registerUserRequest.FirstName, 
                LastName = registerUserRequest.LastName 
            };
        }

        public static UserDetailDTO ConvertUserToUserDetailDTO(this User user)
        {
            return new UserDetailDTO()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }

        public static User ConvertLoginRequest(this LoginUserDTO loginUserRequest)
        {
            return new User()
            {
                Email = loginUserRequest.Email,
                PasswordHash = loginUserRequest.Password,
            };
        }
    }
}
