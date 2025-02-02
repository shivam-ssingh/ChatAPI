using ChatAPI.Data.Interface;
using ChatAPI.Models;
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
        public async Task RegisterUser([FromBodyAttribute] User user)
        {
            await _userService.CreateUser(user);
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBodyAttribute] User user)
        {
           var userLogin = await _userService.LogInUser(user);
            return Ok(userLogin);
        }
    }
}
