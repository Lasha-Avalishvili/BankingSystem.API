using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Auth;
using BankingSystem.Features.InternetBank.Operator.AddAccountForUser;
using BankingSystem.Features.InternetBank.Operator.AddUser;
using BankingSystem.Features.InternetBank.Operator.AddUserDetails;
using BankingSystem.Features.InternetBank.Operator.AuthOperator;
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
        private readonly TokenGenerator _tokenGenerator;
        private readonly IOperatorRepository _operatorRepository;
        private readonly IRegisterUserRepository _registerUserRepository;
        private readonly RegisterOperatorService _registerOperatorService;
        private readonly RegisterUserService _registerUserService;
        private readonly AddUserDetailsService _addUserDetailsService;

        public OperatorController(TokenGenerator tokenGenerator, IOperatorRepository operatorRepository, IRegisterUserRepository userRepository, RegisterOperatorService registerOperatorService, RegisterUserService registerUserService, AddUserDetailsService addUserDetailsService)
        {

            _operatorRepository = operatorRepository;
            _tokenGenerator = tokenGenerator;
            _registerUserRepository = userRepository;
            _registerOperatorService = registerOperatorService;
            _registerUserService = registerUserService;
            _addUserDetailsService = addUserDetailsService;
        }

        [HttpPost("register-operator")]
        public async Task<ActionResult<RegisterOperatorResponse>> RegisterOperator([FromBody] RegisterOperatorRequest request)
        {
            var result =await _registerOperatorService.RegisterOperatorAsync(request);
            return Ok(result);
        }

        [HttpPost("login-operator")]
        public async Task<LoginOperatorResponse> Login([FromBody] LoginOperatorRequest request)
        {
            var operatorByPersonalNumber = await _operatorRepository.GetOperatorByPersonalNumberAsync(request);
            var operatorByPassword = await _operatorRepository.GetOperatorByPasswordAsync(request);
            var response = new LoginOperatorResponse();
            if (operatorByPersonalNumber == null)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Operator Not Found";
                response.JWT = null;
            }
            else if(operatorByPassword == null)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Invalid PersonalNumber or Password";
                response.JWT = null;
            }
            else
            {
                response.IsSuccessful = true;
                response.ErrorMessage = null;
                response.JWT = _tokenGenerator.Generate("api-admin", operatorByPassword.Id.ToString());
            }
            return response;
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpPost("register-user")]
        public async Task<ActionResult<UserEntity>> RegisterUser(RegisterUserRequest request)
        {
            var result = await _registerUserService.RegisterUserAsync(request);
            return Ok(result);
        }

        [HttpPost("add-user-account")]
        public async Task<ActionResult<AccountEntity>> AddAccount([FromBody] AddAccountRequest request)
        {
            var result = await _addUserDetailsService.AddAccountAsync(request);
            return Ok(result);
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpPost("add-user-card")]
        public async Task<ActionResult<CardEntity>> AddCard([FromBody] AddCardRequest request)
        {
            var result = await _addUserDetailsService.AddCardAsync(request);
            return Ok(result);
        }
    }
}
