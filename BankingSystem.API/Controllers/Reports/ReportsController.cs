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
        
        [HttpGet("get-registered-users-count")]
        public async Task<UsersCountResponse> GetUsersRegistered()
        {
            return await _reportsService.GetUsersRegistered(firstDayOfYear, lastYearSameDay, last30Days);
        }

        [HttpGet("get-transactions-count")]
        public async Task<TransactionsCountResponse> TransactionsCount()
        {
            return await _reportsService.GetTransactionsCount(last30Days, last6Months, lastYearSameDay);
        }

        [HttpGet("calculate-income")]
        public async Task<CalculateIncomeResponse> CalculateIncome()
        {
            return await _reportsService.CalculateIncome(last30Days, last6Months, lastYearSameDay);   
        }

        [HttpGet("calculate-average-transaction-fee")]
        public async Task<AverageTransactionFeeResponse> AverageTransactionFee()
        {
            return await _reportsService.CalculateAverageTransactionFee(firstDay);
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
