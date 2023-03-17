using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Operator.AddUser;
using IbanNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BankingSystem.Features.InternetBank.Operator.AddAccountForUser
{
    [Route("/api/v1/[controller]")]
    public class AddUserController : ControllerBase
    {
        private readonly  IAddUserRepository _addUserRepository;

        public AddUserController(IAddUserRepository addUserRepository)
        {
            _addUserRepository = addUserRepository;
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpPost("add-user-account")]
        public async Task<ActionResult<AccountEntity>> AddAccount([FromBody] AddAccountRequest request)
        {
            var isExists = _addUserRepository.AccountExists(request.IBAN);
            if (isExists == true)
            {
                return BadRequest("User Account With This IBN Already Exists");
            }
            IIbanValidator validator = new IbanValidator();
            ValidationResult validationResult = validator.Validate(request.IBAN);
            if (validationResult.IsValid)
            {
                var account = await _addUserRepository.AddAccountAsync(request);
                await _addUserRepository.SaveChangesAsync();

                return Ok(account);
            }
            return BadRequest("IBAN isn't correct");
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpPost("add-user-card")]
        public async Task<ActionResult<CardEntity>> AddCard([FromBody] AddCardRequest request)
        {
            var card = await _addUserRepository.AddCardAsync(request);
            await _addUserRepository.SaveChangesAsync();

            return Ok(card);
        }
    }
}
