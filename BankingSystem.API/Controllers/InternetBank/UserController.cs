using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BankingSystem.Features.InternetBank.User.GetUserInfo;
using BankingSystem.Features.InternetBank.User.Transactions;
using BankingSystem.Features.InternetBank.Operator.AuthUser;
using BankingSystem.Features.InternetBank.User;

namespace BankingSystem.Features.InternetBank.Operator.AddUser
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IGetUserInfoRepository _getUserInfoRepository;
        private readonly ITransactionService _transactionService;
        private readonly GetUserInfoService _getUserInfoService;
        public UserController(IUserRepository userRepository, IGetUserInfoRepository getUserInfoRepository, ITransactionService transactionService, GetUserInfoService getUserInfoService)
        {
            _userRepository = userRepository;
            _getUserInfoRepository = getUserInfoRepository;
            _transactionService = transactionService;
            _getUserInfoService = getUserInfoService;
        }

        [HttpPost("login-user")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request)
        {
           var response = await _userRepository.LoginUserAsync(request);
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
        public async Task<IActionResult> GetUserCards(int accountId)
        {
            var authenticatedUserId = User.FindFirstValue("userId");
            var cards = await _getUserInfoService.GetCardsAsync(authenticatedUserId, accountId);
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
