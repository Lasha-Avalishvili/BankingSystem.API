using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.DB.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Features.Reports
{
    public interface IReportsService
    {
        public Task<(int inner, int outer, int ATM, int all)> CountTransactions(DateTime date);
        public Task<IncomeSummary> CalculateFees(DateTime date);
        public Task<Dictionary<DateTime, int>> GetTrasnactionsChart(DateTime date);
        public Task<UsersCountResponse> GetUsersRegistered(DateTime firstDayOfYear, DateTime lastYearSameDay, DateTime last30Days); // new
        public Task<TransactionsCountResponse> GetTransactionsCount(DateTime last6Months, DateTime lastYearSameDay, DateTime last30Days); // new
        public Task<CalculateIncomeResponse> CalculateIncome(DateTime last6Months, DateTime lastYearSameDay, DateTime last30Days);  /// new

    }
    public class ReportsService : IReportsService
    {
        private readonly IReportsRepository _reportsRepository;
        public ReportsService(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        public async Task<(int inner, int outer, int ATM, int all)> CountTransactions(DateTime date)
        {
            var transactions = await _reportsRepository.GetTransactionsAsync(date);
            var groupedtransactions = transactions.GroupBy(t => t.TransactionType)
                .Select(g => new
                {
                    TransactionType = g.Key,
                    Count = g.Count()
                });

            var all = groupedtransactions.Sum(t => t.Count);
            var inner = groupedtransactions.FirstOrDefault(t => t.TransactionType == TransactionType.Inner)?.Count ?? 0;
            var outer = groupedtransactions.FirstOrDefault(t => t.TransactionType == TransactionType.Outer)?.Count ?? 0;
            var ATM = groupedtransactions.FirstOrDefault(t => t.TransactionType == TransactionType.ATM)?.Count ?? 0;

            return (inner, outer, ATM, all);
        }

        public async Task<IncomeSummary> CalculateFees(DateTime date)
        {
            var transactions = await _reportsRepository.GetTransactionsAsync(date);
            var transactionFees = transactions.Select(t => new
            {
                t.FeeInUSD,
                t.FeeInGEL,
                t.FeeInEUR
            })
                .ToList();

            var allFeeInUSD = transactionFees.Sum(t => t.FeeInUSD);
            var allFeeInGEL = transactionFees.Sum(t => t.FeeInGEL);
            var allFeeInEUR = transactionFees.Sum(t => t.FeeInEUR);

            var totalCount = transactionFees.Count();
            var avgFeeInGEL = totalCount > 0 ? allFeeInGEL / totalCount : 0;
            var avgFeeInUSD = totalCount > 0 ? allFeeInUSD / totalCount : 0;
            var avgFeeInEUR = totalCount > 0 ? allFeeInEUR / totalCount : 0;

            return new IncomeSummary
            {
                feeInGEL = allFeeInGEL,
                feeInUSD = allFeeInUSD,
                feeInEUR = allFeeInEUR,
                avgFeeInGEL = avgFeeInGEL,
                avgFeeInUSD = avgFeeInUSD,
                avgFeeInEUR = avgFeeInEUR
            };
        }

        public async Task<Dictionary<DateTime, int>> GetTrasnactionsChart(DateTime date)
        {
            var transactions = await _reportsRepository.GetTransactionsAsync(date);
            var transactionCountByDay = new Dictionary<DateTime, int>();

            for(var startdate = date; date <= DateTime.UtcNow; date = date.AddDays(1))
            {
                var transactionsCount = transactions.Count(x => x.CreatedAt.Date== date.Date);
                transactionCountByDay.Add(date, transactionsCount);
            }

            return transactionCountByDay;
        }

        public async Task <UsersCountResponse> GetUsersRegistered (DateTime firstDayOfYear, DateTime lastYearSameDay, DateTime last30Days)  /// new
        {
            var userCount1 = await _reportsRepository.GetUserCountAsync(firstDayOfYear);
            var userCount2 = await _reportsRepository.GetUserCountAsync(lastYearSameDay);
            var userCount3 = await _reportsRepository.GetUserCountAsync(last30Days);
            return new UsersCountResponse { UsersInLast30Days = userCount3, UsersInOneYear = userCount2, UsersThisYear= userCount3 };

        }

        public async Task<TransactionsCountSummary> CountTransactionsB(DateTime date) // new
        {
            var transactions = await _reportsRepository.GetTransactionsAsync(date);
            var groupedtransactions = transactions.GroupBy(t => t.TransactionType)
                .Select(g => new
                {
                    TransactionType = g.Key,
                    Count = g.Count()
                });

            var all = groupedtransactions.Sum(t => t.Count);
            var inner = groupedtransactions.FirstOrDefault(t => t.TransactionType == TransactionType.Inner)?.Count ?? 0;
            var outer = groupedtransactions.FirstOrDefault(t => t.TransactionType == TransactionType.Outer)?.Count ?? 0;
            var ATM = groupedtransactions.FirstOrDefault(t => t.TransactionType == TransactionType.ATM)?.Count ?? 0;

            return new TransactionsCountSummary { ATM= ATM, inner = inner, outer = outer, total = all };
        }
        public async Task<TransactionsCountResponse> GetTransactionsCount(DateTime last6Months, DateTime lastYearSameDay, DateTime last30Days)  /// new
        {
            var result1 = await CountTransactionsB(lastYearSameDay);    
            var result2 = await CountTransactionsB(last6Months);
            var result3 = await CountTransactionsB(last30Days);

            return new TransactionsCountResponse { UsersInLast30Days = result1, UsersInOneYear = result2, UsersThisYear = result3 };
        }

        public async Task<IncomeCurrencies> CalculateFeesB(DateTime date)
        {
            var transactions = await _reportsRepository.GetTransactionsAsync(date);
            var transactionFees = transactions.Select(t => new
            {
                t.FeeInUSD,
                t.FeeInGEL,
                t.FeeInEUR
            })
                .ToList();

            var allFeeInUSD = transactionFees.Sum(t => t.FeeInUSD);
            var allFeeInGEL = transactionFees.Sum(t => t.FeeInGEL);
            var allFeeInEUR = transactionFees.Sum(t => t.FeeInEUR);

            return new IncomeCurrencies
            {
                feeInGEL = allFeeInGEL,
                feeInUSD = allFeeInUSD,
                feeInEUR = allFeeInEUR,
            };
        }

        public async Task<CalculateIncomeResponse> CalculateIncome(DateTime last6Months, DateTime lastYearSameDay, DateTime last30Days)  /// new
        {
            var result1 = await CalculateFeesB(lastYearSameDay);
            var result2 = await CalculateFeesB(last6Months);
            var result3 = await CalculateFeesB(last30Days);

            return new CalculateIncomeResponse { UsersInLast30Days = result1, UsersInOneYear = result2, UsersThisYear = result3 };
        }


    }

    public class IncomeSummary
    {
        public decimal feeInGEL { get; set; }
        public decimal feeInUSD { get; set; }
        public decimal feeInEUR { get; set; }
        public decimal avgFeeInGEL { get; set; }
        public decimal avgFeeInUSD { get; set; }
        public decimal avgFeeInEUR { get; set; }
    }

    public class TransactionsCountSummary
    {
        public int inner { get; set; }
        public int outer { get; set; }
        public int ATM { get; set; }
        public int total { get; set; }
    }

    public class IncomeCurrencies
    {
        public decimal feeInGEL { get; set; }
        public decimal feeInUSD { get; set; }
        public decimal feeInEUR { get; set; }
    }
 
}

