using System.Net;
using Business.Services;
using Business.Services.Token;
using Dto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Note.Controllers
{
    public class AuthenticationController:BaseApi
    {
        private readonly ITokenHandler _tokenHandler;
        private readonly IUserService _userService;

        public AuthenticationController(ITokenHandler tokenHandler,IUserService userService)
        {
            _tokenHandler = tokenHandler;
            _userService = userService;
        }
        
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(AuthModel user)
        {
            var loginUser = await _userService.GetUser(user);

            if (loginUser != null){
                var token = _tokenHandler.CreateToken(loginUser);
                return Ok(token);
            }
            return StatusCode((int)HttpStatusCode.Unauthorized);


        }
        [AllowAnonymous]
        [HttpPost("SingUp")]
        public async Task<IActionResult> SingUp(SingUpModel user)
        {
            
            if (user.UserName != null && user.Password != null){
                await _userService.SingUpUser(user);
                return Ok();
            }
            return StatusCode((int)HttpStatusCode.BadRequest);


        }

            
    }
}