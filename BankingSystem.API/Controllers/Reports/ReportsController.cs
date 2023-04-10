using BankingSystem.Features.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers.Reports
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsRepository _reportsRepository;
        private readonly IReportsService _reportsService;
        private readonly DateTime firstDayOfYear = new DateTime(DateTime.Now.Year, 1, 1);
        private readonly DateTime lastYearSameDay = DateTime.UtcNow.AddYears(-1);
        private readonly DateTime last30Days = DateTime.UtcNow.AddDays(-30);
        private readonly DateTime last6Months = DateTime.UtcNow.AddMonths(-6);
        private readonly DateTime firstDay = new DateTime(1, 1, 1);

        public ReportsController(IReportsRepository reportsRepository, IReportsService reportsService)
        {
            _reportsRepository = reportsRepository;
            _reportsService = reportsService;
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpGet("get-registered-users-count")]
        public async Task<UsersCountResponse> GetUsersRegistered()
        {
            return await _reportsService.GetUsersRegistered(firstDayOfYear, lastYearSameDay, last30Days);
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpGet("get-transactions-count")]
        public async Task<TransactionsCountResponse> TransactionsCount()
        {
            return await _reportsService.GetTransactionsCount(last30Days, last6Months, lastYearSameDay);
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpGet("calculate-income")]
        public async Task<CalculateIncomeResponse> CalculateIncome()
        {
            return await _reportsService.CalculateIncome(last30Days, last6Months, lastYearSameDay);   
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpGet("calculate-average-transaction-fee")]
        public async Task<AverageTransactionFeeResponse> AverageTransactionFee()
        {
            return await _reportsService.CalculateAverageTransactionFee(firstDay);
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpGet("transaction-count-by-day-last-month")]
        public async Task<IActionResult> GetTransactionCountByDayLastMonth()
        {
            var transactionCounts = await _reportsService.GetTrasnactionsChart(last30Days);
            return Ok(transactionCounts);
        }

        [Authorize("ApiAdmin", AuthenticationSchemes = "Bearer")]
        [HttpGet("calculate-ATM-cashout")]
        public async Task<IActionResult> ATMCashouts()
        {
            var result = await _reportsService.GetTotalCashout();
            return Ok(result);
        }
    }
}
