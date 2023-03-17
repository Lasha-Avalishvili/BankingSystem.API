using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Features.InternetBank.User.Transactions
{
    
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionRepository transactionRepository, ITransactionService transactionService)
        {
           _transactionService = transactionService;
        }

        [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
        [HttpPost("money-transfer")]
        public async Task<IActionResult> TransactionFunds([FromBody] TransactionRequest transactionRequest)
        {
            var transaction = await _transactionService.TransferFunds(transactionRequest);

            return Ok(transaction);
        }
    }
}
