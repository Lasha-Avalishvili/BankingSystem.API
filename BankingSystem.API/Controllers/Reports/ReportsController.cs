using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.API.Controllers.Reports
{
    public class ReportsController : ControllerBase
    {
        private readonly IReportsRepository _reportsRepository;
        private readonly IReportsService _reportsService;
        private readonly DateTime last30Days = DateTime.UtcNow.AddDays(-30);
        private readonly DateTime last6Months = DateTime.UtcNow.AddMonths(-6);
        private readonly DateTime lastYearSameDay = DateTime.UtcNow.AddYears(-1);
        private readonly DateTime firstDayOfYear = new DateTime(DateTime.Now.Year, 1, 1);
        private readonly DateTime firstDay = new DateTime(1, 1, 1);


        public ReportsController(IReportsRepository reportsRepository, IReportsService reportsService)
        {
            _reportsRepository = reportsRepository;
            _reportsService = reportsService;
        }

        [HttpGet("get-users-registered-this-year")]
        public async Task<ActionResult<int>> UsersThisYear()
        {
            var userCount = await _reportsRepository.GetUserCountAsync(firstDayOfYear);
            return userCount;
        }

        [HttpGet("get-users-registered-in-one-year")]
        public async Task<ActionResult<int>> UsersInOneYear()
        {
            var userCount = await _reportsRepository.GetUserCountAsync(lastYearSameDay);
            return userCount;
        }

        [HttpGet("get-users-registered-in-last-30-days")]
        public async Task<ActionResult<int>> UsersInLastMonth()
        {
            var userCount = await _reportsRepository.GetUserCountAsync(last30Days);
            return userCount;
        }

        [HttpGet("get-transactions-made-in-last-30-days")]
        public async Task<ActionResult<string>> TransactionsLastMonth()
        {
            var result = await _reportsService.CountTransactions(last30Days);
            var message = $"All transactions made in the last 30 days are {result.all}, from these transactions {result.inner} are internal, {result.outer} are external, and {result.ATM} are ATM.";

            return Ok(message);
        }

        [HttpGet("get-transactions-made-in-last-6-months")]
        public async Task<ActionResult<string>> TransactionsLast6Month()
        {
            var result = await _reportsService.CountTransactions(last6Months);
            var message = $"All transactions made in the last 6 months are {result.all}, from these transactions {result.inner} are internal, {result.outer} are external, and {result.ATM} are ATM.";

            return Ok(message);
        }

        [HttpGet("get-transactions-made-in-last-year")]
        public async Task<ActionResult<string>> TransactionsLastYear()
        {
            var result = await _reportsService.CountTransactions(lastYearSameDay);
            var message = $"All transactions made in the last year are {result.all}, from these transactions {result.inner} are internal, {result.outer} are external, and {result.ATM} are ATM.";

            return Ok(message);
        }

        [HttpGet("get-income-in-last-30-days")]
        public async Task<ActionResult<string>> IncomeLastMonth()
        {
            var result = await _reportsService.CalculateFees(last30Days);
            var message = $"All transaction fees made in the last 30 days are {result.feeInGEL} GEL, {result.feeInUSD} $ and {result.feeInEUR} EUR";

            return Ok(message);
        }

        [HttpGet("get-income-in-last-6-months")]
        public async Task<ActionResult<string>> IncomeLast6Month()
        {
            var result = await _reportsService.CalculateFees(last6Months);
            var message = $"All transaction fees made in the last 6 months are {result.feeInGEL} GEL, {result.feeInUSD} $ and {result.feeInEUR} EUR";

            return Ok(message);
        }

        [HttpGet("get-income-in-last-year")]
        public async Task<ActionResult<string>> IncomeLastYear()
        {
            var result = await _reportsService.CalculateFees(lastYearSameDay);
            var message = $"All transaction fees made in the last year are {result.feeInGEL}  GEL,  {result.feeInUSD}  $ and  {result.feeInEUR} EUR";

            return Ok(message);
        }

        [HttpGet("get-average-transaction-fee")]
        public async Task<ActionResult<string>> AverageTransactionFee()
        {
            var result = await _reportsService.CalculateFees(firstDay);
            var message = $"Average transaction fees are {result.avgFeeInGEL} GEL, {result.avgFeeInUSD} USD, and {result.avgFeeInEUR} € ";

            return Ok(message);
        }

        [HttpGet("transaction-count-by-day-last-month")]
        public async Task<IActionResult> GetTransactionCountByDayLastMonth()
        {
            var transactionCounts = await _reportsService.GetTrasnactionsChart(last30Days);
            return Ok(transactionCounts);
        }

        [HttpGet("get-ATM-cashout-count")]
        public async Task<ActionResult<decimal>> ATMCashouts()
        {
            var result = await _reportsRepository.GetATMCashoutsCountAsync();

            return result;
        }

    }
}
