﻿using ChatAPI.Extensions;
using ChatAPI.Models;
using ChatAPI.Options;
using ChatAPI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

//using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChatAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService { get; set; }
        private readonly JWTOptions _jwtOptions;
        public UserController(IUserService userService, IOptions<JWTOptions> jwtOptions)
        {
            _userService = userService;
            _jwtOptions = jwtOptions.Value;
        }


        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBodyAttribute] RegisterUserDTO registerRequest)
        {
            try
            {
                var createdUser = await _userService.CreateUser(registerRequest);
                //create JWT
                var token = CreateToken(createdUser);
                return Ok(new AuthDTO() { AuthToken = token});
            }
            catch (Exception ex)
            {
                //if (ex.Message == "Email Already Present")
                //{
                //    //TODO: Use middleware as mentioned here https://learn.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-8.0&viewFallbackFrom=aspnetcore-3.0
                //    return BadRequest(new { error = ex.Message });
                //}
                ////error handling using problem -> https://stackoverflow.com/a/68892997
                //return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
                if (ex.Message == "Email Not Present")
                {
                    return BadRequest(new { Error = ex.Message });
                }
                return UnprocessableEntity(new { Error = "Some Error Occured", Exception = ex.Message });
            }
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBodyAttribute] LoginUserDTO loginRequest)
        {
            try
            {
                var userLogin = await _userService.LogInUser(loginRequest);
                var token = CreateToken(userLogin);
                return Ok(new AuthDTO() { AuthToken = token });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Email Not Present")
                {
                    return BadRequest(new { Error = ex.Message });
                }
                if (ex.Message == "Incorrect Password")
                {
                    return BadRequest(new { Error = ex.Message });

                }
                return UnprocessableEntity(new { Error = "Some Error Occured", Exception = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var userDetails = await _userService.GetUserById(id);
                return Ok(userDetails);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Email Not Present")
                {
                    return BadRequest(new { Error = ex.Message });
                }
                return UnprocessableEntity(new { Error = "Some Error Occured", Exception = ex.Message });
            }
        }
        private string CreateToken(UserDetailDTO user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

            var credentials  = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(
                    [
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),

                    ]),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
                SigningCredentials = credentials,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
            };

            var handler = new JsonWebTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return token;
        }
    }
}
