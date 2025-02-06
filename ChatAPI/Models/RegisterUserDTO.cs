using System.ComponentModel.DataAnnotations;

namespace ChatAPI.Models
{
    public class RegisterUserDTO
    {
        //[Required(ErrorMessage = "First name is required.")]
        public string? FirstName { get; set; }

        //[Required(ErrorMessage = "Last name is required.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
