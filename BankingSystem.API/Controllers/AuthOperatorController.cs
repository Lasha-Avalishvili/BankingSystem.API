using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Validations;

namespace BankingSystem.Features.InternetBank.Operator.AuthOperator
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class AuthOperatorController : ControllerBase
    {
        private TokenGenerator _tokenGenerator;
        private readonly IOperatorRepository _operatorRepository;
   

        public AuthOperatorController(TokenGenerator tokenGenerator, IOperatorRepository operatorRepository)
        {
            
            _operatorRepository = operatorRepository;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("register-operator")]
        public async Task<ActionResult<OperatorEntity>> RegisterOperator([FromBody] OperatorRegisterRequest request)
        {
            var exists = _operatorRepository.OperatorExists(request.PersonalNumber);
            if (!exists)
            {
                var registeredOperator = await _operatorRepository.RegisterOperatorAsync(request);
                await _operatorRepository.SaveChangesAsync();

                return Ok(registeredOperator);
            }
            return BadRequest("Operator with this personal number already exsists");
        }

        [HttpPost("login-operator")]
         public async Task<IActionResult> Login([FromBody] OperatorLoginRequest request)
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

    }
}
