using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Auth;
using BankingSystem.Features.InternetBank.Operator.AddAccountForUser;
using BankingSystem.Features.InternetBank.Operator.AddUser;
using BankingSystem.Features.InternetBank.Operator.AddUserDetails;
using BankingSystem.Features.InternetBank.Operator.AuthUser;
using BankingSystem.Features.InternetBank.Operator.RegisterUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers.InternetBank
{
    [Route("/api/[controller]")]
    [ApiController]
    public class OperatorController : ControllerBase
    {
        private readonly RegisterUserService _registerUserService;
        private readonly AddUserDetailsService _addUserDetailsService;
        private readonly AuthService _authService;

        public OperatorController(RegisterUserService registerUserService, AddUserDetailsService addUserDetailsService, AuthService authService)
        {            
            _registerUserService = registerUserService;
            _addUserDetailsService = addUserDetailsService;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginOperator([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
        {
            var result = await _registerUserService.RegisterUserAsync(request);
            return Ok(result);
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpPost("add-user-account")]
        public async Task<IActionResult> AddAccount([FromBody] AddAccountRequest request)
        {
            var result = await _addUserDetailsService.AddAccountAsync(request);
            return Ok(result);
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpPost("add-user-card")]
        public async Task<IActionResult> AddCard([FromBody] AddCardRequest request)
        {
            var result = await _addUserDetailsService.AddCardAsync(request);
            return Ok(result);
        }
    }
}
