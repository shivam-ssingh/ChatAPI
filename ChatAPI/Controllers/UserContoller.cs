using ChatAPI.Extensions;
using ChatAPI.Models;
using ChatAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService { get; set; }
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBodyAttribute] RegisterUserDTO registerRequest)
        {
            try
            {
                var createdUser = await _userService.CreateUser(registerRequest.ConvertRegisterRequest());
                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                if(ex.Message == "Email Already Present")
                {
                    return BadRequest(ex.Message); 
                }
                //error handling using problem -> https://stackoverflow.com/a/68892997
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBodyAttribute] LoginUserDTO loginRequest)
        {
            try
            {
                var userLogin = await _userService.LogInUser(loginRequest.ConvertLoginRequest());
                return Ok(userLogin);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Email Not Present")
                {
                    return BadRequest(ex.Message);
                }
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
