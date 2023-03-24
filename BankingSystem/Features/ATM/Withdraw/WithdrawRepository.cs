using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;


namespace BankingSystem.Features.ATM.Withdraw
{
    public interface IWithdrawRepository
    {
        public Task<CardEntity> AuthorizeCardAsync(string cardNumber, string PIN);
        public Task<AccountEntity> FindAccountAsync(int accountId);
        public Task<decimal> GetUserAtmTransactions(int userId, Currency currency);
        public Task SaveChangesAsync(TransactionEntity transaction);
    }

    public class WithdrawRepository : IWithdrawRepository
    {
        private readonly AppDbContext _db;

        public WithdrawRepository(AppDbContext db)
        {
            _db = db;
        }
        
        public async Task<CardEntity> AuthorizeCardAsync(string cardNumber, string PIN)
        {
            var card = await _db.Cards.FirstOrDefaultAsync(c => c.CardNumber == cardNumber && c.PIN == PIN);
            return card;
        } 

        public async Task<AccountEntity> FindAccountAsync(int accountId)
        {
            var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            return account;
        }

        public async Task<decimal> GetUserAtmTransactions(int userId, Currency currency) 
        {
            var ATMCashout = await _db.Transactions
           .Join(_db.Accounts,
                t => t.SenderAccountId,
                a => a.Id,
           (t, a) => new { Transaction = t, Account = a })
        .Where(x => x.Account.UserId == userId) 
        .Where(x => x.Transaction.TransactionType == TransactionType.ATM)
        .Where(x => x.Transaction.CurrencyFrom == currency)
        .Where(x => x.Transaction.CreatedAt > DateTime.UtcNow.AddHours(-24))
        .SumAsync(x => x.Transaction.Amount);

            return ATMCashout;
            
        }

        public async Task SaveChangesAsync(TransactionEntity transaction)
        {
            await _db.Transactions.AddAsync(transaction);
            await _db.SaveChangesAsync();
        }
    }
}
