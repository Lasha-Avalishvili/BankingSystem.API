using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.Reports
{
    public interface IReportsRepository
    {
        public Task<List<TransactionEntity>> GetTransactionsAsync(DateTime date);
        public Task<int> GetUserCountAsync(DateTime date);
        public Task<decimal> GettotalCashOutInGELAsync();
        public Task<decimal> GettotalCashOutInUSDAsync();
        public Task<decimal> GettotalCashOutInEURAsync();

    }

    public class ReportsRepository : IReportsRepository
    {
        private readonly AppDbContext _db;
        public ReportsRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<int> GetUserCountAsync(DateTime date)
        {
            var userCount = await _db.Users.CountAsync(i => i.RegisteredAt >= date);
            return userCount;
        }

        public Task<List<TransactionEntity>> GetTransactionsAsync(DateTime date)
        {
            return _db.Transactions
                .Where(t => t.CreatedAt >= date)
                .ToListAsync();
        }

        public async Task<decimal> GettotalCashOutInGELAsync() // needs convertation
        {
            var totalCashOutInGEL = await _db.Transactions
                .Where(t => t.TransactionType == TransactionType.ATM)
                .Where(t => t.CurrencyFrom == Currency.GEL)
                .SumAsync(t => t.Amount);

            return totalCashOutInGEL;
        }
        public async Task<decimal> GettotalCashOutInUSDAsync()
        {
            var totalCashOutInUSD = await _db.Transactions
               .Where(t => t.TransactionType == TransactionType.ATM)
               .Where(t => t.CurrencyFrom == Currency.USD)
               .SumAsync(t => t.Amount);

            return totalCashOutInUSD;
        }
        public async Task<decimal> GettotalCashOutInEURAsync()
        {
            var totalCashOutInEUR = await _db.Transactions
               .Where(t => t.TransactionType == TransactionType.ATM)
               .Where(t => t.CurrencyFrom == Currency.EUR)
               .SumAsync(t => t.Amount);

            return totalCashOutInEUR;
        }

    }
}

