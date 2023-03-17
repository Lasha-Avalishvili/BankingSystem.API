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
        public Task<decimal> GetATMCashoutsCountAsync();

    }

    public class ReportsRepository : IReportsRepository
    {
        private readonly AppDbContext _db;
        public ReportsRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<List<TransactionEntity>> GetTransactionsAsync(DateTime date)
        {
            return _db.Transactions
                .Where(t => t.CreatedAt >= date)
                .ToListAsync();
        }

        public async Task<decimal> GetATMCashoutsCountAsync()
        {
            var result = await _db.Transactions
                .Where(t => t.TransactionType == TransactionType.ATM)
                .SumAsync(t => t.Amount);
            return result;
        }

        public async Task<int> GetUserCountAsync(DateTime date)
        {
            var userCount = await _db.Users.CountAsync(u => u.RegisteredAt >= date);
            return userCount;
        }

    }
}

