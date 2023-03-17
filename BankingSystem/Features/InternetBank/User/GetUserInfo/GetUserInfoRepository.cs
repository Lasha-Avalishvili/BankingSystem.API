using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.InternetBank.User.GetUserInfo
{
    public interface IGetUserInfoRepository
    {
        Task<List<AccountEntity>> GetUserAccountsAsync(int userId);
        Task<List<CardEntity>> GetUserCardsAsync(int userId);
        Task<decimal> GetUserBalanceAsync(string iban, int userId);
        Task<List<TransactionEntity>> GetUserAccountTransactionsAsync(string iban, int userId);

        //bool UserExists(string cardNumber);
    }
    public class GetUserInfoRepository : IGetUserInfoRepository
    {
        private readonly AppDbContext _db;

        public GetUserInfoRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<AccountEntity>> GetUserAccountsAsync(int userId)
        {
            var accounts = await _db.Accounts.Where(i => i.UserId == userId).ToListAsync();
            return accounts;
        }

        public async Task<List<CardEntity>> GetUserCardsAsync(int userId)
        {
            var cards = await _db.Cards.Where(i => i.AccountId == userId).ToListAsync();
            return cards;
        }

        public async Task<decimal> GetUserBalanceAsync(string iban, int userId)
        {
            var accounts = await _db.Accounts.Where(i => i.UserId == userId).ToListAsync();
            if (accounts == null)
            {
                throw new ArgumentException("Invalid account or user ID");
            }
            decimal balance = await _db.Accounts
                .Where(a => a.IBAN == iban && a.UserId == userId)
                .Select(a => a.Balance)
                .FirstOrDefaultAsync();

            return balance;
        }
        public async Task<List<TransactionEntity>> GetUserAccountTransactionsAsync(string iban, int userId)
        {
            var account = await _db.Accounts
                .Where(a => a.IBAN == iban && a.UserId == userId)
                .SingleOrDefaultAsync();

            if (account == null)
            {
                return new List<TransactionEntity>();
            }

            var transactions = await _db.Transactions
                .Where(t => t.SenderAccount == iban || t.RecipientAccount == iban)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return transactions;
        }
    }
}
