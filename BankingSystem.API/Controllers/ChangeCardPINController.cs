using BankingSystem.Features.ATM.ChangePin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace BankingSystem.API.Controllers
{
    [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ChangeCardPINController : ControllerBase
    {
        private readonly IChangeCardPINRepository _changePINRepository;

        public ChangeCardPINController(IChangeCardPINRepository changePINRepository)
        {
            _changePINRepository = changePINRepository;
        }
        [HttpPost("change-card-pin")]
        
        public async Task<string> ChangePIN([FromBody]ChangeCardPINRequest changeCardPINRequest)
        {
            var userId = HttpContext.User.FindFirstValue("userId");
            if (userId == null)
            {
                return ("User Not Found");
            }

            var response = await _changePINRepository.ChangePINAsync(changeCardPINRequest, userId);

            return response;
        }
    }
}
