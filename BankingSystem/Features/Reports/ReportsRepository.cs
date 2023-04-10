using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.Reports
{
    public interface IReportsRepository
    {
        public Task<List<TransactionEntity>> GetTransactionsAsync(DateTime date);
        public Task<int> GetUsersCountAsync(DateTime date);
        public Task<decimal> GettotalCashoutInGELAsync();
    }

    public class ReportsRepository : IReportsRepository
    {
        private readonly AppDbContext _db;
        public ReportsRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<int> GetUsersCountAsync(DateTime date)
        {
            var userCount = await _db.Users
                .Join(_db.UserRoles, 
                u => u.Id,
                a => a.UserId,
           (u, a) => new { Users = u, UserRoles = a })
                .Where(x=>x.UserRoles.RoleId==2)
                .CountAsync(x => x.Users.RegisteredAt >= date);
          
            return userCount;
        }

        public async Task<List<TransactionEntity>> GetTransactionsAsync(DateTime date)
        {
            return await _db.Transactions
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

