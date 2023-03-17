using BankingSystem.DB;
using BankingSystem.Features.ATM.Withdraw;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Features.ATM
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WithdrawATMController : ControllerBase
    {
        private readonly IWithdrawRepository _withdrawRepository;
        private readonly IWithdrawService _withdrawService;
        public WithdrawATMController(IWithdrawRepository withdrawRepository, IWithdrawService withdrawService)
        {
            _withdrawRepository = withdrawRepository;
            _withdrawService = withdrawService;
        }
        [HttpPost("withdraw")]
        public async Task<IActionResult> WithdrawATM([FromBody] WithdrawRequest withdrawRequest)
        {
            await _withdrawService.Withdraw(withdrawRequest);

            return Ok("Transfer successful");
        }
    }
}
