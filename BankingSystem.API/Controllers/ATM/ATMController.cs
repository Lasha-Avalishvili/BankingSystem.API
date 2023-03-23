using System.Security.Claims;
using BankingSystem.DB;
using BankingSystem.Features.ATM.ChangePin;
using BankingSystem.Features.ATM.Withdraw;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers.ATM
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WithdrawATMController : ControllerBase
    {
        private readonly IWithdrawRepository _withdrawRepository;
        private readonly IWithdrawService _withdrawService;
        private readonly IChangePinService _changePinService;
        public WithdrawATMController(IWithdrawRepository withdrawRepository, IWithdrawService withdrawService, IChangePinService changePinService)
        {
            _withdrawRepository = withdrawRepository;
            _withdrawService = withdrawService;
            _changePinService = changePinService;
        }
        [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
        [HttpPost("withdraw")]
        public async Task<IActionResult> WithdrawATM([FromBody] WithdrawRequest withdrawRequest)
        {
           var result = await _withdrawService.Withdraw(withdrawRequest);

            return Ok(result);
        }

        [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
        [HttpPost("change-pin")]
        public async Task<IActionResult> ChangePIN([FromBody] ChangeCardPinRequest changeCardPINRequest)
        {
            var authenticatedUserId = User.FindFirstValue("userId");
            if (authenticatedUserId != null)
            {
                var response = await _changePinService.ChangePin(changeCardPINRequest, authenticatedUserId);
                return Ok(response);
            }
            return BadRequest();
        }
    }
}
