using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.Reports
{
    public interface IGetStatRepository
    {
        Task<List<UserEntity>> GetCurrentYearRegisteredUsersAsync();
        Task<List<UserEntity>> GetLastYearRegisteredUsersAsync();
        Task<List<UserEntity>> GetRegisteredUsersLast30DayAsync();
        Task<long> GetTransactionsCountInLastMonthAsync();
        Task<long> GetTransactionsCountInLastSixMonthAsync();
        Task<long> GetTransactionsCountInLastOneYearAsync();
        Task<GetIncomeFromFeeResponse> GetIncomeFromLastMonthTransactionsAsync();
        Task<GetIncomeFromFeeResponse> GetFeeIncomeFromLastSixMOnthAsync();
        Task<GetIncomeFromFeeResponse> GetFeeIncomeFromLastOneYearAsync();

    }
    public class GetStatRepository : IGetStatRepository
    {
        private readonly AppDbContext _db;

        public GetStatRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<UserEntity>> GetCurrentYearRegisteredUsersAsync()
        {
            var currentYear = DateTime.Now.Year;
            var registredUsers = await _db.Users
                .Where(u => u.RegisteredAt.Year == currentYear)
                .ToListAsync();

            return registredUsers;
        }

        public async Task<List<UserEntity>> GetLastYearRegisteredUsersAsync()
        {
            var lastYear = DateTime.Now.Year - 1;
            var registeredUsersLastYear = await _db.Users
                .Where(u => u.RegisteredAt.Year == lastYear)
                .ToListAsync();

            return registeredUsersLastYear;
        }

        public async Task<List<UserEntity>> GetRegisteredUsersLast30DayAsync()
        {
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);
            var registeredUsersLast30Days = await _db.Users
                .Where(u => u.RegisteredAt >= thirtyDaysAgo)
                .ToListAsync();

            return registeredUsersLast30Days;
        }
        public async Task<long> GetTransactionsCountInLastMonthAsync()
        {
            var today = DateTime.Today;
            var firstDayOfLastMonth = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
            var lastDayOfLastMonth = firstDayOfLastMonth.AddMonths(1).AddDays(-1);
            var transactionsLastMonth = await _db.Transactions
                .Where(t => t.CreatedAt >= firstDayOfLastMonth && t.CreatedAt <= lastDayOfLastMonth)
                .ToListAsync();

            var transactionCountLastMonth = transactionsLastMonth.Count;

            return transactionCountLastMonth;
        }

        public async Task<long> GetTransactionsCountInLastSixMonthAsync()
        {
            var today = DateTime.Today;
            var firstDayOfSixthMonthAgo = new DateTime(today.Year, today.Month, 1).AddMonths(-6);
            var lastDayOfLastMonth = firstDayOfSixthMonthAgo.AddMonths(6).AddDays(-1);
            var transactionsLastSixMonths = await _db.Transactions
                .Where(t => t.CreatedAt >= firstDayOfSixthMonthAgo && t.CreatedAt <= lastDayOfLastMonth)
                .ToListAsync();

            var transactionCountLastSixMonths = transactionsLastSixMonths.Count;

            return transactionCountLastSixMonths;
        }

        public async Task<long> GetTransactionsCountInLastOneYearAsync()
        {
            var today = DateTime.Today;
            var firstDayOfLastYear = new DateTime(today.Year - 1, 1, 1);
            var lastDayOfLastMonth = firstDayOfLastYear.AddYears(1).AddDays(-1);
            var transactionsLastYear = await _db.Transactions
                .Where(t => t.CreatedAt >= firstDayOfLastYear && t.CreatedAt <= lastDayOfLastMonth)
                .ToListAsync();

            var transactionCountLastYear = transactionsLastYear.Count;

            return transactionCountLastYear;
        }
        public async Task <GetIncomeFromFeeResponse> GetIncomeFromLastMonthTransactionsAsync()
        {
            var startDate = DateTime.UtcNow.AddMonths(-1);
            var endDate = DateTime.UtcNow;

            var transactions = await _db.Transactions
                .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate).ToListAsync();

            var getIncomeStat = new GetIncomeFromFeeResponse();

            getIncomeStat.FeeInGel = transactions.Sum(t => t.FeeInGEL);
            getIncomeStat.FeeInUsd = transactions.Sum(t => t.FeeInUSD);
            getIncomeStat.FeeInEur = transactions.Sum(t => t.FeeInEUR);

            return getIncomeStat;
        }

        async public Task<GetIncomeFromFeeResponse> GetFeeIncomeFromLastSixMOnthAsync()
        {
            var startDate = DateTime.UtcNow.AddMonths(-6);
            var endDate = DateTime.UtcNow;

            var transactions = _db.Transactions
                .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate);

            var getIncomeStat = new GetIncomeFromFeeResponse();

            getIncomeStat.FeeInGel = transactions.Sum(t => t.FeeInGEL);
            getIncomeStat.FeeInUsd = transactions.Sum(t => t.FeeInUSD);
            getIncomeStat.FeeInEur = transactions.Sum(t => t.FeeInEUR);

            return getIncomeStat;
        }

        public async Task<GetIncomeFromFeeResponse> GetFeeIncomeFromLastOneYearAsync()
        {
            var startDate = DateTime.UtcNow.AddYears(-1);
            var endDate = DateTime.UtcNow;

            var transactions = _db.Transactions
                .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate);

            var getIncomeStat = new GetIncomeFromFeeResponse();

            getIncomeStat.FeeInGel = transactions.Sum(t => t.FeeInGEL);
            getIncomeStat.FeeInUsd = transactions.Sum(t => t.FeeInUSD);
            getIncomeStat.FeeInEur = transactions.Sum(t => t.FeeInEUR);

            return getIncomeStat;
        }
    }
}
