using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BankingSystem.Features.InternetBank.User.GetUserInfo;
using BankingSystem.Features.InternetBank.User.Transactions;
using BankingSystem.Features.InternetBank.Auth;
using BankingSystem.DB;

namespace BankingSystem.Features.InternetBank.Operator.AddUser
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly GetUserInfoService _getUserInfoService;
        private readonly AuthService _authService;
        
        public UserController(ITransactionService transactionService, GetUserInfoService getUserInfoService, AuthService authService)
        {
            _transactionService = transactionService;
            _getUserInfoService = getUserInfoService;
            _authService = authService;
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
        [HttpGet("get-accounts")]
        public async Task<IActionResult> GetUserAccounts()
        {
            var authenticatedUserId = User.FindFirstValue("userId");
            var accounts =await _getUserInfoService.GetAccountsAsync(authenticatedUserId);
            return Ok(accounts);
        }

        [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
        [HttpGet("get-cards")]
        public async Task<IActionResult> GetUserCards(string Iban)
        {
            var authenticatedUserId = User.FindFirstValue("userId");
            var cards = await _getUserInfoService.GetCardsAsync(authenticatedUserId, Iban);
            return Ok(cards);
        }

        [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
        [HttpGet("get-account-transactions")]
        public async Task<IActionResult> GetUserAccountTransactions(string iban)
        {
            var authenticatedUserId = User.FindFirstValue("userId");
            var transactions = await _getUserInfoService.GetTransactionsAsync(iban, authenticatedUserId);
            return Ok(transactions);
        }

        [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
        [HttpPost("money-transfer")]
        public async Task<IActionResult> TransactionFunds([FromBody] TransactionRequest transactionRequest)
        {
            var authenticatedUserId = User.FindFirstValue("userId");
            var transaction = await _transactionService.TransferFunds(transactionRequest, authenticatedUserId);
            return Ok(transaction);
        }

        
    }
}
