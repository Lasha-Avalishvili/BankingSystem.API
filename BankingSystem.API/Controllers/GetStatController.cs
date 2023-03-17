using BankingSystem.DB.Entities;
using BankingSystem.Features.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GetStatController : ControllerBase
    {
        private readonly IGetStatRepository _getStatRepository;

        public GetStatController(IGetStatRepository getStatRepository)
        {
            _getStatRepository = getStatRepository;
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpGet("get-registred-users-in-current-year")]
        async public Task<List<UserEntity>> GetCurrentYearRegisteredUsers()
        {
            var registeredUsers = await _getStatRepository.GetCurrentYearRegisteredUsersAsync(); 
            return registeredUsers;
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpGet("get-registred-users-in-last-year")]
        public async Task<List<UserEntity>> GetLastYearRegisteredUsers()
        {
            var registeredUsers = await _getStatRepository.GetLastYearRegisteredUsersAsync();
            return registeredUsers;
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpGet("get-registred-users-in-last-30-days")]
        public async Task<List<UserEntity>> GetRegisteredUsersLast30Day()
        {
            var registeredUsers = await _getStatRepository.GetRegisteredUsersLast30DayAsync();
            return registeredUsers;
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpGet("transactions-in-last-one-month")]
        public async Task<long> GetNumberOfTransactionsInLastMonth()
        {
            var numberOfTransactionsInLastMonth = await _getStatRepository.GetTransactionsCountInLastMonthAsync();
            return numberOfTransactionsInLastMonth;
        }

      
        [HttpGet("transactions-in-last-six-month")]
        public async Task<long> GetNumberOfTransactionsInLastSixMonth()
        {
            var numberOfTransactionsInLastMonth = await _getStatRepository.GetTransactionsCountInLastSixMonthAsync();
            return numberOfTransactionsInLastMonth;
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpGet("transactions-users-in-last-one-year")]
        public async Task<long> GetNumberOfTransactionsInLastOneYear()
        {
            var numberOfTransactionsInLastMonth = await _getStatRepository.GetTransactionsCountInLastOneYearAsync();
            return numberOfTransactionsInLastMonth;
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpGet("last-month-income-from-transactions")]
        public async Task<GetIncomeFromFeeResponse> GetIncomeFromLastMonthTransactions()
        {
             var incomeFromFee = await _getStatRepository.GetIncomeFromLastMonthTransactionsAsync();
            return incomeFromFee;
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpGet("last-six-month-income-from-fees")]
        public async Task<GetIncomeFromFeeResponse> GetFeeIncomeFromLastsixMonth()
        {
            var incomeFromFee = await _getStatRepository.GetFeeIncomeFromLastSixMOnthAsync();
            return incomeFromFee;
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")] 
        [HttpGet("last-one-year-income-from-fees")]
        public async Task<GetIncomeFromFeeResponse> GetFeeIncomeFromLastSixMOnth()
        {
            var lastYearIncomeFromFee = await _getStatRepository.GetFeeIncomeFromLastOneYearAsync();
            return lastYearIncomeFromFee;
        }
    }
}
