using System.Security.Claims;
using BankingSystem.DB;
using BankingSystem.Features.ATM.ChangePin;
using BankingSystem.Features.ATM.Withdraw;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers.ATM
{
    /// <summary>
    /// ChangeCardPINController had authentication on top, why?
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WithdrawATMController : ControllerBase
    {
        private readonly IWithdrawRepository _withdrawRepository;
        private readonly IWithdrawService _withdrawService;
        private readonly IChangeCardPINRepository _changePINRepository;
        public WithdrawATMController(IWithdrawRepository withdrawRepository, IWithdrawService withdrawService, IChangeCardPINRepository changePINRepository)
        {
            _withdrawRepository = withdrawRepository;
            _withdrawService = withdrawService;
            _changePINRepository = changePINRepository;
        }
        [HttpPost("withdraw")]
        public async Task<IActionResult> WithdrawATM([FromBody] WithdrawRequest withdrawRequest)
        {
            await _withdrawService.Withdraw(withdrawRequest);

            return Ok("Transfer successful");
        }

        [HttpPost("change-pin")]
        public async Task<string> ChangePIN([FromBody] ChangeCardPINRequest changeCardPINRequest)
        {
            var userId = HttpContext.User.FindFirstValue("userId");
            if (userId == null)
            {
                return "User Not Found";
            }

            var response = await _changePINRepository.ChangePINAsync(changeCardPINRequest, userId);

            return response;
        }
    }
}
