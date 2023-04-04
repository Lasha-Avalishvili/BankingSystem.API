using System.Security.Claims;
using BankingSystem.DB;
using BankingSystem.Features.ATM.AccountBlance;
using BankingSystem.Features.ATM.ChangePin;
using BankingSystem.Features.ATM.GetBalance;
using BankingSystem.Features.ATM.Withdraw;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers.ATM
{
    [ApiController]
    [Route("api/[controller]")]
    public class WithdrawATMController : ControllerBase
    {
      
        private readonly IWithdrawService _withdrawService;
        private readonly IChangePinService _changePinService;
        private readonly IGetBalanceService _getBalanceService;
        public WithdrawATMController(IWithdrawService withdrawService, IChangePinService changePinService, IGetBalanceService getBalanceService)
        {
            _withdrawService = withdrawService;
            _changePinService = changePinService;
            _getBalanceService = getBalanceService;
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> WithdrawATM([FromBody] WithdrawRequest request)
        {
            var result = await _withdrawService.WithdawFromAtm(request);

            return Ok(result);
        }

        [HttpPost("change-pin")]
        public async Task<IActionResult> ChangePIN([FromBody] ChangeCardPinRequest changeCardPINRequest)
        {
                var response = await _changePinService.ChangePin(changeCardPINRequest);
                return Ok(response);
        }

        [HttpPost("get-balance")]
        public async Task<IActionResult> ChangePIN([FromBody] GetBalanceRequest request)
        {
            var response = await _getBalanceService.GetBalance(request);
            return Ok(response);
        }
    }
}
