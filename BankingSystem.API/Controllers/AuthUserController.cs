using BankingSystem.DB.Entities;
using BankingSystem.DB;
using BankingSystem.Features.InternetBank.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BankingSystem.Features.InternetBank.Operator.RegisterUser;
using Microsoft.EntityFrameworkCore;
using BankingSystem.Features.InternetBank.Operator.AuthUser;

namespace BankingSystem.Features.InternetBank.Operator.AddUser
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthUserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public AuthUserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpPost("register-user")]
        public async Task<ActionResult<UserEntity>> RegisterUser(UserRegisterRequest request)
        {
            var user = await _userRepository.RegisterUserAsync(request);
            await _userRepository.SaveChangesAsync();

            return Ok(request);
        }
        
        [HttpPost("login-user")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginRequest request)
        {
           var response = await _userRepository.LoginUserAsync(request);

            return Ok(response);
        }
    }
}
