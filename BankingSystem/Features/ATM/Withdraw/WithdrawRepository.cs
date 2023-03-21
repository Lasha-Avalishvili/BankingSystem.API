using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;


namespace BankingSystem.Features.ATM.Withdraw
{
    public interface IWithdrawRepository
    {
        Task<AccountEntity> GetSenderAccountAsync(WithdrawRequest withdrawRequest);
        Task<List<TransactionEntity>> GetCurrentDayTransactionsForUser(long userId);
        Task SaveChangesAsync(TransactionEntity transaction);
    }

    public class WithdrawRepository : IWithdrawRepository
    {
        private readonly AppDbContext _db;

        public WithdrawRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<AccountEntity> GetSenderAccountAsync(WithdrawRequest withdrawRequest)
        {
            var card = await _db.Cards.FirstOrDefaultAsync(c => c.CardNumber == withdrawRequest.CardNumber && c.PIN == withdrawRequest.PIN);
            var senderAccount = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == card.AccountId);
       
            return senderAccount;
        }

        public async Task<List<TransactionEntity>> GetCurrentDayTransactionsForUser(long userId)
        {
            var last24Hours = DateTime.UtcNow.AddHours(-24);
            var transactions = await _db.Transactions
                .Where(t => t.SenderAccountId == userId && t.CreatedAt >= last24Hours)
                .ToListAsync();

            return transactions;
        }
        public async Task SaveChangesAsync(TransactionEntity transaction)
        {
            await _db.Transactions.AddAsync(transaction);
            await _db.SaveChangesAsync();
        }
    }
}
