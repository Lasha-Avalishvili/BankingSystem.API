using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.Reports
{
    public interface IReportsRepository
    {
        public Task<List<TransactionEntity>> GetTransactionsAsync(DateTime date);
        public Task<int> GetUserCountAsync(DateTime date);
        public Task<decimal> GettotalCashoutInGELAsync();
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

        public async Task<decimal> GettotalCashoutInGELAsync()
        {
            var totalCashoutInGEL = await _db.Transactions
               .Where(t => t.TransactionType == TransactionType.ATM)
               .SumAsync(t => t.AmountInGEL);

            return totalCashoutInGEL;
        }
    }
}

