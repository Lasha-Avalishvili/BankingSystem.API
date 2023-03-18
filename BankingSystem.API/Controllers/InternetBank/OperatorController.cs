using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Auth;
using BankingSystem.Features.InternetBank.Operator.AddAccountForUser;
using BankingSystem.Features.InternetBank.Operator.AddUser;
using BankingSystem.Features.InternetBank.Operator.AddUserDetails;
using BankingSystem.Features.InternetBank.Operator.AuthOperator;
using BankingSystem.Features.InternetBank.Operator.AuthUser;
using BankingSystem.Features.InternetBank.Operator.RegisterUser;
using IbanNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Validations;

namespace BankingSystem.API.Controllers.InternetBank
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class OperatorController : ControllerBase
    {
        private readonly TokenGenerator _tokenGenerator;
        private readonly IOperatorRepository _operatorRepository;
        private readonly IUserRepository _userRepository;
        private readonly RegisterOperatorService _registerOperatorService;
        private readonly RegisterUserService _registerUserService;
        private readonly AddUserDetailsService _addUserDetailsService;

        public OperatorController(TokenGenerator tokenGenerator, IOperatorRepository operatorRepository, IUserRepository userRepository, RegisterOperatorService registerOperatorService, RegisterUserService registerUserService, AddUserDetailsService addUserDetailsService)
        {

            _operatorRepository = operatorRepository;
            _tokenGenerator = tokenGenerator;
            _userRepository = userRepository;
            _registerOperatorService = registerOperatorService;
            _registerUserService = registerUserService;
            _addUserDetailsService = addUserDetailsService;
        }

        //[HttpPost("register-operator")]
        //public async Task<ActionResult<OperatorEntity>> RegisterOperator([FromBody] OperatorRegisterRequest request)
        //{
        //    var exists = _operatorRepository.OperatorExists(request.PersonalNumber);
        //    if (!exists)
        //    {
        //        var registeredOperator = await _operatorRepository.RegisterOperatorAsync(request);
        //        await _operatorRepository.SaveChangesAsync();

        //        return Ok(registeredOperator);
        //    }
        //    return BadRequest("Operator with this personal number already exsists");
        //}

        [HttpPost("register-operator")]
        public async Task<ActionResult<RegisterOperatorResponse>> RegisterOperator([FromBody] RegisterOperatorRequest request)
        {
            var result =await _registerOperatorService.RegisterOperatorAsync(request);
            return Ok(result);
            // no validation for the same personal number
        }

        [HttpPost("login-operator")]
        public async Task<IActionResult> Login([FromBody] LoginOperatorRequest request)
        {
            var operatorByName = await _operatorRepository.GetOperatorByNameAsync(request);

            if (operatorByName == null)
            {
                return NotFound("Operator not found");
            }

            var operatorByPassword = await _operatorRepository.GetOperatorByPasswordAsync(request);

            if (operatorByPassword == null)
            {
                return BadRequest("invalid name or password");
            }

            return Ok(_tokenGenerator.GenerateForAdmin(operatorByPassword.Id.ToString()));
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpPost("register-user")]
        public async Task<ActionResult<UserEntity>> RegisterUser(RegisterUserRequest request)
        {
            var result = await _registerUserService.RegisterUserAsync(request);
            return Ok(result);
            // no validation for the same personal number
        }

        //[Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        //[HttpPost("add-user-account")]
        //public async Task<ActionResult<AccountEntity>> AddAccount([FromBody] AddAccountRequest request)
        //{
        //    var isExists = _addUserRepository.AccountExists(request.IBAN);
        //    if (isExists == true)
        //    {
        //        return BadRequest("User Account With This IBN Already Exists");
        //    }
        //    IIbanValidator validator = new IbanValidator();
        //    ValidationResult validationResult = validator.Validate(request.IBAN);
        //    if (validationResult.IsValid)
        //    {
        //        var account = await _addUserRepository.AddAccountAsync(request);
        //        await _addUserRepository.SaveChangesAsync();

        //        return Ok(account);
        //    }
        //    return BadRequest("IBAN isn't correct");
        //}
        //[Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
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
