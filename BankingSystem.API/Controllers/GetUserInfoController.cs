using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankingSystem.Features.InternetBank.User.GetUserInfo
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class GetUserInfoController : ControllerBase
    {
        private readonly IGetUserInfoRepository _getUserInfoRepository;

        public GetUserInfoController(IGetUserInfoRepository getUserInfoRepository)
        {
            _getUserInfoRepository = getUserInfoRepository;
        }

        [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
        [HttpGet("get-user-accounts")]
        public async Task<IActionResult> GetUserAccounts(int userId)
        {
            var authenticatedUserId = User.FindFirstValue("userId");

            if (authenticatedUserId == null || int.Parse(authenticatedUserId) != userId) 
            {
                return NotFound();
            }

            var accounts = await _getUserInfoRepository.GetUserAccountsAsync(userId);
            return Ok(accounts);
        }

        [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
        [HttpGet("get-user-cards")]
        public async Task<IActionResult> GetUserCards(int userId)
        {
            var authenticatedUserId = User.FindFirstValue("userId");

            if (authenticatedUserId == null || int.Parse(authenticatedUserId) != userId)
            {
                return NotFound();
            }

            var cards = await _getUserInfoRepository.GetUserCardsAsync(userId);
            return Ok(cards);
        }

        [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
        [HttpGet("get-account-balance")]
        public async Task<IActionResult> GetUserBalance(string iban, int userId)
        {
            var authenticatedUserId = User.FindFirstValue("userId");

            if (authenticatedUserId == null || int.Parse(authenticatedUserId) != userId)
            {
                return NotFound("Invalid userId or account");
            }
            else
            {
               var balance = await _getUserInfoRepository.GetUserBalanceAsync(iban, userId);
               return Ok(balance);
            }
            
        }

        [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
        [HttpGet("get-account-transactions")]
        public async Task<IActionResult> GetUserAccountTransactions(string iban, int userId)
        {
            var authenticatedUserId = User.FindFirstValue("userId");

            if (authenticatedUserId == null || int.Parse(authenticatedUserId) != userId)
            {
                return NotFound("Invalid userId or account");
            }
            else
            {
                var transactions = await _getUserInfoRepository.GetUserAccountTransactionsAsync(iban, userId);
                return Ok(transactions);
            }
        }
    }
}
